using UnityEngine;

[CreateAssetMenu(fileName = "New suspension", menuName = "Add suspension")]
public class suspension_stats : ScriptableObject
{
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;
}
