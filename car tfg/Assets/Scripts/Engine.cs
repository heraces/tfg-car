using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Engine : MonoBehaviour
{
    //retreaving values from scriptable object
    [SerializeField]
    private engine_diagram _engine;

    public suspension_stats suspension;
    public Car_model car;

    private int gas_power;
    private float acc;
    private float rpm = 0;
    private float cc_cylinder;
    private short torque;
    private short hp;
    private float speed = 5;
    private AudioSource s;

    //to be soterd on a tyre scriptable object
    private float tire_grip = .8f;

    private float turn_angle = 30;
    private Rigidbody _rigidbody;
    private float window_size;

    private float ackerman_angle_left;
    private float ackerman_angle_right;

    private sus_physics right_front_wheel;
    private sus_physics left_front_wheel;
    private sus_physics left_rear_wheel;
    private sus_physics right_rear_wheel;

    private short gear = 1;

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


        s = GetComponent<AudioSource>();

        //soudn hz = rpm/60
        window_size = Screen.width/2;

        _rigidbody = GetComponent<Rigidbody>();
        foreach(sus_physics item in GetComponentsInChildren<sus_physics>())
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

        if (_engine)
        {

            if (!_engine.gas) { gas_power = 9000; }
            else { gas_power = 9000; }

            rpm = 500;
            cc_cylinder = _engine.cc / _engine.cylinder_number;
            //StartCoroutine(revolution());
        }
    }
    private IEnumerator revolution() {

        float rps = 100000; // rpm/60 1 rv/s * 360dg/rv 
        short fire_stage = 0;
        float angle = 0;
        //calculates the firing time of each piston during this frame and calls a threat to make the sound
        while (true)
        {
            angle = 0;
            while (angle < _engine.firing_order[fire_stage])
            {
                angle += 1;
                yield return new WaitForSeconds(1/(360*rps));

            }
            fire_stage += 1;
            if (fire_stage >= _engine.cylinder_number)
            {
                fire_stage = 0;
            }
            s.Play();
        }
    }

    private void Update()
    {
        //ackerman steering 
        //actual steering 
        divider = -(window_size - Input.mousePosition.x) / window_size;// gets the position of the mouse

        if (divider > 0)
        {
            ackerman_angle_left = Mathf.Rad2Deg * Mathf.Atan(car.wheelBase / (car.rearTrack + car.turn_radius) * divider);
            ackerman_angle_right = Mathf.Rad2Deg * Mathf.Atan(car.wheelBase / (car.rearTrack - car.turn_radius) * divider);
        }
        else if(divider < 0)
        {
            //car.wheelBase/car.rearTrack+car.turnRadius will be calculated directly in the scriptable object
            ackerman_angle_left = Mathf.Rad2Deg * Mathf.Atan(car.wheelBase / (car.rearTrack - car.turn_radius) * divider);
            ackerman_angle_right = Mathf.Rad2Deg * Mathf.Atan(car.wheelBase / (car.rearTrack + car.turn_radius) * divider);
        }
        else
        {
            ackerman_angle_left = 0;
            ackerman_angle_right = 0;
        }


        left_front_wheel.ackerman_angle(ackerman_angle_left);
        right_front_wheel.ackerman_angle(ackerman_angle_right);

        float steering = turn_angle * divider  ;
        transform.Rotate(new Vector3(0, transform.rotation.y + steering, 0) * Time.deltaTime);
        //ackerman steering 
        //actual steering 

    }

    private void FixedUpdate()
    {
        //acceleration
        if (Input.GetKey(KeyCode.W))
        {
            // mass *10 = wheel_rad * rpm*torque*car
            //if mass >

            int target_rpm = _engine.max_rev;
            // V = w *r --- w = angular velocity --- w = rps = rpm/60
            //float tecnical_rpm = _rigidbody.velocity.magnitude * 60 * car.tranmision[gear]/ (car.wheel_rad* 2* 3.14f); 
            float tecnical_rpm = _rigidbody.velocity.magnitude * 60 * car.tranmision[gear];
            //3.6 m/s = 1km/h

            if (tecnical_rpm >= 8000 && gear < car.tranmision.Length-1){ gear++;}
            else if (tecnical_rpm < 2500 && gear > 0) { gear--; }

            float force = _engine.torque_curve.Evaluate(rpm) * car.tranmision[gear] * car.wheel_rad;
            float turningForce = _rigidbody.velocity.magnitude;

            _rigidbody.AddForceAtPosition(transform.forward * force * 100, left_rear_wheel.transform.position);
            _rigidbody.AddForceAtPosition(transform.forward * force * 100, right_rear_wheel.transform.position);
            //_rigidbody.AddForceAtPosition(left_front_wheel.transform.forward * 1000, left_front_wheel.transform.position);
            //_rigidbody.AddForceAtPosition(right_front_wheel.transform.forward * 1000, right_front_wheel.transform.position);

            rpm = tecnical_rpm;
            rpm = Mathf.Clamp(rpm, 1000, _engine.max_rev);

        }

        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * 2);
            _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * 2);

            if (_rigidbody.velocity.magnitude < 1)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
        //acceleration 
    }

}
