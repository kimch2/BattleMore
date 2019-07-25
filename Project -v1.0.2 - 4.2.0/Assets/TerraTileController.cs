using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraTileController : MonoBehaviour
{
    public int Width = 19;
    public int Height = 9;
    public int StartingControlNumber = 2;
    public Vector3 LowerLeftCorner;
    public GameObject TilePrefab;

    public ModularAura PlayerOneAura;
    public ModularAura PlayerTwoAura;

    TerraCraftTile[,] TileGrid;
    float TileSize;


    public static TerraTileController main;
    private void Awake()
    {
        main = this;
        TileSize = TilePrefab.transform.localScale.z;
  
         TileGrid = new TerraCraftTile[Width,Height];
        for (int i = 0; i < Width; i++)
        {

            for (int j = 0; j < Height; j++)
            {
                Vector3 position = LowerLeftCorner + Vector3.right * TilePrefab.transform.localScale.x * i;
                position += Vector3.forward * TilePrefab.transform.localScale.z * j;
                GameObject tile = Instantiate<GameObject>(TilePrefab, position, Quaternion.identity);
                tile.transform.SetParent(this.transform);
                TerraCraftTile tileComponent = tile.GetComponent<TerraCraftTile>();
                TileGrid[i, j] = tileComponent;

                if (i < StartingControlNumber)
                {
                    tileComponent.Capture(1);
                }
                else if (i > Width - StartingControlNumber -1)
                {
                    tileComponent.Capture(2);
                }
            }
        }
        LowerLeftCorner -= TilePrefab.transform.localScale / 2;
        AstarPath.active.Scan();
    }

    public bool onFriendlyTile(UnitManager manager)
    {
        Vector3 relativeLocation = manager.transform.position - LowerLeftCorner;
        relativeLocation /= TileSize;
       return (TileGrid[(int)relativeLocation.x, (int)relativeLocation.z].PlayerOwner == manager.PlayerOwner);
    }

    public void ApplyAura(UnitManager manager)
    {
        if (manager.PlayerOwner == 1)
        {if(PlayerOneAura)
            PlayerOneAura.ApplyBuff(manager);
        }
        else {
            if(PlayerTwoAura)
            PlayerTwoAura.ApplyBuff(manager);
        }
    }

    public void RemoveAura(UnitManager manager)
    {
        if (manager.PlayerOwner == 1)
        {
            if(PlayerOneAura)
            PlayerOneAura.RemoveBuff(manager);
        }
        else
        {
            if(PlayerTwoAura)
            PlayerTwoAura.RemoveBuff(manager);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(LowerLeftCorner, 1);
    }

}
