using UnityEngine;

public class SeedItem : IItem
{
    public Flower Flower { get; private set; }
    public Sprite Sprite => Flower.seedSprite;

    public SeedItem(Flower flower)
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
            PlaceSeed(playerState, cell);
        }
        else
        {
            RemoveFlower(playerState, cell);
        }
    }

    private void PlaceSeed(PlayerState playerState, Vector3Int cell)
    {
        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(Flower);
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
        playerState.audioSource.PlayOneShot(playerState.placeSound);
        playerState.flowerState.SetFlower(cell, Flower, FlowerState.GrowthStage.Sprout);
    }
    
    // TODO: Figure out a good way to share this code with FlowerItem instead of duplicating it.
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
