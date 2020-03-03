using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEffectsManager : MonoBehaviour
{
    public static GenericEffectsManager main;

    public GameObject Stun;
    public GameObject Criticalhit;
    public GameObject Crater;
    public GameObject Dust;
    public GameObject Healing;
    public GameObject SlowMovement;
    public GameObject ArmorBlock;

    // Start is called before the first frame update
    void Start()
    {
        main = this;    
    }


    public static GameObject StunEffect()
    {
        return Instantiate<GameObject>(main.Stun);
    }

    public static GameObject CriticalHitEffect()
    {
        return Instantiate<GameObject>(main.Criticalhit);
    }

    public static GameObject CraterEffect()
    {
        return Instantiate<GameObject>(main.Crater);
    }

    public static GameObject DustEffect()
    {
        return Instantiate<GameObject>(main.Dust);
    }

    public static GameObject HealingEffect()
    {
        return Instantiate<GameObject>(main.Healing);
    }

    public static GameObject SlowMovementEffect()
    {
        return Instantiate<GameObject>(main.SlowMovement);
    }

    public static GameObject ArmorBlockEffect()
    {
        return Instantiate<GameObject>(main.ArmorBlock);
    }
}
