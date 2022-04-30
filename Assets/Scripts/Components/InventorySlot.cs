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

    public bool isDiscovered;

    public void OnClick()
    {

    }

    private PlayerState playerState;

    private void OnEnable()
    {
        UpdateDisplay();
    }

#if UNITY_EDITOR
    private void Update()
    {
        UpdateDisplay();
    }
#endif

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

        image.color = isDiscovered ? Color.white : Color.black;
        text.gameObject.SetActive(isDiscovered);

        PlayerState.InventoryInfo flowerInfo = playerState.GetInventoryInfo(flower);
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
