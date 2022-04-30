using UnityEngine;

[CreateAssetMenu]
public class HybridRule : ScriptableObject
{
    public Flower parentA;
    public Flower parentB;
    public Flower hybridChild;

    public bool IsAllowed(Flower a, Flower b)
    {
        return (a == parentA && b == parentB) || (b == parentA && a == parentB);
    }
}
