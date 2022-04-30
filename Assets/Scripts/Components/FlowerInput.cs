using System;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public Flower selectedFlower;
    public bool isSeed;

    private void Awake()
    {
        placeFlower.performed += ctx => OnPlaceFlower();
    }

    private void OnPlaceFlower()
    {
        Vector3Int cell = cursor.GetSelectedCell();
        if (!flowerTilemap.HasTile(cell))
        {
            flowerState.SetGrowthStage(cell, isSeed ? FlowerState.GrowthStage.Seed : FlowerState.GrowthStage.Flower);
            flowerTilemap.SetTile(cell, selectedFlower);
            flowerTilemap.RefreshTile(cell);
        }
    }

    private void OnEnable()
    {
        placeFlower.Enable();
        removeFlower.Enable();
    }

    private void OnDisable()
    {
        placeFlower.Disable();
        removeFlower.Disable();
    }

    private void Update()
    {
        if (selectedFlower == null)
        {
            selectedFlowerDisplay.sprite = null;
            return;
        }
        selectedFlowerDisplay.sprite = selectedFlower.flowerSprite;

        // Prevent "clicking through" the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            placeFlower.Disable();
            removeFlower.Disable();
        }
        else
        {
            placeFlower.Enable();
            removeFlower.Enable();
        }
    }
}
