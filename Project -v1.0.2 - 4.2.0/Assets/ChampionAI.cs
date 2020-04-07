using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAI : MonoBehaviour
{
    enum CurrentState { Dodging, MovingToAttack, Attacking, MovingToCast, Casting}
    enum MetaStates { Normal, Shielded}

    [HideInInspector]
    public UnitManager myManager;
    UnitStats myStats;

    [Range(0,1)]
    public float Accuracy =1;

    [Range(0,.5f)]
    public float ReactionTime = .2f;

    [Tooltip("Time it takes to make a click on the perfect spot.")]
    [Range(.01f, .5f)]
    public float ClickPerfectionTime;

    [Range(0, 1)]
    [Tooltip("0 - 100% chance at picking the right target vs a random target")]
    public float TargetAquisitionSkill = 1;

    [Tooltip("How well the AI is aware of how accurate its actions are, IE, if it has time to dodge, if it will be hit by something")]
    [Range(0, 1)]
    public float selfConsciousness;


    Coroutine currentActionItem;
    UnitManager currentTarget;
    float TargetsPriority;

    private void Start()
    {
        myManager = GetComponent<UnitManager>();
        myStats = myManager.myStats;
        AbilityHeatMap.main.AddChampionListener(this);
    }

    float NextSpellCastTime;
    float[] LastSpellCastTime = new float[8];
    float nextUpdateTime;
    private void Update()
    {
        if (Time.time > nextUpdateTime || myManager.getState() is DefaultState)
        {
            if (currentActionItem != null)
            {
                return;
            }
            nextUpdateTime += .07f;

            currentTarget = EvaluateAutoAttack();
            //Debug.Log("Checking " + currentTarget);
            if (currentTarget)
            {
                if (Time.time > NextSpellCastTime)
                {
                    Ability ab = null;
                    if (Random.value < .5f)
                    {
                        ab = GetDamageAbility();
                    }
                    else
                    {
                        ab = GetSummonAbility();
                    }
                    if (ab)
                    {
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(CastSpell(currentTarget.transform.position, currentTarget.gameObject, myManager.abilityList.IndexOf(ab), ReactionTime));
                        NextSpellCastTime = Time.time + Random.Range(2, 4);
                        return;
                    }
        
                }



                if (myManager.myWeapon[0].inRange(currentTarget))
                {
                    StopAllCoroutines();
                    //currentActionItem = StartCoroutine(AttackTarget(currentTarget, ReactionTime));
                    myManager.GiveOrder(Orders.CreateAttackOrder(currentTarget.gameObject));
                }
                else
                {                   
                    Vector3 Direction = (currentTarget.transform.position - transform.position).normalized * 4;
                    Vector3 ToCheck = transform.position + Direction;
                    // This causes the champion to jitter in their movement, need to fix
                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(ToCheck, ReactionTime));
                        return;
                    }
                    ToCheck = transform.position + Quaternion.Euler(0, 55, 0) * Direction;
                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(ToCheck, ReactionTime));
                        return;
                    }
                    ToCheck = transform.position + Quaternion.Euler(0, -55, 0) * Direction;
                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(ToCheck, ReactionTime));
                        return;
                    }
                }
            }
        } 
    }




    Vector3 AlterWithAccuracy(Vector3 TargetSpot, float leewayTime, out float TimeTaken)
    {
        // If we have enough time, we can be perfectly accurate.
        TimeTaken = Mathf.Max(leewayTime, ClickPerfectionTime);
        float instanceAccuracy = Accuracy + ((1-Accuracy) * TimeTaken / ClickPerfectionTime);

        float radius = Random.Range(0, 1 - instanceAccuracy) * 5;
        float angle = Random.Range(0, 360);

        TargetSpot.x += Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
        TargetSpot.z += Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
        return TargetSpot;
    }

    public float AlterWithConsciousness( float toAlter)
    {
        return  toAlter + Random.Range(-.5f, .5f) * (1 - selfConsciousness);
    }


   
    UnitManager EvaluateAutoAttack()
    {
        UnitManager Best = currentTarget;

        // A good target assessor will constantly re-evaluate his target, a worse one might stick with what he has.
        if (Best && Random.value > TargetAquisitionSkill)
        {
            return Best;
        }

        float WeaponDamage = myManager.myWeapon[0].baseDamage;
        UnitManager LowestHealthGuy = null;
        float targetPriority = 1; // can kill minion= 3, unit has lowest life = 2, isHero = *2
        
        foreach (UnitManager manag in myManager.enemies)
        {
            if (myManager.isValidTarget(manag))
            {
                float priorityScore = 1;
                if (manag.myStats.health < WeaponDamage)
                {
                    priorityScore = 1;
                }
                else if (!LowestHealthGuy || myManager.myStats.health < LowestHealthGuy.myStats.health)
                {
                    LowestHealthGuy = manag;
                    priorityScore = 2;
                }

                if (manag.myStats.isUnitType(UnitTypes.UnitTypeTag.Hero))
                {
                    priorityScore *= 2;
                }

                priorityScore *= Random.Range(TargetAquisitionSkill, 1);
                if (priorityScore > targetPriority || (priorityScore == targetPriority && manag == LowestHealthGuy))
                {
                    targetPriority = priorityScore;
                    Best = manag;
                }
            }
        }
        return Best;
    }

    //============================== Reacting to dangerous things ====================================

    public void InSafeZone()
    {
        InDanger = 0;
    }

    int InDanger;
    public void InDangerZone(List<AbilityHeatMap.DangerZone> MyZones)
    {
        if (InDanger  != MyZones.Count)
        {
            InDanger = MyZones.Count;
            if (InDanger > 0)
            {
                /*
                //Debug.Log("NOw in danger zone");
                for (float j = 4; j < 40; j += 4)
                {
                    for (float i = 0; i < 360; i += 36)
                    {
                        Vector3 toCheck = transform.position + Quaternion.Euler(0, i, 0) * (Vector3.forward * j);
                        if (AbilityHeatMap.main.IsSafe(MyZones, toCheck, this))
                        {
                            Ability movementAb;
                            bool CanRun = CanRunAway(toCheck, 10, Vector3.Distance(transform.position, toCheck), out movementAb);
                            if (CanRun)
                            {
                                if (movementAb)
                                {
                                    myManager.UseAbility(myManager.abilityList.IndexOf(movementAb), false);
                                }
                                StopAllCoroutines();
                                currentActionItem = StartCoroutine(MoveToSpot(toCheck, ReactionTime));
                            }
                            return;
                        }
                    }
                }     */

                Ability Ab = CanBlock();
                if (Ab != null)
                {
                    StopAllCoroutines();
                    currentActionItem = StartCoroutine(CastSpell(transform.position, this.gameObject, myManager.abilityList.IndexOf(Ab), ReactionTime));
                }
                else
                {
                    Vector3 safeZone = FindSafePerpendicular(currentTarget);
                    if (transform.position != safeZone)
                    {
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(safeZone, ReactionTime));
                    }
                }
            }
        }
    }


    public Vector3 FindSafePerpendicular(UnitManager target)
    {
        float MaxRunDistance = myManager.cMover.MaxSpeed * 1.5f;

        // Tries to find a safe perpendicular zone to run to, if not, it does concentric circles
        if (target)
        {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            Vector3 PerpdicularDir = Quaternion.Euler(0, 90, 0) * targetDirection;

            for (float i = 4; i < MaxRunDistance; i++)
            {
                Vector3 RightPos = transform.position + PerpdicularDir * i;
                if (AbilityHeatMap.main.IsSafe(RightPos, this))
                {
                    return RightPos;
                }
                else
                {
                    Vector3 LeftPos = transform.position - PerpdicularDir * i;
                    if (AbilityHeatMap.main.IsSafe(LeftPos, this))
                    {
                        return LeftPos;
                    }
                }
            }
        }
        for (float j = 4; j < MaxRunDistance; j += 5)
        {
            for (float i = 0; i < 360; i += 45)
            {
                Vector3 toCheck = transform.position + Quaternion.Euler(0, i, 0) * (Vector3.forward * j);
                if (AbilityHeatMap.main.IsSafe(toCheck, this))
                {                                      
                    return toCheck;
                }
            }
        }
        return transform.position;
    }


    public void SeeCircleWarningZone(Vector3 point, float TimeTilStrike, float radius)
    {
        float distanceToCenter = Vector3.Distance(point, transform.position) - myManager.CharController.radius;      
        distanceToCenter = AlterWithConsciousness(distanceToCenter);
        if (distanceToCenter > radius)
        {   // Its going to miss me, don't need to do anything
            return;
        }

        float distanceToRun = (radius - distanceToCenter);
        // We figure out the optimal place to run
        Vector3 ToRunTo = transform.position + (transform.position - point).normalized * distanceToRun;
        Debug.Log(transform.position + " ---- " + ToRunTo + "   " + distanceToRun);

        // How much leftover time do we have before it strikes?
        float LeewayTime = AlterWithConsciousness(TimeTilStrike - (distanceToRun / myManager.cMover.MaxSpeed) - ReactionTime);

        float DecisionTimeTaken = 0;

        //How much time will it take to get perfect accuracy, and if there isn't, what is our accuracy?
        ToRunTo = AlterWithAccuracy(ToRunTo, LeewayTime, out DecisionTimeTaken);

        Debug.Log( "Later " + ToRunTo + "   " + Vector3.Distance(transform.position, ToRunTo));

        Ability movementAb;
        bool CanRun = CanRunAway(ToRunTo, TimeTilStrike, distanceToRun, out movementAb);

        Ability BlockAb = CanBlock();
        if (BlockAb && Random.value > .5f)
        {
            Debug.Log("Casting escape ability");
            StopAllCoroutines();
            currentActionItem = StartCoroutine(CastSpell(ToRunTo, gameObject, myManager.abilityList.IndexOf(BlockAb), ReactionTime));
        }
        else {
            if (CanRun)
            {
                if (movementAb)
                {
                    myManager.UseAbility(myManager.abilityList.IndexOf(movementAb), false);
                }
                StopAllCoroutines();
                currentActionItem = StartCoroutine(MoveToSpot(ToRunTo, ReactionTime));
            }
        }
    }

    bool CanRunAway(Vector3 optimalMoveLocation,float TimeTilStrike, float distanceToRun, out Ability toCast)
    {
        toCast = null;

        if (distanceToRun < myManager.cMover.MaxSpeed * TimeTilStrike)
        {
            return true;
        }
        {
            foreach (Ability ab in myManager.abilityList)
            {
                if (ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab)))
                {
                    if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.FriendlyMovement))
                    { // need to check if it give me enough speed to run out
                        toCast = ab;
                        return true;
                    }
                }
            }
        }
        return false;  //Can't possibly run away
    }

    Ability CanBlock()
    {
        foreach (Ability ab in myManager.abilityList)
        {
            if (ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab)))
            {
                if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.ReactiveDefense))
                {
                    return ab;
                }
            }
        }
        return null;
    }

    public Ability GetDamageAbility()
    {
        int index = 0;
        foreach (Ability ab in myManager.abilityList)
        {
            if ((ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab))) && LastSpellCastTime[index] + 6 < Time.time  )
            {
                if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.OffensiveDamage))
                {
                    return ab;
                }
            }
            index++;
        }
        return null;
    }

    public Ability GetSummonAbility()
    {
        int index = 0;
        foreach (Ability ab in myManager.abilityList)
        {
            if ((ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab))) && LastSpellCastTime[index] + 6 < Time.time)
            {
                if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.Summon))
                {
                    return ab;
                }
            }
            index++;
        }
        return null;
    }

    // ========================= GIVE ORDER ======================================================

    IEnumerator CastSpell(Vector3 location, GameObject target,int abilityIndex, float decisionTime)
    {
        LastSpellCastTime[abilityIndex] = Time.time;
        yield return new WaitForSeconds(ReactionTime + decisionTime);
        if (myManager.abilityList[abilityIndex].canActivate(false).canCast)
        {
            if (myManager.abilityList[abilityIndex] is TargetAbility)
            {
                myManager.UseTargetAbility(target, location, abilityIndex, false);
            }
            else
            {
                myManager.UseAbility(abilityIndex, false);
            }
        }
        // Need to handle for channel time? both in terms of not interrupting it (a bug elsewhere) or not making decisions til its done.
        currentActionItem = null;
    }

    IEnumerator AttackTarget(UnitManager target, float decisionTime)
    {
        yield return new WaitForSeconds(ReactionTime + decisionTime);

        if ( target)
        {
            myManager.GiveOrder(Orders.CreateAttackOrder(target.gameObject));
        }

        currentActionItem = null;
    }

    IEnumerator MoveToSpot(Vector3 location, float decisionTime)
    {
        yield return new WaitForSeconds(ReactionTime + decisionTime);
        myManager.GiveOrder(Orders.CreateMoveOrder(location));
        while(myManager.getState() is MoveState)
        {
            yield return null;
        }
        currentActionItem = null;
    }
}