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

    public InputAction interact;

    private PlayerState playerState;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        interact.performed += ctx => OnInteract();
    }

    private void OnInteract()
    {
        Vector3Int cell = cursor.GetSelectedCell();
        if (flowerTilemap.HasTile(cell))
        {
            RemoveFlower(cell);
        }
        else if (playerState.selectedFlower != null)
        {
            PlaceFlower(cell);
        }
    }

    private void PlaceFlower(Vector3Int cell)
    {
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
        flowerState.SetGrowthStage(cell, playerState.isSeed ? FlowerState.GrowthStage.Sprout : FlowerState.GrowthStage.Flower);
        flowerTilemap.SetTile(cell, playerState.selectedFlower);
        flowerTilemap.RefreshTile(cell);
    }

    private void RemoveFlower(Vector3Int cell)
    {
        FlowerState.GrowthStage stage = flowerState.GetGrowthStage(cell);
        if (stage == FlowerState.GrowthStage.Sprout)
        {
            return;
        }

        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(flowerTilemap.GetTile<Flower>(cell));
        if (stage == FlowerState.GrowthStage.Seed)
        {
            info.seedCount += 1;
        }
        else if (stage == FlowerState.GrowthStage.Flower)
        {
            info.flowerCount += 1;
        }
        info.isDiscovered = true;

        flowerState.SetGrowthStage(cell, FlowerState.GrowthStage.NoFlower);
        flowerTilemap.SetTile(cell, null);
        flowerTilemap.RefreshTile(cell);
    }

    private void OnEnable()
    {
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void Update()
    {
        // Prevent "clicking through" the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            interact.Disable();
        }
        else
        {
            interact.Enable();
        }

        if (playerState.selectedFlower == null)
        {
            selectedFlowerDisplay.sprite = null;
            return;
        }
        selectedFlowerDisplay.sprite = playerState.isSeed ? playerState.selectedFlower.seedSprite : playerState.selectedFlower.flowerSprite;

    }
}
