using UnityEngine;

public class FlowerItem : PlantableItem
{

    public override Sprite Sprite => Flower.flowerSprite;

    public FlowerItem(Flower flower) : base(flower)
    {
    }

    protected override void PlantObject(PlayerState playerState, Vector3Int cell)
    {
        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(Flower);
        if (info.flowerCount > 0)
        {
            info.flowerCount -= 1;
            playerState.audioSource.PlayOneShot(playerState.placeSound);
            playerState.flowerState.SetFlower(cell, Flower, FlowerState.GrowthStage.Flower);
        }
    }
}
