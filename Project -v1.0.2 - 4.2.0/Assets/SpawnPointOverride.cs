using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointOverride : MonoBehaviour
{
    int PlayerOwner;
    public OnHitContainer myHitContainer;
    public UnityEngine.Events.UnityEvent OnSpawn;
    public UnityEngine.Events.UnityEvent OnDespawn;
    public float Duration;
    float EndSpawnTime;

    public void setSource(GameObject source)
    {
        UnitManager man = source.GetComponent<UnitManager>();
        PlayerOwner = man.PlayerOwner;
        myHitContainer.Initialize(man);
        EndSpawnTime = Time.time + Duration;
    }

    private void Start()
    {
        DaminionsInitializer.main.SetSpawnLocation(this,PlayerOwner);
    }

    public void ResetTimer()
    {
        EndSpawnTime = Time.time + Duration;
    }

    public void ModifyUnit(UnitManager unit)
    {
        OnSpawn.Invoke();
        myHitContainer.trigger(null, unit, 0);
    }

    public bool CheckIfOnScreen()
    {
        Vector3 screenPoint = CarbotCamera.singleton.myCamera.WorldToViewportPoint(transform.position);

        if (screenPoint.z < 0 ||  screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            OnDespawn.Invoke();
            this.enabled = false;
            DaminionsInitializer.main.SetSpawnLocation(null, PlayerOwner);
            return false;
        }
        return true;
    }


    private void Update()
    {
        if (Time.time > EndSpawnTime)
        {
            OnDespawn.Invoke();
            this.enabled = false;
            DaminionsInitializer.main.SetSpawnLocation(null, PlayerOwner);
        }
    }
}
