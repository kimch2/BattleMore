using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthBarState  {


	// Update is called once per frame
	void UpdateHealthBar (float m);
	void UpdateCoolDown (float m);
	void updateEnergy(float m);
}
