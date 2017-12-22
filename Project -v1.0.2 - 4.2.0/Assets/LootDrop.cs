using UnityEngine;
using System.Collections;

public class LootDrop : MonoBehaviour,Modifier {

	public GameObject Loot;
	public Vector3 PositionOffset;
	public bool placeOnGround;
	void Start()
	{
		GetComponent<UnitStats> ().addDeathTrigger (this);

	}

	public float modify (float a, GameObject deathSource, DamageTypes.DamageType theType){

		if (Loot) {
			
			GameObject obj =  (GameObject)Instantiate (Loot, this.gameObject.transform.position + PositionOffset, Quaternion.identity);
			if (placeOnGround) {

				RaycastHit objecthit;
				Vector3 down = this.gameObject.transform.TransformDirection (Vector3.down);

				if (Physics.Raycast (this.gameObject.transform.position, down, out objecthit, 1000,  1 << 8)) {

					obj.transform.position = objecthit.point + PositionOffset;
				}
			}
		}
		return a;

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawCube (transform.position + PositionOffset, Vector3.one);
	}

}
