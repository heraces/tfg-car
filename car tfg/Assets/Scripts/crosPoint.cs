using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crosPoint : MonoBehaviour
{
    public crosPoint[] connections;

    private GameObject control_spotlight;
    private crosPoint[] crosPoints;
    private crosPoint lastCrosPoint;
    private int number_of_turns = 5;

    public bool isTarget = false;

    private void Start()
    {
        control_spotlight = transform.GetChild(0).gameObject;
        control_spotlight.SetActive(false);
    }

    public crosPoint[] findPath(crosPoint lastPoint)
    {
        lastCrosPoint = lastPoint;
        crosPoints = connections;
        number_of_turns = Random.Range(4, 7);
        crosPoint[] newCrosPoints = new crosPoint[number_of_turns];

        for (int i = 0; i < number_of_turns; i++)
        {
            int index = Random.Range(0, crosPoints.Length);
            while (i >= 2 && newCrosPoints[i - 2] == crosPoints[index])
            {
                index = Random.Range(0, crosPoints.Length);
            }
            newCrosPoints[i] = crosPoints[index];
            lastCrosPoint = crosPoints[index];
            crosPoints = lastCrosPoint.connections;
        }

        return newCrosPoints;
    }

    public void LightsOn()
    {
        isTarget = true;
        control_spotlight.SetActive(true);
    }

    public void LightsOff()
    {
        isTarget = false;
        control_spotlight.SetActive(false);
    }


}
