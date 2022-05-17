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
            if (playerState.flowerState.PutInInventory(cell))
            {
                playerState.audioSource.PlayOneShot(playerState.removeSound);
            }
        }
    }

    public void UpdateRangeDisplay(SpriteRenderer rangeDisplay)
    {
        rangeDisplay.gameObject.SetActive(false);
    }

    public bool IsTransparentWhenHeld() => true;

    protected abstract void PlantObject(PlayerState playerState, Vector3Int cell);
}
