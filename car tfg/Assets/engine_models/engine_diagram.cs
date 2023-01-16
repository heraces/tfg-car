using UnityEngine;

[CreateAssetMenu(fileName ="New engine", menuName ="Add engine")]
public class engine_diagram : ScriptableObject
{
    public bool gas; // 0 gas, 1 diesel
    public short cc;
    public short cylinder_number;
    public short compress_ratio;
    public short shape; //angle of the engine, 0 means in line engine

    public bool has_turbo;
    public short low_rev_turbo;
    public short high_rev_turbo;

    public short max_rev;
}
