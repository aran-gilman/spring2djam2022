using System.Collections.Generic;
using UnityEngine;

public class FlowerState : MonoBehaviour
{
    public enum GrowthStage
    {
        NoFlower,
        Sprout,
        Grown
    }

    public GrowthStage GetGrowthStage(Vector3Int cell)
    {
        if (!growthStages.ContainsKey(cell))
        {
            growthStages.Add(cell, GrowthStage.NoFlower);
        }
        return growthStages[cell];
    }
    public void SetGrowthStage(Vector3Int cell, GrowthStage stage)
    {
        if (growthStages.ContainsKey(cell))
        {
            growthStages[cell] = stage;
        }
        else
        {
            growthStages.Add(cell, stage);
        }
    }

    private Dictionary<Vector3Int, GrowthStage> growthStages = new Dictionary<Vector3Int, GrowthStage>();

}
