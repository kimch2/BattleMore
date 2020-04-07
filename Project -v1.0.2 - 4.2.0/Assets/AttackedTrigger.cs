using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedTrigger : MonoBehaviour, Modifier
{
    //Used to warn the play that this guy is being attacked specifically, like a payload

    public List<int> VoiceLines = new List<int>();
    public float TimeBetweenPlaying;
    private float nextActionTime;
   
    // Use this for initialization
    void Start()
    {
        nextActionTime = Time.time + 1;
       
      GetComponent<UnitStats>().addModifier(this);
    }





    public float modify(float damage, GameObject source, OnHitContainer hitSource, DamageTypes.DamageType theType)
    {
        if (Time.time > nextActionTime)
        {
 
             dialogManager.instance.playLine(VoiceLines[ Random.Range( 0, VoiceLines.Count)]);
            nextActionTime = Time.time + TimeBetweenPlaying;
        }
        return damage;
    }
}
