using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ModularAura))]
public class ModularAuraEditor : Editor
{
    StatChanger.BuffType toAdd = StatChanger.BuffType.AttackSpeed;
    public override void OnInspectorGUI()
    {


        ModularAura aura = (ModularAura)target;
        foreach(StatChanger.BuffType buff in aura.myBuffs)
        {
            switch (buff)
            {
                case StatChanger.BuffType.Armor:
                    aura.Armor = EditorGUILayout.FloatField("Armor", aura.Armor);
                    break;

                case StatChanger.BuffType.AttackSpeed:
                    aura.AttackSpeedFlat = EditorGUILayout.FloatField("Flat Attack Speed",aura.AttackSpeedFlat);
                    aura.AttackSpeedPerc = EditorGUILayout.FloatField("Percent Attack Speed", aura.AttackSpeedPerc);
                    break;

                case StatChanger.BuffType.Damage:
                    aura.DamageFlat = EditorGUILayout.FloatField("Flat Damage", aura.DamageFlat);
                    aura.DamagePerc = EditorGUILayout.FloatField("Percent Damage", aura.DamagePerc);
                    break;

                case StatChanger.BuffType.Energy:
                    aura.EnergyFlat = EditorGUILayout.FloatField("Flat Max Energy", aura.EnergyFlat);
                    aura.EnergyPerc = EditorGUILayout.FloatField("Percent Max Energy",aura.EnergyPerc);
                    break;

                case StatChanger.BuffType.EnergyRegen:
                    aura.EnergyRegenFlat = EditorGUILayout.FloatField("Flat Energy Regen", aura.EnergyRegenFlat);
                    aura.EnergyRegenPerc = EditorGUILayout.FloatField("Percent Energy Regen", aura.EnergyRegenPerc);
                    break;

                case StatChanger.BuffType.HP:
                    aura.HealthFlat = EditorGUILayout.FloatField("Max HP Flat",aura.HealthFlat);
                    aura.HealthPerc = EditorGUILayout.FloatField("Max HP Percent",aura.HealthPerc);
                    break;

                case StatChanger.BuffType.HPRegen:
                    aura.HealthRegenFlat = EditorGUILayout.FloatField("Health Regen Flat",aura.HealthRegenFlat);
                    aura.HealthRegenPerc = EditorGUILayout.FloatField("Health Regen Percent",aura.HealthRegenPerc);
                    break;

                case StatChanger.BuffType.MoveSpeed:
                    aura.MoveSpeedFlat = EditorGUILayout.FloatField("Move Speed Flat",aura.MoveSpeedFlat);
                    aura.MoveSpeedPerc = EditorGUILayout.FloatField("Move Speed Percent",aura.MoveSpeedPerc);
                    break;

                case StatChanger.BuffType.Cooldown:
                    aura.CooldownPerc = EditorGUILayout.FloatField("Cooldown Percent", aura.CooldownPerc);
                    break;

                case StatChanger.BuffType.Range:
                    aura.RangeFlat = EditorGUILayout.FloatField("Flat Weapon Range", aura.RangeFlat);
                    aura.RangePerc = EditorGUILayout.FloatField("Percent Weapon Range", aura.RangePerc);
                    break;
            }
            if (GUILayout.Button("Remove"))
            {
                aura.myBuffs.Remove(buff);
            }
            GUILayout.Space(25);
        }

        GUILayout.BeginHorizontal();

        toAdd = (StatChanger.BuffType)EditorGUILayout.EnumPopup("To Add", toAdd);
        if (GUILayout.Button("Add New One"))
        {
            aura.myBuffs.Add(toAdd);
        }

        GUILayout.EndHorizontal();
    }
}

