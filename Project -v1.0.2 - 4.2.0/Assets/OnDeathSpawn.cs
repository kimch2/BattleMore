using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathSpawn : MonoBehaviour
{
    public int numToSpawn = 1;
    public GameObject toSpawnObject;
    [Tooltip("only use this if the thing that is spawning is the thign that this is on, which causing reference errors, because it will refer to itself and not the prefab")]
    public string toSpawn;
    

    // NEED TO CODE THIS MORE PROPERLY SO IT DOES ALL TEAM SETTING DATA
    public void Dying()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            if (toSpawnObject)
            {
                Instantiate(toSpawnObject, this.transform.position + new Vector3(0,0, (i * 2) - (numToSpawn/2)), Quaternion.identity);
            }
            else
            {
                Instantiate(Resources.Load<GameObject>(toSpawn), this.transform.position + new Vector3(0, 0, (i * 2) - (numToSpawn / 2)), Quaternion.identity);
            }
        }
        
    }
}
