using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterLifeEffect :IEffect, Modifier {

    // Unit will become invulnerable for X seconds when it should have died.
    public float senserTime = 5;
    public float TimeInvulnerable = 10;


    public override void BeginEffect()
    {      
        OnTargetManager.myStats.addLethalTrigger(this);
        Invoke("EndEffect", senserTime);
    }

    public override void EndEffect()
    {
        RemoveVisualFX();
        OnTargetManager.myStats.otherTags.Remove(UnitTypes.UnitTypeTag.Invulnerable);
        OnTargetManager.myStats.SetTags();
        OnTargetManager.myStats.removeLethalTrigger(this);
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        OnTargetManager.myStats.otherTags.Add(UnitTypes.UnitTypeTag.Invulnerable);
        OnTargetManager.myStats.SetTags();
        OnTargetManager.myStats.removeLethalTrigger(this);
        OnTargetManager.myStats.attackPriority -= 3;
        Pathfinding.RVO.RVOController controller = OnTargetManager.GetComponent<Pathfinding.RVO.RVOController>();

        if (controller)
        {
            controller.radius = .3f;
            controller.layer = Pathfinding.RVO.RVOLayer.Layer4;
            controller.collidesWith = Pathfinding.RVO.RVOLayer.Layer4;            
        }

        StartCoroutine(EndInvulnerable());
        CancelInvoke("EndEffect");
        return 0;
    }

   

    IEnumerator EndInvulnerable()
    {

        yield return new WaitForSeconds(TimeInvulnerable);       
        OnTargetManager.myStats.otherTags.Remove(UnitTypes.UnitTypeTag.Invulnerable);
        OnTargetManager.myStats.SetTags();
        OnTargetManager.myStats.kill(this.gameObject);
    }

    public override bool validTarget(GameObject target)
    {
        return true;
    }

    public override void applyTo(GameObject source, UnitManager target)
    {
        AfterLifeEffect after = (AfterLifeEffect)CopyIEffect(target, true);
    }

}
