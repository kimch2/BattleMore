﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBar : MonoBehaviour
{


    public string usedFor;
    public GameObject colorBar;
    public SpriteRenderer sprite;

    public List<HealthThreshHold> RatioLevels;
    Vector3 healthVector = new Vector3(1, 1, 1);
    public float BarWidth = 9;

    SpriteRenderer myColorSprite;

    protected void Awake()
    {
        myColorSprite = colorBar.GetComponentInChildren<SpriteRenderer>(true);

    }

    protected void Start()
    {
        healthVector.y = colorBar.transform.parent.localScale.y;
        sprite.size = new Vector2(BarWidth, sprite.size.y);
        GetComponentInChildren<SpriteRenderer>().size = new Vector2(BarWidth, sprite.size.y);
        sprite.transform.parent.localPosition = new Vector3(BarWidth / 2, 0, 0);
        sprite.transform.localPosition = new Vector2(0 - BarWidth / 2, 0);
    }



    /// <summary>
    /// Returns whether it is on or not
    /// </summary>
    /// <returns><c>true</c>, if ratio was updated, <c>false</c> otherwise.</returns>
    public virtual bool updateRatio(float ratio, UnitIconInfo unitIcon, UnitIconInfo slider)
    {
        bool changeTo = (ratio < .99f && ratio > 0); // (ratio < .99 && ratio > 0);

        if (changeTo != gameObject.activeSelf)
        {
            gameObject.SetActive(changeTo);
        }

        if (slider)
        {
            slider.updateSlider(ratio);
        }

        if (changeTo)
        {
            healthVector.x = ratio;
            colorBar.transform.localScale = healthVector;
            foreach (HealthThreshHold hold in RatioLevels)
            {
                if (ratio > hold.minimum)
                {
                    sprite.color = hold.HPBarColor;
                    if (unitIcon)
                    {
                        unitIcon.changeColor(hold.HPBarColor);
                    }
                    break;
                }
            }
        }
        return this.gameObject.activeSelf;
    }


    public float getRatio()
    {
        return colorBar.transform.localScale.x;
    }


    public Color getBarColor()
    {
        if (!myColorSprite)
        {
            myColorSprite = colorBar.GetComponentInChildren<SpriteRenderer>(true);
        }
        return myColorSprite.color;

    }
    public virtual void SetShieldAmount(float amount) // Need a way to keep track of health that is subtracted while you still have a shield
    {

    }
}
[System.Serializable]
public class HealthThreshHold
{
	public float minimum;
	public Color HPBarColor;

}
