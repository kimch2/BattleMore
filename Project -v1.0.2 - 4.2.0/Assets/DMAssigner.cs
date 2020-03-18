using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DMAssigner : MonoBehaviour,IDragHandler,IDropHandler, IEndDragHandler
{
    public enum DMAssignmentType { unit, ability, relic}
    public DMAssignmentType myType;
    public DMUnitCard assignedCard;
    public Text firstText;
    public Text secondText;
    public int ChosenNumber = 1;

    public void AssignCard(DMUnitCard newCard)
    {
        DMAssigner temp = newCard.currentlyAssignedTo;

        newCard.UnAssign();

        DMUnitCard current = assignedCard;
        if (current)
        {
            current.UnAssign();
            if (temp)
            {
                current.AssignToCard(temp);
            }
        }

        assignedCard = newCard;
        AssignCosts(newCard);
    }

    public void UnAssign()
    {
        firstText.text = "" ;
        secondText.text = "" ;
        assignedCard = null;
    }

    public void AssignCosts(DMUnitCard toassign)
    {
        if (toassign.MyUnit)
        {
            UnitStats stats = toassign.MyUnit.GetComponent<UnitStats>();
            firstText.text = "$" + stats.cost;
            secondText.text = "#" + stats.supply;
        }
        else if (toassign.MyAbility)
        {
            Ability MyAbility = toassign.MyAbility.GetComponent<Ability>();
            firstText.text = "";
            if (MyAbility.chargeCount > 0)
            {
                firstText.text += MyAbility.myCost.cooldown;
            }

            secondText.text = "";
            if (MyAbility.chargeCount > 0) {
                secondText.text +=MyAbility.chargeCount;
            }
        }
        else if (toassign.myUpgrade)
        {
            firstText.text = "";
            secondText.text = "";
        }
        else
        {
            firstText.text = "";
            secondText.text = "";
        }
        
    }


    public void OnDrop(PointerEventData eventData)
    {
        bool foundOne = false;
        if (DMCollectionManager.instance.currentDragger != null)
        {
            DMUnitCard card = DMCollectionManager.instance.currentDragger.GetComponent<DMUnitCard>();
            if (myType == DMAssignmentType.unit && card.MyUnit != null)
            {
                foundOne = true;                    
            }

            else if (myType == DMAssignmentType.ability && card.MyAbility != null)
            {
                foundOne = true;
            }
            else if (myType == DMAssignmentType.relic && card.myUpgrade != null)
            {
                foundOne = true;
            }
        }

        if (foundOne)
        {
            DMCollectionManager.instance.currentReceptacle = this;
        }
         
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (assignedCard) {
            assignedCard.OnDrag(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (assignedCard)
        {
            assignedCard.OnEndDrag(eventData);
        }
    }
}
