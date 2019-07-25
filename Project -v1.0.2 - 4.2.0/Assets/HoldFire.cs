using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldFire : Ability
{

    public IWeapon toDisable;
    public Sprite holdFireIcon;
    public Sprite WeaponsFree;
   public  bool WeaponDisabled = false;

    public override void Start() // We have this here so other source can call Start and any of this guy's inheriters will have it called instead
    {
        myType = type.activated;
    }

    public override void Activate()
    {
        StartCoroutine(TrueActivate());
    }

    IEnumerator TrueActivate()
    {// We call this from an invoke because of a Collection MOdified error that happens when the weapon gets removed while iterating through its triggers.
        yield return null;
        if (myManager.myWeapon.Contains(toDisable))
        {
            iconPic = holdFireIcon;
            myManager.removeWeapon(toDisable);
            Descripton = "Enable Air-to-Ground Missiles.";
            description = "Enable Air-to-Ground Missiles.";
        }
        else
        {
            iconPic = WeaponsFree;
            myManager.setWeapon(toDisable);
            description = "Disable Air-to-Ground Missiles.";
            Descripton = "Disable Air-to-Ground Missiles.";

        }
        WeaponDisabled = !WeaponDisabled;
        RaceManager.upDateUI();
    }

    public override continueOrder canActivate(bool error)
    {
        return new continueOrder(true, true);
    }

    public override void setAutoCast(bool offOn)
    {
      
    }
}
