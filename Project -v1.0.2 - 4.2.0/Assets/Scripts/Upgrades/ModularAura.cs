using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularAura : IEffect
{
    // We store a list of enums that show which stats are being changed, 
    //so we don't have to apply a stat change of "0" on stats that aren't used 



    public List<AuraNumber> myBuffs = new List<AuraNumber>();
    [Tooltip("Any script in this list should extend the ICustomAura interface ")]
   
    public bool Stacks;
    [Tooltip("If this is 0, it is permanant")]
    public float Duration = 0;
    [Tooltip("This will immediately give it to the guy this is attached to, instead of having to be applied via spell")]
    public bool ApplyOnStart;

    private void Start()
    {
        if (ApplyOnStart)
        {
            applyTo(SourceManager.gameObject, SourceManager);
        }
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    { 
        ApplyBuff(target);
        if (Duration > 0)
        {
            StartCoroutine(DelayedRemove(target));
        }
    }

    IEnumerator DelayedRemove(UnitManager target)
    {
        yield return new WaitForSeconds(Duration);
        if (target)
        {
            RemoveBuff(target);
        }
    }

    public override void RemoveEffect(UnitManager target)
    {
        RemoveBuff(target);      
    }

    public void ApplyBuff(UnitManager manager)
    {
        foreach (AuraNumber buff in myBuffs)
        {
            switch (buff.buffType)
            {
                case StatChanger.BuffType.Armor:
                    manager.myStats.statChanger.changeArmor(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner,  Stacks);
                    break;

                case StatChanger.BuffType.AttackSpeed:
                    manager.myStats.statChanger.changeAttackSpeed(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.Damage:
                    manager.myStats.statChanger.changeWeaponDamage(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.Energy:
                    manager.myStats.statChanger.changeEnergyMax(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.EnergyRegen:
                    manager.myStats.statChanger.changeEnergyRegen(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.HP:
                    manager.myStats.statChanger.changeHealthMax(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.HPRegen:
                    manager.myStats.statChanger.changeHealthRegen(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.MoveSpeed:
                    manager.myStats.statChanger.changeMoveSpeed(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.Cooldown:
                    manager.myStats.statChanger.changeCooldown(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.Range:
                    manager.myStats.statChanger.changeWeaponRange(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;

                case StatChanger.BuffType.Priority: // TAUNT - Should this go through the Meta-status class???
                    manager.myStats.statChanger.changeAttackPriority(buff.Percent, buff.Flat, this, SourceManager.PlayerOwner == manager.PlayerOwner, Stacks);
                    break;
            }
        }
    }

    public void RemoveBuff(UnitManager manager)
    {
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