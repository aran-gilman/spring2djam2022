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

    public Flower flower;
    public ItemType itemType;

    public void OnClick()
    {
        if (!playerState.GetInventoryInfo(flower).isDiscovered)
        {
            return;
        }
        playerState.selectedFlower = flower;
        playerState.isSeed = itemType == ItemType.Seed;
    }

    private PlayerState playerState;

    private void OnEnable()
    {
        UpdateDisplay();
    }

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
                break;

            case ItemType.Seed:
                image.sprite = flower.seedSprite;
                if (flowerInfo.seedCount == 0 && flower.canBuySeeds)
                {
                    text.text = $"{playerState.baseSeedCost * flower.valueMultiplier}G";
                }
                else
                {
                    text.text = $"{flowerInfo.seedCount}";
                }
                break;
        }
    }
}
