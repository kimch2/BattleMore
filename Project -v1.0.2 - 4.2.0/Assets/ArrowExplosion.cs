using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowExplosion : explosion
{
    protected override void FindTargets()
    {
        float TempDamageAmount = 0;
        float BestDistance = Mathf.Pow(sizeSquared + 4, 2);
        UnitManager bestTarget = null;

        foreach (RaceManager manager in GameManager.main.playerList)
        {
            if (manager.playerNumber == MyHitContainer.playerNumber)
            {
                if (!AppliesToAlly)
                {
                    continue;
                }
                else
                {
                    TempDamageAmount = damageAmount * MyHitContainer.FriendlyFireRatio;
                }
            }
            else if (manager.playerNumber != MyHitContainer.playerNumber && !AppliesToEnemy)
            {

                continue;
            }
            else
            {
                TempDamageAmount = damageAmount;
            }

            List<UnitManager> toDamage = new List<UnitManager>();


            foreach (KeyValuePair<string, List<UnitManager>> unitList in manager.getUnitList())
            {
                foreach (UnitManager unit in unitList.Value)
                {
                    if (unit == null)
                    {
                        continue;
                    }
                    float currDistance = getDistance(unit);
                    //Debug.Log("Setting current " + currDistance + "     " + BestDistance);
                    if (currDistance <= BestDistance)
                    {
                        BestDistance = currDistance;
                        bestTarget = unit;
                    }
                }
            }
            if (bestTarget)
            {
                DealDamage(bestTarget, TempDamageAmount);
            }
        }
    }
}
