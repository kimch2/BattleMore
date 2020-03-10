using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAuraApplier : MonoBehaviour
{
    private GameObject source;

    public bool affectsEnemies = true;
    public bool affectsAllies;
    public float duration = .1f;

    private int playerNumber;
    public ModularAura AuraToApply;
    public List<IEffect> effectsToApply;

    // TODO still have to make it so it doesn't stack and possibility of removing the aura if it leaves the area


    // Use this for initialization
    void Start()
    {
        Invoke("selfDestruct", duration);
    }

    void  selfDestruct()
    {
        Destroy(this.gameObject);
    }

    public void setSource(GameObject obj)
    {
        source = obj;
        playerNumber = source.GetComponent<UnitManager>().PlayerOwner;

    }


    void OnTriggerEnter(Collider other)
    {
        UnitManager manage = other.gameObject.GetComponent<UnitManager>();
        if (manage)
        {
            if ((manage.PlayerOwner != playerNumber && affectsEnemies) || (manage.PlayerOwner == playerNumber && affectsAllies))
            {
                if (AuraToApply)
                {
                    GameObject newAura = Instantiate<GameObject>(AuraToApply.gameObject, other.transform);
                    newAura.transform.localPosition = Vector3.zero;
                    newAura.GetComponent<ModularAura>().ApplyBuff(manage);
                }
                foreach (IEffect fect in effectsToApply)
                {
                    fect.applyTo(source, manage);
                }


                // Add in code that sets the source
            }
        }
    }
}
