using UnityEngine;

[CreateAssetMenu(fileName = "New car", menuName = "Add car")]
public class Car_model : ScriptableObject
{
    public Object car;
    public float[] tranmision; // ex: [3, 2, 1.5, 1, 0.5] 
    public float wheel_rad;
    public float weight;
}
