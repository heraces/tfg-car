using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    [SerializeField]
    private engine_diagram _engine;
    private float powerfactor = 1;
    private short rpm = 0;
    private float cc_cylinder;
    private short torque;
    private short hp;

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
            rpm = 500;
            cc_cylinder = _engine.cc / _engine.cylinder_number;
            StartCoroutine(revolution());
        }
    }

    private IEnumerator revolution() {

        while (true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //formula to accelerate
                powerfactor = _engine.max_rev / rpm;
                Debug.Log(powerfactor);
            }
            rpm= (short)(powerfactor * rpm);
            Debug.Log("car has" + rpm + "rpm");
            //an explosion is gonna occur every 60/(rpm*no_of_cylinders) seconds
            yield return new WaitForSeconds(60 / (rpm * _engine.cylinder_number));
        }
    }
    // cc = 

}
