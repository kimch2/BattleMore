using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAssessor
{

    public static Ability CanBlock(UnitManager myManager)
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

    public static Ability GetDamageAbility(UnitManager myManager, float[] LastSpellCastTime)
    {
        int index = 0;
        foreach (Ability ab in myManager.abilityList)
        {
            if ((ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab))) && LastSpellCastTime[index] + 6 < Time.time)
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

    public static Ability GetSummonAbility(UnitManager myManager, float[] LastSpellCastTime)
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

    public static Ability GetDamageEnhancer(UnitManager myManager, float[] LastSpellCastTime)
    {
        int index = 0;
        foreach (Ability ab in myManager.abilityList)
        {
            if ((ab.chargeCount > 0 || (ab.myCost && ab.myCost.canActivate(ab))) && LastSpellCastTime[index] + 6 < Time.time)
            {
                if (ab.metaData.AbilityUsage.Contains(AbilityMetaData.MetaAbilityType.DamageEnhancer))
                {
                    return ab;
                }
            }
            index++;
        }
        return null;
    }


    public static UnitManager EvaluateAutoAttack(UnitManager myManager, UnitManager Best, float TargetAquisitionSkill)
    {
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
}
