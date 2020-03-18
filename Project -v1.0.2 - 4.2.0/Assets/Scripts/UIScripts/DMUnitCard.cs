using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DMUnitCard : MonoBehaviour , IDragHandler, IEndDragHandler
{
    public Text Cost;
    public Text UnitCount;
    public Image UnitIcon;
    public Text UnitDescription;
    public Text UnitName;
    public Text UnitTags;
    [Tooltip("This gets filled in at runtime")]
    public GameObject MyUnit;
    public Ability MyAbility;
    public Upgrade myUpgrade;



     Vector3 IconStartPosition = new  Vector3(0,98,0);
    public DMAssigner currentlyAssignedTo;

    public void DestroyMe()
    {
        Destroy(UnitIcon.gameObject);
    }

    public void LoadUnit(GameObject unit)
    {
        MyUnit = unit;

        UnitManager manager = unit.GetComponent<UnitManager>();
        UnitStats stats = unit.GetComponent<UnitStats>();
        Cost.text = stats.cost + "";
        UnitCount.text = "" + stats.supply;

        UnitIcon.sprite = stats.Icon;

        UnitDescription.text = stats.UnitDescription + "\n\nHP: " + stats.Maxhealth + ""
            + (manager.myWeapon.Count > 0 ? ("\nDPS: " + (int)(manager.myWeapon[0].baseDamage / manager.myWeapon[0].attackPeriod)) + " (x"+ stats.supply + ")" + 
            "\nRange: " + manager.myWeapon[0].range : "");

        UnitName.text = manager.UnitName;
        string tagString = "";
        foreach (UnitTypes.UnitTypeTag thingy in stats.otherTags)
        {
            tagString += " #" + thingy.ToString();
        }
        UnitTags.text = tagString;

    }

    public void LoadAbility(GameObject unit)
    {
        MyAbility = unit.GetComponent<Ability>();
        
        Cost.text = "" + MyAbility.myCost.cooldown;
        UnitIcon.sprite = MyAbility.iconPic;
        UnitDescription.text = MyAbility.Descripton;
        UnitName.text = MyAbility.Name;

        UnitTags.text = "";
        UnitCount.text = "" + MyAbility.chargeCount;
    }

    public void LoadRelic(GameObject unit)
    {
        myUpgrade = unit.GetComponent<Upgrade>();

        Cost.text = "";
        UnitIcon.sprite = myUpgrade.iconPic;
        UnitDescription.text = myUpgrade.Descripton;
        UnitName.text = myUpgrade.Name;

        UnitTags.text = "";
        UnitCount.text = "";
    }

    public void OnDrag(PointerEventData eventData)
    {
        UnitIcon.transform.position = Input.mousePosition;
        //Debug.Log("A" + UnitIcon);
        //Debug.Log("B" + DMCollectionManager.instance);
        UnitIcon.transform.SetParent(DMCollectionManager.instance.transform);
        DMCollectionManager.instance.currentDragger = this;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DMCollectionManager.instance.currentReceptacle)
        {
            AssignToCard(DMCollectionManager.instance.currentReceptacle);
        }
        else
        {
            UnAssign();
        }
        DMCollectionManager.instance.currentDragger = null;
        DMCollectionManager.instance.currentReceptacle = null;
    }

    public void UnAssign()
    {
        UnitIcon.transform.SetParent(this.transform);
        UnitIcon.transform.localPosition = IconStartPosition;
        if (currentlyAssignedTo)
        {
            if (MyUnit)
            {
                DMCollectionManager.AssignUnit(currentlyAssignedTo.ChosenNumber, null);
            }
            else if (MyAbility)
            {
                DMCollectionManager.AssignAbility(currentlyAssignedTo.ChosenNumber, null);
            }
            currentlyAssignedTo.UnAssign();
        }

        currentlyAssignedTo = null;
    }

    public void AssignToCard(DMAssigner assigner)
    {
        assigner.AssignCard(this);
        currentlyAssignedTo = assigner;
        UnitIcon.transform.position = assigner.transform.position;
        UnitIcon.transform.SetParent(assigner.transform);

        if (MyUnit)
        {
            DMCollectionManager.AssignUnit(assigner.ChosenNumber, MyUnit);
        }
        else if (MyAbility)
        {
            DMCollectionManager.AssignAbility(assigner.ChosenNumber, MyAbility.gameObject);
        }
    }
}
