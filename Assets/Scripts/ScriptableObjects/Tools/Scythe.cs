using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Tools/Scythe")]
public class Scythe : Tool
{
    public override void Activate(PlayerState playerState)
    {
        PlayerState.ToolInfo toolInfo = playerState.GetToolInfo(this);
        FlowerState flowerState = playerState.flowerState;

        foreach (Vector3Int cell in GetCellsInRange(playerState.cursor.GetSelectedCell(), toolInfo.level))
        {
            Flower flower = flowerState.flowerTilemap.GetTile<Flower>(cell);
            FlowerState.GrowthStage growthStage = flowerState.GetGrowthStage(cell);
            switch (growthStage)
            {
                case FlowerState.GrowthStage.NoFlower:
                case FlowerState.GrowthStage.Sprout:
                    continue;

                case FlowerState.GrowthStage.Seed:
                    playerState.GetInventoryInfo(flower).seedCount += 1;
                    break;

                case FlowerState.GrowthStage.Flower:
                    playerState.GetInventoryInfo(flower).flowerCount += 1;
                    break;
            }
            flowerState.SetFlower(cell, null, FlowerState.GrowthStage.NoFlower);
        }
    }

    private IEnumerable<Vector3Int> GetCellsInRange(Vector3Int center, int range)
    {
        List<Vector3Int> cells = new List<Vector3Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                cells.Add(center + new Vector3Int(x, y, 0));
            }
        }
        return cells;
    }
}
