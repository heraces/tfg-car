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
    public float rpm = 0;
    private float cc_cylinder;
    private short torque;
    private short hp;
    private float speed = 5;
    private AudioSource s;

    //to be soterd on a tyre scriptable object
    private float tire_grip = .8f;

    private float turn_angle = 50;
    private Rigidbody _rigidbody;
    private float window_size;
    private float backSpeed = 500f;

    private float ackerman_angle_left;
    private float ackerman_angle_right;

    private sus_physics right_front_wheel;
    private sus_physics left_front_wheel;
    private sus_physics left_rear_wheel;
    private sus_physics right_rear_wheel;

    private short gear = 1;

    private crosPoint lastCrosPoint = null;
    private crosPoint thisCrosPoint;



    [SerializeField] private GameObject buttonContrarreloj;
    [SerializeField] private GameObject boxesPlace;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject tick;
    [SerializeField] private GameObject contrarrelojArrow;
    [SerializeField] private endContrarreloj contrarrelojEndPanel;


    [SerializeField] private GameObject needle;

    [SerializeField]
    private AnimationCurve grip;

    private bool contrarreloj = false;
    private bool currently_running = false;
    private int raceIndex = 0;
    private crosPoint[] controlPoints;
    private countDown countDown;

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
        buttonContrarreloj.SetActive(false);
        countDown = FindObjectOfType<countDown>();
        contrarrelojArrow.SetActive(false);


        //soudn hz = rpm/60
        window_size = Screen.width/2;

        _rigidbody = GetComponent<Rigidbody>();
        foreach(sus_physics item in GetComponentsInChildren<sus_physics>())
        {
            item.setSuspension(suspension);
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
        if (currently_running)
        {
            Vector3 thaPosition = new Vector3(controlPoints[raceIndex].transform.position.x, transform.position.y, controlPoints[raceIndex].transform.position.z);
            contrarrelojArrow.transform.LookAt(thaPosition);
        }
        else if (contrarreloj && Input.GetKeyDown("space"))
        {
            contrarreloj = false;
            runbb();
            buttonContrarreloj.SetActive(false);
            startContrarreloj(thisCrosPoint.findPath(lastCrosPoint.GetComponent<crosPoint>()));

        }
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
        //transform.Rotate(new Vector3(0, transform.rotation.y + steering, 0) * Time.deltaTime);
        //ackerman steering 
        //actual steering 

        if(tag == "Player")
        {
            float rpmPor1000 = rpm / 1000 * 40;
            needle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 30 - rpmPor1000));
            
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

        //turning
        /*
        Vector3 steeringDir = left_front_wheel.transform.right;
        float steeringVel = Vector3.Dot(steeringDir, _rigidbody.GetPointVelocity(transform.position));
        Debug.Log(steeringVel + "wwww" + _rigidbody.GetPointVelocity(transform.position));
        float desiredVelchange = -steeringVel * grip.Evaluate(_rigidbody.velocity.magnitude);
        float acc = desiredVelchange / Time.fixedDeltaTime;

        Vector3 steeringDirR = right_front_wheel.transform.right;
        float steeringVelR = Vector3.Dot(steeringDirR, _rigidbody.GetPointVelocity(transform.position));
        float desiredVelchangeR = -steeringVelR * grip.Evaluate(_rigidbody.velocity.magnitude);
        float accR = desiredVelchangeR / Time.fixedDeltaTime;


        _rigidbody.AddForceAtPosition(steeringDir * acc * _rigidbody.mass/4, left_front_wheel.transform.position);
        _rigidbody.AddForceAtPosition(steeringDirR * accR * _rigidbody.mass / 4, right_front_wheel.transform.position);

    */
        //acceleration


        //tracking rpm

        // V = w *r --- w = angular velocity --- w = rps = rpm/60
        //float tecnical_rpm = _rigidbody.velocity.magnitude * 60 * car.tranmision[gear]/ (car.wheel_rad* 2* 3.14f); 
        float tecnical_rpm = _rigidbody.velocity.magnitude * 60 * car.tranmision[gear];
        //3.6 m/s = 1km/h

        if (tecnical_rpm >= 8000 && gear < car.tranmision.Length - 1) { gear++; }
        else if (tecnical_rpm < 2500 && gear > 0) { gear--; }

        rpm = tecnical_rpm;
        rpm = Mathf.Clamp(rpm, 1000, _engine.max_rev);

        if (Input.GetKey(KeyCode.W))
        {
            float force = _engine.torque_curve.Evaluate(rpm) * car.tranmision[gear] * car.wheel_rad;

            _rigidbody.AddForceAtPosition(left_rear_wheel.transform.forward * force / Time.fixedDeltaTime, left_rear_wheel.transform.position);
            _rigidbody.AddForceAtPosition(right_rear_wheel.transform.forward * force /Time.fixedDeltaTime, right_rear_wheel.transform.position);

        }

        if (Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, Vector3.zero, Time.deltaTime * 2);
            _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, Vector3.zero, Time.deltaTime * 2);

            if (_rigidbody.velocity.magnitude < 1)
            {
                _rigidbody.AddForceAtPosition(transform.forward * -backSpeed * 100, left_rear_wheel.transform.position);
                _rigidbody.AddForceAtPosition(transform.forward * -backSpeed * 100, right_rear_wheel.transform.position);
            }
        }
        //acceleration 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<crosPoint>())
        {
            thisCrosPoint = other.gameObject.GetComponent<crosPoint>();
            if (!currently_running) {

                if (lastCrosPoint)
                {
                    contrarreloj = true; 
                    buttonContrarreloj.SetActive(true);
                }
            }
            else
            {
                if (thisCrosPoint.isTarget)
                {
                    raceIndex++;
                    setNextControl();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<crosPoint>())
        {
            buttonContrarreloj.SetActive(false);
            contrarreloj = false;
            lastCrosPoint = thisCrosPoint;
            //start contrarreloj disapear
        }
    }
    private void startContrarreloj(crosPoint[] controlPoints)
    {
        this.controlPoints = controlPoints;
        raceIndex = 0;
        for(int i = 0; i< controlPoints.Length; i++)
        {
            GameObject slot = Instantiate(box);
            slot.transform.SetParent(boxesPlace.transform);
            slot.transform.localScale = new Vector3(1,1,1);
        }
        setNextControl();
    }

    private void setNextControl()
    {
        if (raceIndex >= controlPoints.Length) {

            controlPoints[raceIndex - 1].LightsOff(); 

            dontRun(); 
        }
        else
        {
            if (raceIndex > 0)
            {
                countDown.AddTime();
                GameObject green = Instantiate(tick);
                green.transform.SetParent(boxesPlace.transform.GetChild(raceIndex).transform);
                green.transform.localPosition = new Vector3(-100, 25, 0);
                green.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
                controlPoints[raceIndex - 1].LightsOff();
            }
            controlPoints[raceIndex].LightsOn();
        }
    }
    private void runbb()
    {
        currently_running = true;
        boxesPlace.SetActive(true);
        contrarrelojArrow.SetActive(true);
        int pp = boxesPlace.transform.childCount;
        foreach (Transform child in boxesPlace.GetComponentsInChildren<Transform>())
        {
            if(child.parent == boxesPlace.transform)  Destroy(child.gameObject);
        }
        countDown.startCounting();
    }

    public void dontRun()
    {
        if (controlPoints != null)
        {
            foreach (crosPoint item in controlPoints)
            {
                item.LightsOff();
            }

            contrarrelojEndPanel.callMe(10f,5,5);

        }
        contrarrelojArrow.SetActive(false);
        currently_running = false;
        boxesPlace.SetActive(false);
    }
}
