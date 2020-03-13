using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTargetComboAbil : TargetAbility
{

    public GameObject ObjectToSpawn;
    public ComboTag.AbilType TagType;

    public List<ComboTag.AbilType> Combination;

    [Tooltip("This will only be used if the thing spawning is an explosion/projectile. But Projectiles shouldn't be spawned from this class (AreaTargetComboAbil), only from a inheritor class")]
    public float Damage;

    new void Start()
    {
        base.Start();
        myType = type.target;      
        InitializeCharges();
    }

    public override void Activate()
    {
   
    }

    public override continueOrder canActivate(bool error)
    {
        return DefaultCanActivate(false,true);
    }

    public override void Cast()
    {
        myCost.payCost();
        changeCharge(-1);

        if (soundEffect)
        {
            audioSrc.PlayOneShot(soundEffect);
        }
        Vector3 pos = location;

        GameObject proj = (GameObject)Instantiate(ObjectToSpawn, pos, Quaternion.identity);
        if (!SetOnHitContainer(proj,Damage, null))
        {
            proj.SendMessage("setSource", myManager.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

    public override bool Cast(GameObject target, Vector3 location)
    {
        changeCharge(-1);
        myCost.payCost();

        Vector3 pos = location;
        //pos.y += 5;
        GameObject proj = (GameObject)Instantiate(ObjectToSpawn, pos, Quaternion.identity);
        if (!SetOnHitContainer(proj,Damage, null))
        {
            proj.SendMessage("setSource", this.gameObject);
        }
        return false;
    }

    public override bool isValidTarget(GameObject target, Vector3 location)
    {
        return true;
    }

    public override void setAutoCast(bool offOn)
    {
        BaseSetAutoCast(offOn);
    }
}
