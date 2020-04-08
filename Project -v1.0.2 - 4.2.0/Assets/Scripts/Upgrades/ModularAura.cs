using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularAura : IEffect
{
    // We store a list of enums that show which stats are being changed, 
    //so we don't have to apply a stat change of "0" on stats that aren't used 



    public List<AuraNumber> myBuffs = new List<AuraNumber>();

    public bool Stacks;

    [Tooltip("If this is 0, it is permanant")]
    public float Duration = 0;

    [Tooltip ("This only applies if there is a duration")]
    public bool Decays;


    public override void BeginEffect()
    {
        ApplyBuff(OnTargetManager, 1);
        if (Duration > 0)
        {
            StartCoroutine(DelayedRemove(OnTargetManager));
        }
    }

    public override void EndEffect()
    {
        RemoveBuff(OnTargetManager);
        base.EndEffect();
    }


    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        bool AlreadyOnIt;
        ModularAura aura = (ModularAura)CopyIEffect(target, true, out AlreadyOnIt);
        if (AlreadyOnIt && Stacks)
        {
            aura.ApplyBuff(aura.OnTargetManager, 1);
        }

    }

    IEnumerator DelayedRemove(UnitManager target)
    {
        if (Decays)
        {
            for (float i = Duration; i > 0; i -= .5f)
            {
                yield return new WaitForSeconds(.5f);
                if (target)
                {
                    ApplyBuff(target, i / Duration);
                }
                else
                {
                    break;
                }
            }
            if (target)
            {
                RemoveBuff(target);
            }
        }
        else
        {
            yield return new WaitForSeconds(Duration);

            if (target)
            {
                RemoveBuff(target);
            }
        }
    }


    public void ApplyBuff(UnitManager manager, float percentage) // percenetage being used when an effect decays over time
    {

        bool friendly =true;
        if (SourceManager && SourceManager.PlayerOwner != manager.PlayerOwner)
        {
            friendly = false;
        }
        foreach (AuraNumber buff in myBuffs)
        {
            float flat = buff.Flat * percentage;
            float perc = buff.Percent * percentage;
            switch (buff.buffType)
            {
                case StatChanger.BuffType.Armor:
                    manager.myStats.statChanger.changeArmor(perc,flat, this, friendly,  Stacks);
                    break;

                case StatChanger.BuffType.AttackSpeed:
                    manager.myStats.statChanger.changeAttackSpeed(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.Damage:
                    Debug.Log("Adding damage " + flat);
                    manager.myStats.statChanger.changeWeaponDamage(perc, flat, this, friendly, Stacks);
                    Debug.Log("After damage " + manager.myWeapon[0].baseDamage + "  " + manager.myWeapon[0].Title);
                    break;

                case StatChanger.BuffType.Energy:
                    manager.myStats.statChanger.changeEnergyMax(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.EnergyRegen:
                    manager.myStats.statChanger.changeEnergyRegen(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.HP:
                    manager.myStats.statChanger.changeHealthMax(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.HPRegen:
                    manager.myStats.statChanger.changeHealthRegen(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.MoveSpeed:
                    manager.myStats.statChanger.changeMoveSpeed(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.Cooldown:
                    manager.myStats.statChanger.changeCooldown(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.Range:
                    manager.myStats.statChanger.changeWeaponRange(perc, flat, this, friendly, Stacks);
                    break;

                case StatChanger.BuffType.Priority: // TAUNT - Should this go through the Meta-status class???
                    manager.myStats.statChanger.changeAttackPriority(perc, flat, this, friendly, Stacks);
                    break;
            }
        }
    }

    public void RemoveBuff(UnitManager manager)
    {
       // Debug.Log("Removeing buff");
        foreach (AuraNumber buff in myBuffs)
        {
            switch (buff.buffType)
            {
                case StatChanger.BuffType.Armor:
                    manager.myStats.statChanger.removeArmor(this);
                    break;

                case StatChanger.BuffType.AttackSpeed:
                    manager.myStats.statChanger.removeAttackSpeed(this);
                    break;

                case StatChanger.BuffType.Damage:
                    Debug.Log("removing damage");
                    manager.myStats.statChanger.removeWeaponDamage(this);
                    break;

                case StatChanger.BuffType.Energy:
                    manager.myStats.statChanger.removeEnergyMax(this);
                    break;

                case StatChanger.BuffType.EnergyRegen:
                    manager.myStats.statChanger.removeEnergyRegen(this);
                    break;

                case StatChanger.BuffType.HP:
                    manager.myStats.statChanger.removeHealthMax(this);
                    break;

                case StatChanger.BuffType.HPRegen:
                    manager.myStats.statChanger.removeHealthRegen(this);
                    break;

                case StatChanger.BuffType.MoveSpeed:
                    manager.myStats.statChanger.removeMoveSpeed(this);
                    break;

                case StatChanger.BuffType.Cooldown:
                    manager.myStats.statChanger.removeCooldown(this);
                    break;

                case StatChanger.BuffType.Range:
                    manager.myStats.statChanger.removeWeaponRange(this);
                    break;

                case StatChanger.BuffType.Priority:
                    manager.myStats.statChanger.removeAttackPriority(this);
                    break;

            }
        }
    }
}

[System.Serializable]
public class AuraNumber
{
    public StatChanger.BuffType buffType;
    public float Percent;
    public float Flat;
}