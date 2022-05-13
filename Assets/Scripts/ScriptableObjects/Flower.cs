using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class Flower : TileBase
{
    public string displayName;
    public Sprite flowerSprite;
    public Sprite sproutSprite;
    public Sprite seedSprite;
    public int valueMultiplier;
    public bool canBuySeeds;

    public FlowerItem Item() => new FlowerItem(this);
    public SeedItem SeedItem() => new SeedItem(this);

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        FlowerState flowerstate = tilemap.GetComponent<FlowerState>();
        if (flowerstate == null)
        {
            Debug.LogError($"Failed to display flower at {position}: Tilemap has no associated FlowerState.");
            return;
        }

        FlowerState.GrowthStage stage = flowerstate.GetInfo(position).growthStage;
        switch(stage)
        {
            case FlowerState.GrowthStage.NoFlower:
                Debug.LogError($"Failed to display flower at {position}: Growth stage set to 'No Flower'");
                break;

            case FlowerState.GrowthStage.Seed:
                tileData.sprite = seedSprite;
                break;

            case FlowerState.GrowthStage.Sprout:
                tileData.sprite = sproutSprite;
                break;

            case FlowerState.GrowthStage.Flower:
                tileData.sprite = flowerSprite;
                break;
        }
    }
}
