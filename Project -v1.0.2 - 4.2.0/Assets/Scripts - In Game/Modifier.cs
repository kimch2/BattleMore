using UnityEngine;
using System.Collections;

public interface Modifier {



	float modify(float damage, GameObject ProjectileOrUnit, OnHitContainer hitSource, DamageTypes.DamageType theType);


}
