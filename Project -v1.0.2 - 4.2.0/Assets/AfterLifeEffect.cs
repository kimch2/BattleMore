using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterLifeEffect :IEffect, Modifier {

    // Unit will become invulnerable for X seconds when it should have died.
    public float senserTime = 5;
    public float TimeInvulnerable = 10;
    public GameObject VisualEffect;


    public void InitializeOnTarget()
    {      
        OnTargetManager.myStats.addLethalTrigger(this);
        Invoke("endEffect", senserTime);
        VisualEffect = Instantiate<GameObject>(VisualEffect, this.transform);
        VisualEffect.transform.localPosition = Vector3.down;
      
    }

    public void endEffect()
    {
        OnTargetManager.myStats.otherTags.Remove(UnitTypes.UnitTypeTag.Invulnerable);
        OnTargetManager.myStats.SetTags();
        OnTargetManager.myStats.removeLethalTrigger(this);
        Destroy(VisualEffect);
    }

    public float modify(float damage, GameObject source, DamageTypes.DamageType theType)
    {
        Debug.Log("MOdifying");
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
        CancelInvoke("endEffect");
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
        AfterLifeEffect after = (AfterLifeEffect)CopyIEffect(target);
        after.InitializeOnTarget();
    }

    public override void RemoveEffect(UnitManager target)
    {
        AfterLifeEffect after = target.GetComponent<AfterLifeEffect>();
        if (after)
        {
            after.endEffect();
        }
       
    }
}
