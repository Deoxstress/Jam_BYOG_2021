using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private GameObject[] Waypoint;
    [SerializeField] private float[] distanceArray;
    [SerializeField] private float totaldistance;
    [SerializeField] private GameObject waypointData;
    [SerializeField] private float totalSpeed;
    [SerializeField] private float timer = 5f;
    [SerializeField] public float waitTimer;
    [SerializeField] public float maxTimer;
    [SerializeField] private bool revert;
    public bool isLateral;
    
    [Header("WAYPOINTS")]

    [SerializeField] private int waypointCount = 0;
    [SerializeField] private GameObject lastWP;
    [SerializeField] public GameObject firstWP;
    public bool debugUP;
    void Start()
    {
        Waypoint = new GameObject[waypointData.transform.childCount];
        for (int i = 0; i < waypointData.transform.childCount; i++)
        {
            Waypoint[i] = waypointData.transform.GetChild(i).gameObject;
        }
        transform.position = Waypoint[0].transform.position;
        SpeedByDistance();
        lastWP = Waypoint[Waypoint.Length - 1];
        firstWP = Waypoint[0];
    }
    void Update()
    {
        if (waitTimer > 0) waitTimer -= Time.deltaTime;

        if (waypointCount == Waypoint.Length - 1) {revert = true;}
        else if (waypointCount == 0) { revert = false;}


        if (transform.position != Waypoint[waypointCount].transform.position && waitTimer <= 0)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, Waypoint[waypointCount].transform.position, totalSpeed * Time.deltaTime);
        }
        else if (transform.position == Waypoint[waypointCount].transform.position)
        {
            
            if (revert)
                waypointCount--;
            else
                waypointCount++;
        }

        if (transform.position == lastWP.transform.position && Waypoint[waypointCount] == lastWP || transform.position == firstWP.transform.position && Waypoint[waypointCount] == firstWP) waitTimer = maxTimer;
    }
    void SpeedByDistance()
    {
        int index;
        distanceArray = new float[Waypoint.Length - 1];

        for (index = 0; index < Waypoint.Length - 1; index++)
        {
            distanceArray[index] = Vector3.Distance(Waypoint[index].transform.position, Waypoint[index + 1].transform.position);
        }

        if (distanceArray.Length == 1)
        {
            totaldistance = distanceArray[0];
        }
        else if (distanceArray.Length > 1)
        {
            foreach (int distance in distanceArray)
            {
                totaldistance += distance;
               
            }
        }
        totalSpeed = (totaldistance/timer);
    }
}
