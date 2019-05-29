using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{

	public static ResourceLoader main;
	public static ResourceLoader getMain()
	{
        //InstructionHelperManager.getInstance().addBUtton("Getting Main A", 25, null);
        if (main == null)
		{
			GameObject newObject = new GameObject("ResourceLoader");
           // InstructionHelperManager.getInstance().addBUtton("Getting Main B", 25, null);
            main = newObject.AddComponent<ResourceLoader>();

        }
       // InstructionHelperManager.getInstance().addBUtton("Getting Main C", 25, null);
        return main;
	}

	public Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();

	public GameObject getResource(string s)
    {
        //InstructionHelperManager.getInstance().addBUtton("Finding A " + s, 25, null);
        if (!cachedObjects.ContainsKey(s))
		{
           // InstructionHelperManager.getInstance().addBUtton("Finding B" + s, 25, null);
            cachedObjects.Add(s, Resources.Load<GameObject>(s));
           // InstructionHelperManager.getInstance().addBUtton("Finding C" + s, 25, null);
        }
		return cachedObjects[s];
	}

}

[System.Serializable]
public class floatGameObject
{
	public float number;
	public GameObject obj;

	public floatGameObject(float n, GameObject o)
	{
		number = n;
		obj = o;
	}
}


