using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FlowerState : MonoBehaviour
{
    public enum GrowthStage
    {
        NoFlower,
        Seed,
        Sprout,
        Flower
    }

    public Tilemap tilemap;

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

    public void AdvanceAll()
    {
        List<Vector3Int> cellsToCheck = growthStages.Keys.ToList();
        foreach (Vector3Int cell in cellsToCheck)
        {
            GrowthStage stage = growthStages[cell];

            if (stage == GrowthStage.Seed)
            {
                growthStages[cell] = GrowthStage.Sprout;
                tilemap.RefreshTile(cell);

            }
            else if (stage == GrowthStage.Sprout)
            {
                growthStages[cell] = GrowthStage.Flower;
                tilemap.RefreshTile(cell);
            }
        }
    }

    private Dictionary<Vector3Int, GrowthStage> growthStages = new Dictionary<Vector3Int, GrowthStage>();

}
