using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField]
    private engine_diagram _engine;
    private int gas_power;
    private float acc;
    private float rpm = 0;
    private float cc_cylinder;
    private short torque;
    private short hp;
    private float speed = 5;

    private Vector3 past_velocity;
    private float turn_angle = 30;
    private Rigidbody _rigidbody;
    private float window_size;

    //rozamiento
    //depende de: coeficiente de la rueda * coeficiente del suelo

    //to keep track of turning
    private float divider;
    void Start()
    {
        //hp = torque* radiands/sec(not rpm)

        //base dull sinewave sound +
        //explosive sound from nº cylinders * rpm (inside cylinder explision + out exhaust explosion)
        //idle sound almost no sinewave
        //engine brake sounds lower thant engine accelerating
        //high rev sounds higher than low rev

        window_size = Screen.width/2;

        _rigidbody = GetComponent<Rigidbody>();

        if (_engine)
        {

            if (!_engine.gas) { gas_power = 9000; }
            else { gas_power = 9000; }

            rpm = 500;
            cc_cylinder = _engine.cc / _engine.cylinder_number;
            StartCoroutine(revolution());
        }
    }
    private IEnumerator revolution() {

        float rpm = 500;
        while (true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //formula to accelerate
                acc = cc_cylinder * gas_power /10;
            }
            rpm += 3;
            //an explosion is gonna occur every 60/(rpm*no_of_cylinders) seconds
            yield return new WaitForSeconds(1/(rpm/60));
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //_rigidbody.velocity = Vector3.zero;
            Vector3 newVel = _rigidbody.velocity + transform.forward * speed;
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, newVel, Time.deltaTime * 10);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * 10);
            if (_rigidbody.velocity.magnitude < 1)
            {
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_rigidbody.velocity.magnitude) > 0)
        {
            divider = -(window_size - Input.mousePosition.x) / window_size;
            //targetRotation = new Vector3(0, transform.rotation.y + turn_angle * divider, 0);
            Vector3 past_rotation = transform.forward;
            transform.Rotate(new Vector3(0, transform.rotation.y + turn_angle * divider*_rigidbody.velocity.magnitude, 0) * Time.deltaTime);
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, _rigidbody.velocity*9/10, Time.deltaTime * 10);
        }
    }

}
