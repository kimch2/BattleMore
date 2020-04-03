using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAI : MonoBehaviour
{
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
                if (myManager.myWeapon[0].inRange(currentTarget))
                {
                    Debug.Log("Attacking");
                    StopAllCoroutines();
                    currentActionItem = StartCoroutine(AttackTarget(currentTarget, ReactionTime));
                    myManager.GiveOrder(Orders.CreateAttackOrder(currentTarget.gameObject));
                }
                else
                {
                    
                    Vector3 Direction = (currentTarget.transform.position - transform.position).normalized * 4;
                    Vector3 ToCheck = transform.position + Direction;

                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        Debug.Log("Moving forward");
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(ToCheck, ReactionTime));
                        return;
                    }
                    ToCheck = transform.position + Quaternion.Euler(0, 45, 0) * Direction;
                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        Debug.Log("Moving 45");
                        StopAllCoroutines();
                        currentActionItem = StartCoroutine(MoveToSpot(ToCheck, ReactionTime));
                        return;
                    }
                    ToCheck = transform.position + Quaternion.Euler(0, -45, 0) * Direction;
                    if (AbilityHeatMap.main.IsSafe(ToCheck, this))
                    {
                        Debug.Log("Moving -45");
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
        InDanger = false;
    }

    bool InDanger;
    public void InDangerZone(List<AbilityHeatMap.DangerZone> MyZones)
    {
        if (!InDanger) { 
            InDanger = true;

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
            }
            
        }
    }

    public void ReactToLine(Vector3 OriginPoint, Vector3 EndPoint, float width, float TimeTilStrike)
    {

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

        Ability BlockAb = CanBlockOrEscape(ToRunTo, TimeTilStrike);
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

    Ability CanBlockOrEscape(Vector3 optimalMoveLocation, float TimeTilStrike)
    {
        foreach (Ability ab in myManager.abilityList)
        {
            if (ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab)))
            {
                if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.Dash) ||
                    ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.ReactiveDefense))
                {
                    return ab;
                }
            }
        }
        return null;
    }

    // ========================= GIVE ORDER

    IEnumerator CastSpell(Vector3 location, GameObject target,int abilityIndex, float decisionTime)
    {
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