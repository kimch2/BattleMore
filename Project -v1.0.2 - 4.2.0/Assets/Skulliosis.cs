using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skulliosis : TriggeredAbility
{
    public int CurrentSkullCount;
    public GameObject SkullOrbiter;
    public GameObject SKullPrefab;
    List<GameObject> currentSkulls = new List<GameObject>();

    public static Dictionary<int, Skulliosis> HeroSingleton = new Dictionary<int, Skulliosis>();

    public override void Start()
    {
        base.Start();
        HeroSingleton.Add(myManager.PlayerOwner, this);
        foreach (Ability ab in myManager.abilityList)
        {
            if (ab.myHitContainer)
            {
                RemoteSkulliosis remote = ab.myHitContainer.gameObject.AddComponent<RemoteSkulliosis>();
                ab.myHitContainer.onKillWithTarget.AddListener(remote.AddSkullRemote);
            }
        }
    }

    public void GainRegSkull()
    {
        GameObject newSkull = Instantiate<GameObject>(SKullPrefab);
        collectSkull(newSkull);
    }

        public void GainSkull(GameObject source)
    {
        StartCoroutine(FlyingSkull(source.transform.position));
    }

    IEnumerator FlyingSkull(Vector3 Origin)
    {
        GameObject newSkull = Instantiate<GameObject>(SKullPrefab);
        newSkull.transform.localScale = Vector3.one * 1.3f;
        newSkull.transform.position = Origin;
        newSkull.transform.eulerAngles = Vector3.right * 90;

        for (float i = 0; i < 1f; i += Time.deltaTime)
        {
            yield return null;
            newSkull.transform.position = Vector3.Lerp(Origin, transform.position, i * i);
        }
        collectSkull(newSkull);
    }

    void collectSkull(GameObject newSkull)
    {
        CurrentSkullCount++;
        if (CurrentSkullCount >= 10)
        {
            DMSpawnUnit spawner = GetComponent<DMSpawnUnit>();
            spawner.Activate();
            CurrentSkullCount = 0;
            StartCoroutine(Dissipate(currentSkulls));
            currentSkulls = new List<GameObject>();
            Destroy(newSkull);
        }
        else
        {
            newSkull.transform.parent = SkullOrbiter.transform;
            currentSkulls.Add(newSkull);
            int skullCount = currentSkulls.Count - 1;

            currentSkulls[skullCount].transform.localPosition = Quaternion.Euler(0, 0, 36 * skullCount) * Vector3.up * 5;
            currentSkulls[skullCount].transform.localRotation = Quaternion.Euler(0, 0, 36 * skullCount);
            currentSkulls[skullCount].transform.localScale = Vector3.one;
        }
    }

    IEnumerator Dissipate(List<GameObject> toFix)
    {

        for (float i = 0; i < .35f; i+=  Time.deltaTime)
        {
            yield return null;
            foreach (GameObject thingy in toFix)
            {
                thingy.transform.localScale += Vector3.one * Time.deltaTime*4;
            }           
        }
        for (float i = 0; i < .5f; i += Time.deltaTime)
        {
            yield return null;
            foreach (GameObject thingy in toFix)
            {
                thingy.transform.localScale -= Vector3.one * Time.deltaTime * 5f;
            }
        }
        foreach (GameObject thingy in toFix)
        {
            Destroy(thingy);
        }
    }

}
