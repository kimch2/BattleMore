using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSkulliosis : MonoBehaviour
{
    int RegistryName;
    [Tooltip ("Gives a skull every X seconds, if X isn't 0")]
    public float PassiveSkullCollect;

    private void Start()
    {
        UnitManager manage = GetComponent<UnitManager>();
        if (!manage)
        {
            manage = GetComponentInParent<UnitManager>();
        }
        if (!manage && GetComponent<OnHitContainer>())
        {
            manage = GetComponent<OnHitContainer>().myManager;
        }
        if (!manage)
        {
            manage = GetComponentInParent<OnHitContainer>().myManager;
        }
        RegistryName = manage.PlayerOwner;

        if (PassiveSkullCollect > 0)
        {
            StartCoroutine(passiveSkulls());
        }
    }

    IEnumerator passiveSkulls()
    {
        while (this)
        {
            yield return new WaitForSeconds(PassiveSkullCollect);
            AddSkullRemote();
        }
    }

    //THis class is used to add a skull to the SKulliosis from another Ability
    public void AddSkullRemote()
    {
        if (Skulliosis.HeroSingleton != null)
        {
            Skulliosis.HeroSingleton[RegistryName].GainSkull(gameObject);
        }
    }
    public void AddSkullRemote(GameObject source)
    {
        if (Skulliosis.HeroSingleton != null)
        {
            Skulliosis.HeroSingleton[RegistryName].GainSkull(source);
        }
    }
}

