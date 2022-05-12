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

    [Serializable]
    public class ToolInfo
    {
        public Tool tool;
        public int level;
        public int currentRange;
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
    public InputAction changeToolRange;

    public int baseSeedCost = 5;
    public int baseFlowerPrice = 10;
    public float flowerSeedChance = 0.25f;
    public float flowerHybridChance = 0.2f;

    public List<FlowerInfo> inventory = new List<FlowerInfo>();
    public List<ToolInfo> tools = new List<ToolInfo>();
    public int playerMoney = 10;
    public Mode mode;
    public IItem selectedItem;

    public AudioClip placeSound;
    public AudioClip removeSound;
    public AudioClip moneySound;

    public FlowerInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);
    public ToolInfo GetToolInfo(Tool tool) => tools.Find(it => it.tool == tool);

    public int GetSeedCost(Flower flower) => flower.valueMultiplier * baseSeedCost;

    private void Awake()
    {
        useItem.performed += ctx => OnUseItem();
        changeToolRange.performed += OnChangeToolRange;
    }

    private void OnEnable()
    {
        useItem.Enable();
        changeToolRange.Enable();
    }

    private void OnDisable()
    {
        useItem.Disable();
        changeToolRange.Disable();
    }

    private void OnUseItem()
    {
        if (selectedItem != null)
        {
            selectedItem.Activate(this);
        }
    }

    private void OnChangeToolRange(InputAction.CallbackContext ctx)
    {
        Tool currentTool = selectedItem as Tool;
        if (currentTool == null)
        {
            return;
        }
        ToolInfo info = GetToolInfo(currentTool);
        if (info == null)
        {
            return;
        }

        float val = ctx.ReadValue<float>();
        if (val > 0)
        {
            info.currentRange += 1;
        }
        else
        {
            info.currentRange -= 1;
        }
        info.currentRange = Mathf.Clamp(info.currentRange, 1, info.level);
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
