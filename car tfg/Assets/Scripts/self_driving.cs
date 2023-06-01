using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_driving : MonoBehaviour
{
    private crosPoint crosuPoint;
    private Vector3 vektor;
    private float speed = .05f;
    private Collider cross;

    private float force = 200;
    private Rigidbody _rigidbody;


    private sus_physics right_front_wheel;
    private sus_physics left_front_wheel;
    private sus_physics left_rear_wheel;
    private sus_physics right_rear_wheel;

    void Start()
    {
        crosuPoint = FindObjectOfType<crosPoint>();
        cross = crosuPoint.GetComponent<Collider>();
        vektor = new Vector3(crosuPoint.transform.position.x,transform.position.y, crosuPoint.transform.position.z);
        
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
        //transform.position = Vector3.MoveTowards(transform.position, vektor, speed);
        transform.LookAt(vektor);
        _rigidbody.AddForceAtPosition(-left_rear_wheel.transform.right * force, left_rear_wheel.transform.position);
        _rigidbody.AddForceAtPosition(-right_rear_wheel.transform.right * force, right_rear_wheel.transform.position);
    }
}
