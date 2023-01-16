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
    private float speed;

    void Start()
    {
        //hp = torque* radiands/sec(not rpm)

        //base dull sinewave sound +
        //explosive sound from nº cylinders * rpm (inside cylinder explision + out exhaust explosion)
        //idle sound almost no sinewave
        //engine brake sounds lower thant engine accelerating
        //high rev sounds higher than low rev

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
                Debug.Log(acc);
            }
            rpm += 3;
            //an explosion is gonna occur every 60/(rpm*no_of_cylinders) seconds
            yield return new WaitForSeconds(1/(rpm/60));
        }
    }
    // cc = 

}
