using UnityEngine;
using System.Collections;


public interface ManagerWatcher {



	void updateResources(ResourceManager manage);
		

	void updateSupply( float current, float max);

	void updateUpgrades();


}
