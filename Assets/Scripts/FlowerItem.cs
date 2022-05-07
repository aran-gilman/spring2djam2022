using UnityEngine;

public class FlowerItem : IItem
{
    public Flower Flower { get; private set; }

    public Sprite Sprite => Flower.flowerSprite;

    public FlowerItem(Flower flower)
    {
        Flower = flower;
    }

    public void Activate(PlayerState playerState)
    {
        Vector3Int cell = playerState.cursor.GetSelectedCell();
        if (!playerState.flowerState.CanPlantFlowers(cell))
        {
            return;
        }

        if (playerState.flowerState.GetGrowthStage(cell) == FlowerState.GrowthStage.NoFlower)
        {
            PlaceFlower(playerState, cell);
        }
        else
        {
            RemoveFlower(playerState, cell);
        }

    }

    private void PlaceFlower(PlayerState playerState, Vector3Int cell)
    {
        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(Flower);
        if (playerState.isSeed)
        {
            if (info.seedCount > 0)
            {
                info.seedCount -= 1;
            }
            else if (Flower.canBuySeeds && playerState.playerMoney >= playerState.GetSeedCost(Flower))
            {
                playerState.playerMoney -= playerState.GetSeedCost(Flower);
            }
            else
            {
                return;
            }
        }
        else if (info.flowerCount > 0)
        {
            info.flowerCount -= 1;
        }
        else
        {
            return;
        }
        playerState.audioSource.PlayOneShot(playerState.placeSound);
        playerState.flowerState.SetFlower(cell, Flower, playerState.isSeed ? FlowerState.GrowthStage.Sprout : FlowerState.GrowthStage.Flower);
    }

    private void RemoveFlower(PlayerState playerState, Vector3Int cell)
    {
        FlowerState.GrowthStage stage = playerState.flowerState.GetGrowthStage(cell);
        if (stage == FlowerState.GrowthStage.Sprout)
        {
            return;
        }

        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(playerState.flowerState.flowerTilemap.GetTile<Flower>(cell));
        if (stage == FlowerState.GrowthStage.Seed)
        {
            info.seedCount += 1;
        }
        else if (stage == FlowerState.GrowthStage.Flower)
        {
            info.flowerCount += 1;
        }
        info.isDiscovered = true;

        playerState.audioSource.PlayOneShot(playerState.removeSound);
        playerState.flowerState.SetFlower(cell, null, FlowerState.GrowthStage.NoFlower);
    }
}
