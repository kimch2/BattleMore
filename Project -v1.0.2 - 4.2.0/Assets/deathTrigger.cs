using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class deathTrigger : MonoBehaviour {

	public int player = 1;

	public  int index;
	public  float input;
	public  Vector3 location;
	public  GameObject target; 
	public  bool doIt;
	public float delay;

    private void Start()
    {
        
    }
    public List<SceneEventTrigger> myTriggers;


	public UnityEngine.Events.UnityEvent OnDeath;

	public void Dying()
    {
        if (this.enabled && this.gameObject.activeInHierarchy)
        {
            foreach (SceneEventTrigger trig in myTriggers)
            {
                trig.trigger(index, input, location, target, doIt);
            }
            OnDeath.Invoke();
        }

	}






}
