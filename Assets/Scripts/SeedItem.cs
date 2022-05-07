using UnityEngine;

public class SeedItem : PlantableItem
{
    public override Sprite Sprite => Flower.seedSprite;

    public SeedItem(Flower flower) : base(flower)
    {
    }

    protected override void PlantObject(PlayerState playerState, Vector3Int cell)
    {
        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(Flower);
        if (info.seedCount > 0)
        {
            info.seedCount -= 1;
            playerState.audioSource.PlayOneShot(playerState.placeSound);
        }
        else if (Flower.canBuySeeds && playerState.playerMoney >= playerState.GetSeedCost(Flower))
        {
            playerState.playerMoney -= playerState.GetSeedCost(Flower);
            playerState.audioSource.PlayOneShot(playerState.moneySound);
        }
        else
        {
            return;
        }
        playerState.flowerState.SetFlower(cell, Flower, FlowerState.GrowthStage.Sprout);
    }
}
