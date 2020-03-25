using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SimpleAnimator))]

public class SimpleAnimEditor : Editor
{

    bool Playing;
    int StartIndex;
    int LoopIndex;
    float NextFrameTime;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Playing)
        {
            SimpleAnimator targ = (SimpleAnimator)target;

            if (GUILayout.Button("Stop"))
            {
                Playing = false;
            }
            if (Time.realtimeSinceStartup > NextFrameTime)
            {

                NextFrameTime += (1/targ.frameRate);

                if (targ.StartSprites.Count > 0 && StartIndex < targ.StartSprites.Count)
                {
                    targ.myRenderer.sprite = targ.StartSprites[StartIndex];
                    if (StartIndex == targ.StartSprites.Count - 1 && targ.LoopSprites.Count == 0)
                    {
                        Playing = false;
                        StartIndex = 0;
                        LoopIndex = 0;
                    }
                    else
                    {
                        StartIndex++;
                    }
                }
                else if (targ.LoopSprites.Count > 0)
                {
                    targ.myRenderer.sprite = targ.LoopSprites[LoopIndex];
                    LoopIndex++;
                    if (LoopIndex >= targ.LoopSprites.Count)
                    {
                        LoopIndex = 0;
                    }
                }
            }
            EditorUtility.SetDirty(target);
        }
        else
        {
            if (GUILayout.Button("Play"))
            {
                NextFrameTime = Time.realtimeSinceStartup + (1 / ((SimpleAnimator)target).frameRate);
                Playing = true;
                StartIndex = 0;
                LoopIndex = 0;
            }
        }
      
    }
}
