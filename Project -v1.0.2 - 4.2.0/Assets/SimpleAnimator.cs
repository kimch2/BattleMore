using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    //THis class is used to have sprites be animated on a spriteRenderer or Image without having to do a whol animation file

    public SpriteRenderer myRenderer;
    public float frameRate = 18;
    [Tooltip("The StartSprites are played only once, then the Loop Sprites play indefinitely, If there are no loop sprites, this will disapear once the last Start frame is shown")]
    public List<Sprite> StartSprites;

    public List<Sprite> LoopSprites;
    [Tooltip("This will Disable this object, If you want to replay, call the start function, if 0, this will run indefinitly")]
    public float Duration;
    
    float TurnOffTime;
    Coroutine myRoutine;

    public void OnEnable()
    {
        if (myRoutine != null)
        {
            StopCoroutine(myRoutine);
        }
        myRenderer.enabled = true;
        enabled = true;
        if (StartSprites.Count > 0 || LoopSprites.Count > 0)
        {
            if (Duration > 0)
            {
                TurnOffTime = Time.time + Duration;
            }
            else
            {
                TurnOffTime = Time.time + 100000;
            }
            myRoutine = StartCoroutine(animate());
        }
    }

    

    private void OnDisable()
    {
        if (myRoutine != null)
            StopCoroutine(myRoutine);
        myRoutine = null;        
    }

    IEnumerator animate()
    {
        float frameLength = 1 / frameRate;
        int currentIndex = 0;

        while (currentIndex < StartSprites.Count)
        {
            myRenderer.sprite = StartSprites[currentIndex];
            yield return new WaitForSeconds(frameLength);
            currentIndex++;
        }
        if (LoopSprites.Count > 0)
        { 
            currentIndex = 0;

            while (true)
            {
                myRenderer.sprite = LoopSprites[currentIndex % LoopSprites.Count];
                yield return new WaitForSeconds(frameLength);
                currentIndex++;
                if (Time.time > TurnOffTime)  // Make a separate function to optimize this?
                {
                    break;
                }
            }
            
        }
        myRenderer.enabled = false;
        myRoutine = null;
    }
}
