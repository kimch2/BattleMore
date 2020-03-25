using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEffector : TriggeredAbility
{
    // Class that does effects specifically to your hero
    // Maybe this should be separate from the TriggeredAbility? (but can still be called from it)

    public void reduceCooldowns(float amount)
    {
        foreach (Ability ab in DaminionsInitializer.main.MyHero.abilityList)
        {
            Debug.Log("Reducing " + ab.Name + "  " +  ab.myCost.cooldownTimer);
            if (ab.myCost )
            {
                ab.myCost.cooldownTimer -= amount;
            }
        }
    }
}
