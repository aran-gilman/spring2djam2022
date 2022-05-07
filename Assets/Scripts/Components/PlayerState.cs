using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerState : MonoBehaviour
{
    [Serializable]
    public class FlowerInfo
    {
        public Flower flower;
        public int seedCount;
        public int flowerCount;
        public bool isDiscovered;
    }

    public enum Mode
    {
        Place,
        Sell
    }

    public Cursor cursor;
    public FlowerState flowerState;
    public AudioSource audioSource;
    public SpriteRenderer selectedItemDisplay;

    public InputAction useItem;

    public int baseSeedCost = 5;
    public int baseFlowerPrice = 10;
    public float flowerSeedChance = 0.25f;
    public float flowerHybridChance = 0.2f;

    public List<FlowerInfo> inventory = new List<FlowerInfo>();
    public int playerMoney = 10;
    public Mode mode;
    public IItem selectedItem;

    public AudioClip placeSound;
    public AudioClip removeSound;
    public AudioClip moneySound;

    public FlowerInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);

    public int GetSeedCost(Flower flower) => flower.valueMultiplier * baseSeedCost;

    private void Awake()
    {
        useItem.performed += ctx => OnUseItem();
    }

    private void OnEnable()
    {
        useItem.Enable();
    }

    private void OnDisable()
    {
        useItem.Disable();
    }

    private void OnUseItem()
    {
        if (selectedItem != null)
        {
            selectedItem.Activate(this);
        }
    }
    private void Update()
    {
        // Prevent "clicking through" the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            useItem.Disable();
        }
        else
        {
            useItem.Enable();
        }

        if (selectedItem == null)
        {
            selectedItemDisplay.sprite = null;
            return;
        }
        selectedItemDisplay.sprite = selectedItem.Sprite;

    }
}
