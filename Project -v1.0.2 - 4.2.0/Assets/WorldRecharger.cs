using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecharger : MonoBehaviour {


    public List<UnitStats> ToHeal;
    public List<UnitStats> ToEnergize; // Updates twice a second
    public List<UnitStats> ToSuperEnergize; // Updates 10 times a second

    public List<RoamAI> ToRoam;

    public List<List<Notify>> SpellCastNotify = new List<List<Notify>>();

    public static WorldRecharger main;

    void Awake()
    {
        main = this;
    }

    // Use this for initialization
    void Start() {
        InvokeRepeating("Charge", .5f, .5f);
        InvokeRepeating("Roam", 1, 3);
        InvokeRepeating("SuperCharge", .1f, .1f);

        for (int i = 0; i < GameManager.main.playerList.Length + 1; i++)
        {
            SpellCastNotify.Add(new List<Notify>());
        }
    }


    public void addHeal(UnitStats target)
    {
        ToHeal.Add(target);
    }

    public void removeHeal(UnitStats target)
    {
        ToHeal.Remove(target);
    }

    public void addEnergy(UnitStats target)
    {
        if (target.EnergyRegenPerSec >= 5)
        {
            ToSuperEnergize.Add(target);
        }
        else
        {
            ToEnergize.Add(target);
        }
    }

    public void removeEnergy(UnitStats target)
    {
        if (!ToEnergize.Remove(target))
        {
            ToSuperEnergize.Remove(target);
        }
    }

    public void addRoam(RoamAI target)
    {
        ToRoam.Add(target);
    }

    public void removeRoam(RoamAI target)
    {
        ToRoam.Remove(target);
    }

    void SuperCharge()
    {
        foreach (UnitStats stat in ToSuperEnergize)
        {
            if (stat)
            {
                stat.veternStat.UpEnergy(stat.changeEnergy(stat.EnergyRegenPerSec / 10));
            }
        }
    }

    void Charge()
    {

        foreach (UnitStats stat in ToHeal) {
            if (stat) {
                //Debug.Log("Healing " + stat.Maxhealth);
                stat.veternStat.UpHealing(stat.heal(stat.HealthRegenPerHalf, DamageTypes.DamageType.Regular));
            }
        }
        foreach (UnitStats stat in ToEnergize) {
            if (stat) {
                stat.veternStat.UpEnergy(stat.changeEnergy(stat.EnergyRegenPerHalf));
            }

        }
    }

    int indexMod; // Makes it so only 1/3 roam each update to give it random look

    void Roam()
    {
        for (int i = 0; i < ToRoam.Count; i++) {
            if (ToRoam[i] && i % 3 == indexMod) {
                ToRoam[i].setnewLocation();
            }
        }
        indexMod++;
        indexMod %= 3;
    }

    /// <summary>
    /// Active Player = 1, Main Enemy = 2, Nuetral =3, Other Enemies = 4+
    /// </summary>
    /// <param name="PlayerNumber"></param>
    /// <param name="toAdd"></param>
    public void AddSpellCastNotifier(int[] PlayerNumber, Notify toAdd)
    {
        for (int i = 0; i < PlayerNumber.Length; i++)
        {
            if (!SpellCastNotify[i].Contains(toAdd))
            {
                SpellCastNotify[i].Add(toAdd);
            }
        }
    }


    public void RemoveSpellCaster(Notify toRemove)
    {
        foreach (List<Notify> pair in SpellCastNotify)
        {
            pair.Remove(toRemove);
        }
    }

    public void SpellWasCast(int PlayerNumber, GameObject source) {
        foreach (Notify toCall in SpellCastNotify[PlayerNumber]) {
            if(toCall!=null)
            toCall.trigger(null,source,null,0);
        }
    }

}
