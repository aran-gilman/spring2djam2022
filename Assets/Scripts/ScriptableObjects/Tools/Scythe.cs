using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Scythe")]
public class Scythe : Tool
{
    public override void Activate(PlayerState playerState)
    {
        PlayerState.ToolInfo toolInfo = playerState.GetToolInfo(this);
        FlowerState flowerState = playerState.flowerState;

        foreach (Vector3Int cell in GetCellsInRange(playerState.cursor.GetSelectedCell(), toolInfo.selectedRange))
        {
            Flower flower = flowerState.GetFlower(cell);
            FlowerState.GrowthStage growthStage = flowerState.GetInfo(cell).growthStage;
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
}
