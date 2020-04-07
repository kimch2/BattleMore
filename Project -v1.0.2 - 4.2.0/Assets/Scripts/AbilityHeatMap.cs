using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHeatMap: MonoBehaviour
{
    //This class is used by Enemy AI, to determine where to dodge to avoid enemy abilities
    public static AbilityHeatMap main;
    List<CircleData> circles = new List<CircleData>();
    List<LineData> lines = new List<LineData>();
    List<DangerZone> dangerZones = new List<DangerZone>();
    public List<ChampionAI> AIListeners = new List<ChampionAI>();
    public UnitManager PlayerCharacters;
    public GameObject circleAreaTemplate;
    public GameObject lineAreaTemplate;

    public bool currentlySafe;


    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        foreach (ChampionAI ai in AIListeners)
        {
            if (!ai)
            {
                continue;
            }

            List<DangerZone> Zones = new List<DangerZone>();
            foreach (DangerZone zone in dangerZones)
            {
                if (zone.InDangerZone(ai.transform.position, ai.myManager.CharController.radius))
                {
                    Zones.Add(zone);                        
                }
            }
            if (Zones.Count > 0)
            {
                currentlySafe = false;
                ai.InDangerZone(Zones);
            }
            else
            {
                currentlySafe = true;
                ai.InSafeZone();
            }
        }
    }

    public void AddChampionListener(ChampionAI ai)
    {
        AIListeners.Add(ai);
    }


    public Vector3 GetRunToLocation(List<DangerZone> zones)
    {
        return Vector3.one;
    }

    public bool IsSafe(List<DangerZone> specificZones, Vector3 testPoint, ChampionAI ai)
    {
        foreach (DangerZone zone in specificZones)
        {
            if (zone.InDangerZone(testPoint, ai.myManager.CharController.radius))
            {
                return false;
            }
        }
        return true;
    }


    public bool IsSafe(Vector3 testPoint, ChampionAI ai)
    {
        foreach (DangerZone zone in dangerZones)
        {
            if (zone.InDangerZone(testPoint, ai.myManager.CharController.radius))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="location"></param>
    /// <param name="radius"></param>
    /// <param name="source"></param>
    /// <param name="LandTime"></param>
    /// <param name="priority"></param>
    public void AddCircleWarning(Vector3 location, float radius, Object source, float LandTime, float priority)
    {
        GameObject icon = Instantiate<GameObject>(circleAreaTemplate);
        icon.GetComponent<SpriteRenderer>().color = new Color(priority, 0 ,0);
        icon.transform.localScale = Vector3.one * radius;
        icon.transform.position = location;

        CircleData data = new CircleData(location, radius, source, LandTime, priority, icon);
        dangerZones.Add(data);
    }

    public void RemoveArea(Object Source)
    {
        
        foreach (DangerZone data in dangerZones.FindAll(item => item.source == Source))
        {
            Destroy(data.icon);
        }
        dangerZones.RemoveAll(item => item.source == Source);
    }

    public void AddLine(Vector3 origin, Vector3 endPoint, float width, Object source, float LandTime, float priority)
    {
        GameObject icon = Instantiate<GameObject>(lineAreaTemplate);
        icon.GetComponentInChildren<SpriteRenderer>().color = new Color(priority, 0, 0,.5f);
        icon.transform.localScale = new Vector3(width,1, Vector3.Distance(origin, endPoint));
        icon.transform.position = origin ;
        icon.transform.LookAt(endPoint);

        LineData data = new LineData(origin, endPoint, width, source, LandTime, priority, icon);
        dangerZones.Add(data);
    }

    public void UpdateLine(Vector3 origin, Vector3 endPoint, Object Source)
    {
        DangerZone z = dangerZones.Find(item => item.source == Source);
        ((LineData)z).update(origin, endPoint);
    }


    public abstract class DangerZone
    {
        public Object source;
        public float landTime;
        public float priority;
        public GameObject icon;

        public abstract bool InDangerZone(Vector3 position, float unitRadius);
    }

    class CircleData : DangerZone {
        public Vector3 center;
        public float radius;

        public CircleData(Vector3 location, float radius, Object source, float landTime, float priority, GameObject Icon)
        {
            center = location;
            this.radius = radius;
            this.source = source;
            this.landTime = landTime;
            this.priority = priority;
            icon = Icon;
        }

        public override bool InDangerZone(Vector3 position, float unitRadius)
        {
            //Debug.Log("Distance " + Vector3.Distance(center, position) + unitRadius);
            return Vector3.Distance(center, position) < (radius + unitRadius);
        }
    }

    class LineData : DangerZone {
        public Vector3 StartPoint;
        public Vector3 EndPoint;
        public Vector3 TravelSpeed;
        public float HalfWidth;

        public LineData(Vector3 origin, Vector3 endPoint, float Width, Object source, float landTime, float priority, GameObject icon)
        {
            HalfWidth = Width/2;
            StartPoint = origin;
            EndPoint = endPoint;
            this.source = source;
            this.landTime = landTime;
            this.priority = priority;
            this.icon = icon;
        }

        public void update(Vector3 newOrigin, Vector3 NewEnd)
        {
            StartPoint = newOrigin;
            EndPoint = NewEnd;
            icon.transform.position = newOrigin;
            icon.transform.localScale = new Vector3(HalfWidth *2, 1, Vector3.Distance(StartPoint, EndPoint));
            icon.transform.position = StartPoint;
            icon.transform.LookAt(EndPoint);
        }

        public override bool InDangerZone(Vector3 position, float unitRadius)
        {
            return Vector3.Distance(ClosestPointOnLineSegment(position), position) - unitRadius < HalfWidth;
        }

        Vector3 ClosestPointOnLineSegment(Vector3 point)
        {
            // Shift the problem to the origin to simplify the math.    
            Vector3 wander = point - StartPoint;
            Vector3 span = EndPoint - StartPoint;

            // Compute how far along the line is the closest approach to our point.
            float t = Vector3.Dot(wander, span) / span.sqrMagnitude;

            // Restrict this point to within the line segment from start to end.
            t = Mathf.Clamp01(t);

            // Return this point.
            return StartPoint + t * span;
        }
    }


  

}
