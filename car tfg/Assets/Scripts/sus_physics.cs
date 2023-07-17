using UnityEngine;

public class sus_physics : MonoBehaviour
{
    public enum wheel_place {FrontLeft, FrontRight, BackLeft, BackRight};
    public wheel_place wheel_Place;
    public suspension_stats sus;

    //referencia al rigidbody del coche para aplicar las fuerzas de la suspension
    private Rigidbody _rigidbody;

    //variables intrinsecas de la suspension 
    private float restLength; //distancia estandar de la suspension
    private float springTravel; //la distancia que puede moverse la suspension
    private float springStiffness; //dureza de la suspension
    private float damperStiffness; //damp
    private float wheelRadius;

    //variables para calcular fuerzas
    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float springVelocity;
    private float springForce;
    private float damperForce;

    //vector de fuerza que devolveremos
    private Vector3 suspensionForce;



    void Start()
    {
        setStats();
        _rigidbody = transform.root.GetComponent<Rigidbody>();
        springLength = restLength;
        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

   
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * springVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;
            _rigidbody.AddForceAtPosition(suspensionForce, hit.point);
        }
    }

    private void setStats()
    {
        restLength = sus.restLength;
        springTravel = sus.springTravel;
        springStiffness = sus.springStiffness;
        damperStiffness = sus.damperStiffness;
        wheelRadius = .33f;
    }

    public void setSuspension(suspension_stats sus_stats)
    {
        sus = sus_stats;
        setStats();
    }
    public void ackerman_angle(float angle)
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + angle, transform.localRotation.z);
    }
}
