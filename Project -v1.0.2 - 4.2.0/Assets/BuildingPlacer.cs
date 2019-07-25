using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingPlacer : MonoBehaviour {


	public List<GameObject> objects = new List<GameObject>();
	public GameObject building;
	public Material good;
	public Material bad;
    int AbilityNumber;
	private SphereCollider coll;
	// Use this for initialization
	void Awake () {
		coll = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (building) {
			
			objects.RemoveAll (item => item == null);
			if (objects.Count == 0) {

				if (!AstarPath.active.graphs [0].GetNearest (transform.position).node.Walkable) {
					setRenderers (bad);
					return;
				}
                if (!SelectedManager.main.checkValidTarget(transform.position, UIManager.main.getGroundCast(transform.position).collider.gameObject, AbilityNumber))
                {
                    setRenderers(bad);
                    return;
                }

				Tile t = Grid.main.GetClosestRedTile (this.gameObject.transform.position);
				if (FogOfWar.current.IsInCompleteFog (this.gameObject.transform.position)) {
					setRenderers (bad);
				} else {
					if (!t.Buildable) {
						float dist = Mathf.Pow (t.Center.x - transform.position.x, 2) + Mathf.Pow (t.Center.z - transform.position.z, 2);
						if (Mathf.Sqrt (dist) < coll.radius) {
							setRenderers (bad);
						} else {
							setRenderers (good);
						}
					} else {
						setRenderers (good);
					}
				}
			}
		}
	}

	public bool canBuild(GameObject target = null)
	{  		objects.RemoveAll (item => item == null);


		if (objects.Count != 0) {
            return false;
		}

      if( target && ! SelectedManager.main.checkValidTarget(transform.position,target, AbilityNumber))
        {
            Debug.Log("In Here");
            setRenderers(bad);
            return false; 
        }

			Tile t = Grid.main.GetClosestRedTile (this.gameObject.transform.position);
			if (FogOfWar.current.IsInCompleteFog (this.gameObject.transform.position)) {
				setRenderers (bad);

			return false;
			} else {
				if (!t.Buildable) {
					float dist = Mathf.Pow (t.Center.x - transform.position.x, 2) + Mathf.Pow (t.Center.z - transform.position.z, 2);
					if (Mathf.Sqrt (dist) < coll.radius) {
						setRenderers (bad);
					Debug.Log ("not buildable" );
					return false;
					} else {
						setRenderers (good);
					}
				} else {
					setRenderers (good);
				}

		}

		return true;
	}

	public void reset(GameObject b, Material g, Material ba, int abilityNum)
	{

		FogOfWarUnit fow = b.GetComponent<FogOfWarUnit> ();
		if (fow) {
			fow.enabled = false;
		}
		GetComponent<SphereCollider> ().enabled = true;
		GetComponent<SphereCollider> ().radius = 10;
		objects.Clear ();
		building = b;
		good = g;
		bad = ba;
		setRenderers (good);
        AbilityNumber = abilityNum;
		Animator anim = b.GetComponentInChildren<Animator> ();
		if (anim) {
			anim.SetInteger ("State", 1);
		}

		foreach (CharacterController c in b.GetComponents<CharacterController>()) {
			c.enabled = false;
		}
		foreach (SphereCollider co in b.GetComponents<SphereCollider>()) {
			co.enabled = false;
		}
		foreach (SphereCollider co in b.GetComponentsInChildren<SphereCollider>()) {
			co.enabled = false;
		}

		foreach (BoxCollider co in b.GetComponents<BoxCollider>()) {
			co.enabled = false;
		}
		foreach (BoxCollider co in b.GetComponentsInChildren<BoxCollider>()) {
			co.enabled = false;
		}


		foreach (MeshFilter mf in building.GetComponentsInChildren<MeshFilter>()) {
			if (mf.mesh.name.Contains("Plane")) {
				Destroy (mf.gameObject);
			}
		}

		foreach (SpriteRenderer mf in building.GetComponentsInChildren<SpriteRenderer>()) {
			mf.enabled = false;
		}

		coll = GetComponent<SphereCollider> ();
		Update ();
	}

	void OnTriggerExit(Collider other)
	{
		if (objects.Contains (other.gameObject)) {
			objects.Remove (other.gameObject);

			if (objects.Count == 0) {
				setRenderers (good);
			}
		}

	}

	void OnTriggerEnter(Collider other)
	{	
        if (other.transform.root == building.transform)
        {
            return;
        }

        UnitManager manage = other.gameObject.GetComponent<UnitManager>();
      
		if (manage || other.gameObject.layer == 17) {
            objects.Add (other.gameObject);

				setRenderers (bad);

		} else {
			objects.Add (other.gameObject);

			setRenderers (bad);
		}
	}


	public void setRenderers(Material m)
	{

		if (building) {
			foreach (MeshRenderer mr in building.GetComponentsInChildren<MeshRenderer>()) {
				
				mr.material = m;
			}
		}
	
	}
}
