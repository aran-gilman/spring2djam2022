using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class InventorySlot : MonoBehaviour
{
    public enum ItemType
    {
        Flower,
        Seed
    }

    public enum DisplayText
    {
        Quantity,
        Cost
    }


    public Image image;
    public Text text;
    public Button button;

    public Flower flower;
    public ItemType itemType;

    public void OnClick()
    {
        if (!playerState.GetInventoryInfo(flower).isDiscovered)
        {
            return;
        }

        if (itemType == ItemType.Seed || playerState.mode == PlayerState.Mode.Place)
        {
            playerState.selectedItem = flower.Item();
            playerState.isSeed = itemType == ItemType.Seed;
            return;
        }

        PlayerState.FlowerInfo info = playerState.GetInventoryInfo(flower);
        if (info.flowerCount > 0)
        {
            info.flowerCount -= 1;
            playerState.playerMoney += flower.valueMultiplier * playerState.baseFlowerPrice;
        }
    }

    private PlayerState playerState;

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (playerState == null)
        {
            playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        }

        if (flower == null)
        {
            return;
        }

        PlayerState.FlowerInfo flowerInfo = playerState.GetInventoryInfo(flower);

        image.color = flowerInfo.isDiscovered ? Color.white : Color.black;
        text.gameObject.SetActive(flowerInfo.isDiscovered);

        switch (itemType)
        {
            case ItemType.Flower:
                image.sprite = flower.flowerSprite;
                text.text = $"{flowerInfo.flowerCount}";
                button.interactable = flowerInfo.flowerCount > 0;
                break;

            case ItemType.Seed:
                image.sprite = flower.seedSprite;
                if (flowerInfo.seedCount == 0 && flower.canBuySeeds)
                {
                    text.text = $"{playerState.baseSeedCost * flower.valueMultiplier}G";
                    button.interactable = true;
                }
                else
                {
                    text.text = $"{flowerInfo.seedCount}";
                    button.interactable = flowerInfo.seedCount > 0;
                }
                break;
        }
    }
}
