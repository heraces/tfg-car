using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_driving : MonoBehaviour
{
    private crosPoint crosuPoint;
    private Vector3 vektor;
    private crosPoint targetCrossPoint;

    private float turn_speed = 15;

    private float force = 60;
    private Rigidbody _rigidbody;


    private sus_physics right_front_wheel;
    private sus_physics left_front_wheel;
    private sus_physics left_rear_wheel;
    private sus_physics right_rear_wheel;


    void Start()
    {
        crosuPoint = FindObjectOfType<crosPoint>();
        targetCrossPoint = crosuPoint;

        findFirstPoint();
        vektor = targetCrossPoint.transform.position;
        //findRout();

        _rigidbody = GetComponent<Rigidbody>();
        foreach (sus_physics item in GetComponentsInChildren<sus_physics>())
        {
            if (item.wheel_Place == sus_physics.wheel_place.FrontLeft)
            {
                left_front_wheel = item;
            }
            else if (item.wheel_Place == sus_physics.wheel_place.FrontRight)
            {
                right_front_wheel = item;
            }
            else if (item.wheel_Place == sus_physics.wheel_place.BackLeft)
            {
                left_rear_wheel = item;
            }
            else if (item.wheel_Place == sus_physics.wheel_place.BackRight)
            {
                right_rear_wheel = item;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = vektor - transform.position;

        if (Vector3.SignedAngle(transform.forward, relativePosition, Vector3.up) > 0) {
            transform.Rotate(new Vector3(0, transform.rotation.y + turn_speed, 0) * Time.deltaTime); 
        }

        else if (Vector3.SignedAngle(transform.forward, relativePosition, Vector3.up)< 0)
        {
            transform.Rotate(new Vector3(0, transform.rotation.y - turn_speed, 0) * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        foreach (sus_physics item in GetComponentsInChildren<sus_physics>())
        {
            Vector3 steeringDirR = item.transform.right;
            float steeringVelR = Vector3.Dot(steeringDirR, _rigidbody.GetPointVelocity(item.transform.position));
            float desiredVelchangeR = -steeringVelR * .1f;
            float accR = desiredVelchangeR / Time.fixedDeltaTime;


            _rigidbody.AddForceAtPosition(steeringDirR * accR * _rigidbody.mass / 4, item.transform.position);

        }

        _rigidbody.AddForceAtPosition(left_rear_wheel.transform.forward * force / Time.fixedDeltaTime, left_rear_wheel.transform.position);
        _rigidbody.AddForceAtPosition(right_rear_wheel.transform.forward * force / Time.fixedDeltaTime, right_rear_wheel.transform.position); 
    }

    private void findFirstPoint()
    {
        foreach(crosPoint point in FindObjectsOfType<crosPoint>())
        {
            if(Vector3.Distance(transform.position, point.transform.position) < Vector3.Distance(transform.position, targetCrossPoint.transform.position))
            {
                targetCrossPoint = point;
            }
        }
    }

    private void findRout()
    {
        crosPoint actualPoint = targetCrossPoint;
        List<crosPoint> route = findRout(actualPoint, new List<crosPoint>());
        for(int i = 0; i < route.Count; i++)
        {
            Debug.Log(route[i]);
        }

        Debug.Log(targetCrossPoint);
        Debug.Log(crosuPoint);
    }
    private List<crosPoint> findRout(crosPoint actualPoint, List<crosPoint> list)
    {

        actualPoint.visited = true;
        if (actualPoint == crosuPoint)
        {
            list.Add(actualPoint);
            actualPoint.visited = false;
            return list;
        }
        foreach( crosPoint line in actualPoint.connections)
        {
            if (!line.visited)
            {
                findRout(line, list);
            }
        }
        return list;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == targetCrossPoint.gameObject)
        {
            targetCrossPoint = other.GetComponent<crosPoint>().connections[Random.Range(0, targetCrossPoint.connections.Length)];
            vektor = targetCrossPoint.transform.position;
            Vector3 relativePosition = vektor - transform.position;
            while(Vector3.Angle(transform.forward, relativePosition) > 160)
            {
                targetCrossPoint = other.GetComponent<crosPoint>().connections[Random.Range(0, targetCrossPoint.connections.Length)];
                vektor = targetCrossPoint.transform.position;
                relativePosition = vektor - transform.position;
            }

            Debug.Log(targetCrossPoint);
        }
    }
}
