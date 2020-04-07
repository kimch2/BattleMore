using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBarrier : IEffect, Modifier
{

    // Unit will become invulnerable for X seconds, for each Y damage it recieves it spawns a healing bubble
    float TimeInvulnerable = 3;
    public float DamagePerBubble = 100;
    float DamageRecieved;
    public GameObject BubblePrefab;


    public override void BeginEffect()
    {
        OnTargetManager.myStats.addModifier(this, 0);
        Invoke("EndEffect", TimeInvulnerable);
    }

    public override void EndEffect()
    {
        OnTargetManager.myStats.removeModifier(this);
        base.EndEffect();
    }

    public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
    {
        DamageRecieved += damage;
        if (DamageRecieved > DamagePerBubble)
        {
            DamageRecieved -= DamagePerBubble;
            GameObject obj = Instantiate<GameObject>(BubblePrefab, transform.position, Quaternion.identity);
            // Kinda Dangerous to initialize the object from an unrelated HitContainer, but we need to tell
            // it who it belongs to. THey might not even have a weapon.
            SourceManager.myWeapon[0].myHitContainer.SetOnHitContainer(obj, 0, null);
        }

        return 0;
    }



    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        BubbleBarrier after = (BubbleBarrier)CopyIEffect(target, true, out bool alreadyOnIt);
    }

}