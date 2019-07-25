using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CinematicCamera))]
public class CinematicCameraEditor : Editor
{

    int currentScene;
    int currentShot = -1;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(40);
        currentScene = EditorGUILayout.IntField( "Scene Number", currentScene);
        currentShot = EditorGUILayout.IntField( "Shot Number", currentShot);

        if (GUILayout.Button("Place Start"))
        {
            CinematicCamera.scene myScene = ((CinematicCamera)target).myScenes[currentScene];
            int shotNumber = currentShot != -1 ? currentShot : ((CinematicCamera)target).myScenes[currentScene].myShots.Count;
            if (shotNumber > myScene.myShots.Count - 1)
            {
                myScene.myShots.Add(new CinematicCamera.shot());
            }
            CinematicCamera.shot myShot = myScene.myShots[shotNumber];
            myShot.startLocation = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;

            myShot.startTarget = getTarget(myShot.startLocation);
            
        }


        if (GUILayout.Button("Place End"))
        {
            CinematicCamera.scene myScene = ((CinematicCamera)target).myScenes[currentScene];
            int shotNumber = currentShot != -1 ? currentShot : ((CinematicCamera)target).myScenes[currentScene].myShots.Count-1;
            if (shotNumber > myScene.myShots.Count - 1)
            {
                myScene.myShots.Add(new CinematicCamera.shot());
            }
            CinematicCamera.shot myShot = myScene.myShots[shotNumber];
            myShot.endLocation = UnityEditor.SceneView.lastActiveSceneView.camera.transform.position;

            myScene.myShots[shotNumber].endTarget = getTarget(myShot.endLocation);
          
        }

    }

    Vector3 getTarget( Vector3 Origin)
    {
        Ray ray = new Ray();
        ray.origin = Origin;
        ray.direction = UnityEditor.SceneView.lastActiveSceneView.camera.transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000, 1 << 8 | 1 << 11 | 1 << 16))
        {
           return hit.point;
        }
        return Vector3.zero;
    }

}
