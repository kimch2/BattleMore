using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularAura : MonoBehaviour
{

    public List<StatChanger.BuffType> myBuffs;
    public List<ModularAura.ICustomAura> customAuras = new List<ModularAura.ICustomAura>();
    public bool Stacks;

    public float Armor;

    public float HealthFlat;
    public float HealthPerc;

    public float HealthRegenFlat;
    public float HealthRegenPerc;

    public float MoveSpeedFlat;
    public float MoveSpeedPerc;

    public float DamageFlat;
    public float DamagePerc;

    [Tooltip("This changes the attack period, -.2 will decrease period and increase attack speed")]
    public float AttackSpeedFlat;
    public float AttackSpeedPerc;

    public float EnergyFlat;
    public float EnergyPerc;

    public float EnergyRegenFlat;
    public float EnergyRegenPerc;

    public float CooldownPerc;

    public float RangeFlat;
    public float RangePerc;


    UnitManager myManager;
    private void Start()
    {
        myManager = GetComponent<UnitManager>();
    }

    public void ApplyBuff(UnitManager manager)
    {
        foreach (StatChanger.BuffType buff in myBuffs)
        {
            switch (buff)
            {
                case StatChanger.BuffType.Armor:
                    manager.myStats.statChanger.changeArmor(0,Armor, this,Stacks);
                    break;

                case StatChanger.BuffType.AttackSpeed:
                    manager.myStats.statChanger.changeAttackSpeed(AttackSpeedPerc, AttackSpeedFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.Damage:
                    manager.myStats.statChanger.changeWeaponDamage(DamagePerc,DamageFlat,this,Stacks);
                    break;

                case StatChanger.BuffType.Energy:
                    manager.myStats.statChanger.changeEnergyMax(EnergyPerc, EnergyFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.EnergyRegen:
                    manager.myStats.statChanger.changeEnergyRegen(EnergyRegenPerc, EnergyRegenFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.HP:
                    manager.myStats.statChanger.changeHealthMax(HealthPerc, HealthFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.HPRegen:
                    manager.myStats.statChanger.changeHealthRegen(HealthRegenPerc, HealthRegenFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.MoveSpeed:
                    manager.myStats.statChanger.changeMoveSpeed(MoveSpeedPerc,MoveSpeedFlat, this, Stacks);
                    break;

                case StatChanger.BuffType.Cooldown:
                    manager.myStats.statChanger.changeCooldown(CooldownPerc,0, this, Stacks);
                    break;

                case StatChanger.BuffType.Range:
                    manager.myStats.statChanger.changeWeaponRange(RangePerc, RangeFlat, this, Stacks);
                    break;

            }
        }

        foreach (ICustomAura aura in customAuras)
        {
            if (aura != null)
            {
                aura.Apply(myManager);
            }
        }
    }

    public void RemoveBuff(UnitManager manager)
    {
        foreach (StatChanger.BuffType buff in myBuffs)
        {
            switch (buff)
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

            }
        }

        foreach (ICustomAura aura in customAuras)
        {
            if (aura != null)
            {
                aura.Remove(myManager);
            }
        }
    }


    public interface ICustomAura
    {
        void Apply(UnitManager manage);
        void Remove(UnitManager manage);
    }
}
