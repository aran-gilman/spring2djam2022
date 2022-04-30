using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class FlowerState : MonoBehaviour
{
    public enum GrowthStage
    {
        NoFlower,
        Sprout,
        Grown
    }

    public GrowthStage GetGrowthStage(Vector3Int cell) => growthStages[cell.x, cell.y];
    public void SetGrowthStage(Vector3Int cell, GrowthStage stage) => growthStages[cell.x, cell.y] = stage;

    private GrowthStage[,] growthStages;
    private Tilemap tilemap;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        growthStages = new GrowthStage[tilemap.size.x, tilemap.size.y];
    }
}
