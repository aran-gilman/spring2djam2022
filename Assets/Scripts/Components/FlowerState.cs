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

    public List<HybridRule> hybridRules = new List<HybridRule>();

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

            switch (stage)
            {
                case GrowthStage.Seed:

                    growthStages[cell] = GrowthStage.Sprout;
                    tilemap.RefreshTile(cell);
                    break;

                case GrowthStage.Sprout:

                    growthStages[cell] = GrowthStage.Flower;
                    tilemap.RefreshTile(cell);
                    break;

                case GrowthStage.Flower:
                    MaybeCreateSeed(cell);
                    break;
            }
        }
    }

    private static readonly List<Vector3Int> adjacentCellVectors = new List<Vector3Int>()
    {
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right,
        Vector3Int.up,

        Vector3Int.down + Vector3Int.left,
        Vector3Int.down + Vector3Int.right,
        Vector3Int.up + Vector3Int.left,
        Vector3Int.up + Vector3Int.right
    };

    private static IEnumerable<Vector3Int> GetAdjacentCells(Vector3Int cell) => adjacentCellVectors.Select(adj => adj + cell);

    private PlayerState playerState;
    private Dictionary<Vector3Int, GrowthStage> growthStages = new Dictionary<Vector3Int, GrowthStage>();

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    private void MaybeCreateSeed(Vector3Int cell)
    {
        if (Random.value >= playerState.flowerSeedChance)
        {
            return;
        }

        IEnumerable<Vector3Int> adjacentCells = GetAdjacentCells(cell);
        List<Flower> adjacentFlowers = new List<Flower>();
        List<Vector3Int> emptyCells = new List<Vector3Int>();

        foreach (Vector3Int adj in adjacentCells)
        {
            GrowthStage stage = GetGrowthStage(adj);
            if (stage == GrowthStage.Flower)
            {
                adjacentFlowers.Add(tilemap.GetTile<Flower>(adj));
            }
            else if (stage == GrowthStage.NoFlower)
            {
                emptyCells.Add(adj);
            }
        }

        if (adjacentFlowers.Count == 0 || emptyCells.Count == 0)
        {
            return;
        }

        Flower seed = GetSeed(tilemap.GetTile<Flower>(cell), adjacentFlowers.Distinct());
        Vector3Int seedCell = emptyCells[Random.Range(0, emptyCells.Count)];
        SetGrowthStage(seedCell, GrowthStage.Seed);
        tilemap.SetTile(seedCell, seed);
    }

    private Flower GetSeed(Flower flower, IEnumerable<Flower> adjacentFlowers)
    {
        if (Random.value >= playerState.flowerHybridChance)
        {
            if (Random.value < 0.5f)
            {
                return flower;
            }
            return adjacentFlowers.ElementAt(Random.Range(0, adjacentFlowers.Count()));
        }

        IEnumerable<HybridRule> allowedHybrids = hybridRules
            .Where(rule => adjacentFlowers.Where(f => rule.IsAllowed(flower, f))
            .Count() > 0);

        if (allowedHybrids.Count() == 0)
        {
            if (Random.value < 0.5f)
            {
                return flower;
            }
            return adjacentFlowers.ElementAt(Random.Range(0, adjacentFlowers.Count()));
        }

        return allowedHybrids.Select(rule => rule.hybridChild).ElementAt(Random.Range(0, allowedHybrids.Count()));
    }
}
