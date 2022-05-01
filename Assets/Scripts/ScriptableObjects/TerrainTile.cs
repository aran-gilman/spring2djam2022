using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TerrainTile : TileBase
{
    public Sprite[] sprites;
    public bool canPlantFlowers;
    public int randomSeed;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        System.Random rng = new System.Random(randomSeed + position.x * tilemap.size.x + position.y);
        tileData.sprite = sprites[rng.Next(sprites.Length)];
    }
}
