using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    //THis class is used to have sprites be animated on a spriteRenderer or Image without having to do a whol animation file

    public SpriteRenderer myRenderer;
    public float frameRate = 18;
    [Tooltip("The StartSprites are played only once, then the Loop Sprites play indefinitely")]
    public List<Sprite> StartSprites;
    public List<Sprite> LoopSprites;

    private void Start()
    {
        if (StartSprites.Count > 0 || LoopSprites.Count > 0)
        {
            StartCoroutine(animate());
        }
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
            }
        }
    }
}
