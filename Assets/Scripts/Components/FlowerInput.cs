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

    private PlayerState playerState;

    private void Awake()
    {
        placeFlower.performed += ctx => OnPlaceFlower();
        removeFlower.performed += ctx => OnRemoveFlower();

        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }

    private void OnPlaceFlower()
    {
        if (playerState.selectedFlower == null)
        {
            return;
        }

        Vector3Int cell = cursor.GetSelectedCell();
        if (flowerTilemap.HasTile(cell))
        {
            return;
        }

        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(playerState.selectedFlower);
        if (playerState.isSeed)
        {
            if (info.seedCount > 0)
            {
                info.seedCount -= 1;
            }
            else if (playerState.selectedFlower.canBuySeeds && playerState.playerMoney >= playerState.GetSeedCost(playerState.selectedFlower))
            {
                playerState.playerMoney -= playerState.GetSeedCost(playerState.selectedFlower);
            }
            else
            {
                return;
            }
        }
        else if (info.flowerCount > 0)
        {
            info.flowerCount -= 1;
        }
        else
        {
            return;
        }
        flowerState.SetGrowthStage(cell, playerState.isSeed ? FlowerState.GrowthStage.Seed : FlowerState.GrowthStage.Flower);
        flowerTilemap.SetTile(cell, playerState.selectedFlower);
        flowerTilemap.RefreshTile(cell);
    }

    private void OnRemoveFlower()
    {
        Vector3Int cell = cursor.GetSelectedCell();
        if (!flowerTilemap.HasTile(cell))
        {
            return;
        }
        FlowerState.GrowthStage stage = flowerState.GetGrowthStage(cell);
        if (stage == FlowerState.GrowthStage.Seed)
        {
            playerState.GetInventoryInfo(playerState.selectedFlower).seedCount += 1;
        }
        else if (stage == FlowerState.GrowthStage.Flower)
        {
            playerState.GetInventoryInfo(playerState.selectedFlower).flowerCount += 1;
        }
        flowerState.SetGrowthStage(cell, FlowerState.GrowthStage.NoFlower);
        flowerTilemap.SetTile(cell, null);
        flowerTilemap.RefreshTile(cell);
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

        if (playerState.selectedFlower == null)
        {
            selectedFlowerDisplay.sprite = null;
            return;
        }
        selectedFlowerDisplay.sprite = playerState.isSeed ? playerState.selectedFlower.seedSprite : playerState.selectedFlower.flowerSprite;

    }
}
