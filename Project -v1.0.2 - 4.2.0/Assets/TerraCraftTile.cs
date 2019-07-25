using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraCraftTile : MonoBehaviour
{
    public int PlayerOwner;
    public MeshRenderer myRenderer;
    public Material PlayerTexture;
    public Material EnemyTexture;
    public Material NuetralTexture;
    public GameObject PlayerCaptureEffect;
    public GameObject EnemyCaptureEffect;
    public bool isSpawnZone;
    private void Start()
    {
        int TempOwner = PlayerOwner;
        PlayerOwner = -1;
            Capture(TempOwner);
    }


    public void Capture(int playerNumber)
    {
        if (PlayerOwner != playerNumber)
        {
            PlayerOwner = playerNumber;
            if (playerNumber == 0)
            {
                myRenderer.material = NuetralTexture;// Nuetral

            }
            else if (playerNumber == 1)
            {
                myRenderer.material = PlayerTexture;
                Instantiate(PlayerCaptureEffect, this.transform.position, PlayerCaptureEffect.transform.rotation);
            }
            else
            {
                myRenderer.material = EnemyTexture;
                Instantiate(EnemyCaptureEffect, this.transform.position, EnemyCaptureEffect.transform.rotation);
            }
        }
    }



}
