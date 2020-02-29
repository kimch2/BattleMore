using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


	public RaceManager[] playerList = new RaceManager[3];
	public int playerNumber;
	public RaceManager activePlayer;
	private RaceManager playerOne;
	private RaceManager playerTwo;

	private bool initialized = false;
	public static GameManager main;
	public 	List<MiniMapUIController> MiniMaps;

	public static GameManager getInstance()
	{
		if (main == null) {

			main = GameObject.FindObjectOfType<GameManager>();
		}
		return main;
	}

	// Use this for initialization
	void Awake () {
		main = this;

		foreach (MiniMapUIController min in MiniMaps) {
			if (min) {
				min.DubAwake ();
			}
		}

	}

	void Start()
	{
		foreach (MiniMapUIController min in MiniMaps) {
			if (min) {
				min.Initialize ();
			}
		}
	}


	public void initialize()
		{
		if (!initialized) {
			initialized = true;
		}
	}

	public RaceManager getActivePlayer()
	{
		return activePlayer;
	}





    /// <summary>
    ///  The Active Player is 1, enemy is 2, nuetral is 3, 4+ are other enemies
    /// </summary>
    /// <param name="Origin"></param>
    /// <param name="playerNumber"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static List<UnitManager> GetUnitsInRange(Vector3 Origin, int playerNumber, float radius)
    {

        radius /= 2;
        List<UnitManager> toTarget = new List<UnitManager>();

        foreach (KeyValuePair<string, List<UnitManager>> unitList in GameManager.main.playerList[playerNumber - 1].getUnitList())
        {
            foreach (UnitManager unit in unitList.Value)
            {
                if (unit == null)
                {
                    continue;
                }
                if (new Vector2(unit.transform.position.x - Origin.x, unit.transform.position.z - Origin.z).sqrMagnitude <= Mathf.Pow(radius + unit.CharController.radius, 2))
                {
                    toTarget.Add(unit);
                }
            }
        }
        return toTarget;
    }



}

