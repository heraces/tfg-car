using UnityEngine;

public class sus_physics : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private float restLength;
    private float springTravel;
    private float springStiffness;
    private float damperStiffness;
    private float wheelRadius;

    private float minLength;
    private float maxLength;
    private float lastLength;
    private float springLength;
    private float SpringVelocity;
    private float springForce;
    private float damperForce;

    private Vector3 suspensionForce;


    void Start()
    {
        _rigidbody = transform.root.GetComponent<Rigidbody>();

        minLength = springLength - springTravel;
        maxLength = springLength + springTravel;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            lastLength = springLength;
            springLength = hit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, minLength, maxLength);
            SpringVelocity = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce = springStiffness * (restLength - springLength);
            damperForce = damperStiffness * SpringVelocity;

            suspensionForce = (springForce + damperForce) * transform.up;
            _rigidbody.AddForceAtPosition(suspensionForce, transform.position);
        }
    }
}
