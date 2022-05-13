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
        public bool isOwned;
        public int level;
        public int currentRange;
    }

    public enum Mode
    {
        Place,
        Sell
    }

    public Cursor cursor;
    public SpriteRenderer rangeDisplay;
    public FlowerState flowerState;
    public AudioSource audioSource;
    public SpriteRenderer selectedItemDisplay;

    public InputActionAsset inputActions;

    public int baseSeedCost = 5;
    public int baseFlowerPrice = 10;
    public float flowerSeedChance = 0.25f;
    public float flowerHybridChance = 0.2f;

    public List<FlowerInfo> inventory = new List<FlowerInfo>();
    public List<ToolInfo> tools = new List<ToolInfo>();
    public int playerMoney = 10;
    public Mode mode;

    public AudioClip placeSound;
    public AudioClip removeSound;
    public AudioClip moneySound;

    public Color transparentHeldItemColor;

    public static PlayerState Get() => GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();

    public FlowerInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);
    public ToolInfo GetToolInfo(Tool tool) => tools.Find(it => it.tool == tool);

    public int GetSeedCost(Flower flower) => flower.valueMultiplier * baseSeedCost;

    public IItem GetSelectedItem() => selectedItem;

    // Use a method instead of a property to better convey that there is non-trivial logic involved
    public void SetSelectedItem(IItem newItem)
    {
        selectedItem = newItem;

        if (selectedItem == null)
        {
            selectedItemDisplay.sprite = null;
            rangeDisplay.gameObject.SetActive(false);
            return;
        }

        selectedItemDisplay.sprite = selectedItem.Sprite;
        if (selectedItem.IsTransparent())
        {
            selectedItemDisplay.color = transparentHeldItemColor;
        }
        else
        {
            selectedItemDisplay.color = Color.white;
        }
        rangeDisplay.gameObject.SetActive(selectedItem.ShouldShowRange());
    }

    private IItem selectedItem;

    private void Awake()
    {
        inputActions.FindAction("UseItem").performed += ctx => OnUseItem();
        inputActions.FindAction("AdjustRange").performed += OnChangeToolRange;
        inputActions.FindAction("DeselectHeldItem").performed += ctx => SetSelectedItem(null);
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
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
        int diameter = info.currentRange * 2 + 1;
        rangeDisplay.size = new Vector2(diameter, diameter);
    }

    private void Update()
    {
        // Prevent "clicking through" the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            inputActions.FindActionMap("GameWorld").Disable();
        }
        else
        {
            inputActions.FindActionMap("GameWorld").Enable();
        }
    }
}
