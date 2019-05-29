using UnityEngine;
using System.Collections;

public interface Iinteract  {




	void computeInteractions (Order order);

	void initializeInteractor();

	UnitState computeState (UnitState state);



}
