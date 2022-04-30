using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class FlowerInput : MonoBehaviour
{
    public Cursor cursor;
    public SpriteRenderer selectedFlowerDisplay;
    public Tilemap flowerTilemap;
    public FlowerState flowerState;

    public InputAction placeFlower;
    public InputAction removeFlower;
    public InputAction sellFlower;

    public Flower selectedFlower;

    private void Awake()
    {
        placeFlower.performed += ctx => OnPlaceFlower();
    }

    private void OnPlaceFlower()
    {
        Vector3Int cell = cursor.GetSelectedCell();
        if (!flowerTilemap.HasTile(cell))
        {
            flowerState.SetGrowthStage(cell, FlowerState.GrowthStage.Sprout);
            flowerTilemap.SetTile(cell, selectedFlower);
            flowerTilemap.RefreshTile(cell);
        }
    }

    private void OnEnable()
    {
        placeFlower.Enable();
        removeFlower.Enable();
        sellFlower.Enable();
    }

    private void OnDisable()
    {
        placeFlower.Disable();
        removeFlower.Disable();
        sellFlower.Disable();
    }

    private void Update()
    {
        if (selectedFlower == null)
        {
            selectedFlowerDisplay.sprite = null;
            return;
        }
        selectedFlowerDisplay.sprite = selectedFlower.flowerSprite;
    }
}
