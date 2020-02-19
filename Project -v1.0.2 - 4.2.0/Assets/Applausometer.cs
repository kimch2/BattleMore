using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Applausometer : MonoBehaviour , LethalDamageinterface
{

    public List<ApplauseTracker> myTrackers;
    public AudioSource mySource;

    void Start()
    {
        GameManager.main.playerList[0].addActualDeathWatcher(this);
        GameManager.main.playerList[1].addActualDeathWatcher(this);
    }


    void Update()
    {
        float Max = 0;
        foreach (ApplauseTracker track in myTrackers)
        {
            track.Decay();
            Max = Math.Max(Max, track.currentValue);

        }
        Max /= 100;

        float currentVolume = mySource.volume;
        mySource.volume = Mathf.Lerp(currentVolume, Max, Time.deltaTime * 2);
    }


    public bool lethalDamageTrigger(UnitManager unit, GameObject deathSource)
    {
        if (unit.PlayerOwner == 1)
        {
            myTrackers[1].UpdateScore(unit.myStats.supply * 4);
        }
        else
        {
            myTrackers[0].UpdateScore(unit.myStats.supply * 4);
        }
        if (unit.myStats.supply > 2) {
            MainCamera.main.SmashCamera(unit.gameObject, Mathf.Max( .5f, unit.myStats.supply/ 12));
        }
        //CarbotCamera.singleton.SmashCamera(unit.gameObject);
        return true;
    }

    void UpdateSlider(Slider toAdjust, float Value)
    {
        toAdjust.value = Value / 100;
    }

    [System.Serializable]
    public class ApplauseTracker {
        public Slider ApplauseSlider;
        public float currentValue;
        public float NextUpdateTime;

        public void Decay()
        {
            if (NextUpdateTime < Time.time)
            {
                UpdateScore(- Time.deltaTime * (Time.time - NextUpdateTime));
            }
        }


        public void UpdateScore(float amount)
        {
            if (amount > 0)
            {
                NextUpdateTime = Time.time + 2;
            }

            currentValue += amount;
            currentValue = Mathf.Clamp(currentValue, 0,100);
            if(Math.Abs( ApplauseSlider.value - (currentValue / 100)) >  .01f)
            ApplauseSlider.value = currentValue / 100;
        }
      
    }

}


