using UnityEngine;

public abstract class PlantableItem : IItem
{
    public Flower Flower { get; private set; }
    public abstract Sprite Sprite { get; }

    public PlantableItem(Flower flower)
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

        if (playerState.flowerState.GetInfo(cell).growthStage == FlowerState.GrowthStage.NoFlower)
        {
            PlantObject(playerState, cell);
        }
        else
        {
            RemoveObject(playerState, cell);
        }
    }

    public void UpdateRangeDisplay(SpriteRenderer rangeDisplay)
    {
        rangeDisplay.gameObject.SetActive(false);
    }

    public bool IsTransparentWhenHeld() => true;

    protected abstract void PlantObject(PlayerState playerState, Vector3Int cell);

    private void RemoveObject(PlayerState playerState, Vector3Int cell)
    {
        FlowerState.GrowthStage stage = playerState.flowerState.GetInfo(cell).growthStage;
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
