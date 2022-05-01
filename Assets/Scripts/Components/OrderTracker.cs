using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OrderTracker : MonoBehaviour
{
    public class RequestedItem
    {
        public Flower flower;
        public int quantity;
    }

    public GameObject orderSlotPrefab;
    public Button submitButton;
    public List<Flower> easyFlowers = new List<Flower>();
    public List<Flower> mediumFlowers = new List<Flower>();
    public List<Flower> hardFlowers = new List<Flower>();

    public int minFlowerTypes = 2;
    public int maxFlowerTypesEasy = 3;
    public int maxFlowerTypesMedium = 5;
    public int maxFlowerTypesHard = 7;
    public int minQuantity = 1;
    public int maxQuantity = 9;

    public int unlockMediumAfterCompleting = 3;
    public int unlockHardAfterCompleting = 10;

    public int completedOrders = 0;

    public void OnSubmit()
    {
        foreach(RequestedItem item in requestedItems)
        {
            PlayerState.FlowerInfo info = playerState.GetInventoryInfo(item.flower);
            info.flowerCount -= item.quantity;
            playerState.playerMoney += playerState.baseFlowerPrice * info.flower.valueMultiplier * 2;
        }
        completedOrders++;
        requestedItems.Clear();
        if (completedOrders > unlockHardAfterCompleting)
        {
            GenerateNewOrder(Difficulty.Hard);
        }
        else if (completedOrders > unlockMediumAfterCompleting)
        {
            GenerateNewOrder(Difficulty.Medium);
        }
        else
        {
            GenerateNewOrder(Difficulty.Easy);
        }
    }

    private enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    private PlayerState playerState;
    private List<RequestedItem> requestedItems = new List<RequestedItem>();

    private void Start()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();

        for (int i = 0; i < maxFlowerTypesHard; i++)
        {
            GameObject go = Instantiate(orderSlotPrefab, transform);
            go.SetActive(false);
        }

        GenerateNewOrder(Difficulty.Easy);
    }

    private void Update()
    {
        UpdateTextAndButton();
    }

    private int GetMaxFlowerTypes(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return maxFlowerTypesEasy;
            case Difficulty.Medium:
                return maxFlowerTypesMedium;
            case Difficulty.Hard:
                return maxFlowerTypesHard;
        }
        return maxFlowerTypesEasy;
    }

    private void GenerateNewOrder(Difficulty difficulty)
    {
        IEnumerable<Flower> allowedFlowers = GetFlowersForDifficulty(difficulty)
            .Select(f => new KeyValuePair<Flower, float>(f, UnityEngine.Random.value))
            .OrderBy(kv => kv.Value)
            .Select(kv => kv.Key);

        int numberofTypes = Mathf.Min(UnityEngine.Random.Range(minFlowerTypes, GetMaxFlowerTypes(difficulty) + 1), allowedFlowers.Count());
        requestedItems = allowedFlowers
            .Take(numberofTypes)
            .Select(f => new RequestedItem()
            {
                flower = f,
                quantity = UnityEngine.Random.Range(minQuantity, minQuantity + 1)
            })
            .ToList();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform slot = transform.GetChild(i);
            if (i >= numberofTypes)
            {
                slot.gameObject.SetActive(false);
                continue;
            }

            slot.gameObject.SetActive(true);
            slot.GetComponentInChildren<Image>().sprite = requestedItems[i].flower.flowerSprite;
        }

        UpdateTextAndButton();
    }

    private void UpdateTextAndButton()
    {
        bool allItemsCollected = true;
        for (int i = 0; i < requestedItems.Count; i++)
        {
            Text childText = transform.GetChild(i).GetComponentInChildren<Text>();
            PlayerState.FlowerInfo info = playerState.GetInventoryInfo(requestedItems[i].flower);
            childText.text = $"{requestedItems[i].quantity}";
            if (info.flowerCount >= requestedItems[i].quantity)
            {
                childText.color = Color.green;
            }
            else
            {
                childText.color = new Color(50 / 255, 50 / 255, 50 / 255); // Unity's default text color
                allItemsCollected = false;
            }
        }
        submitButton.interactable = allItemsCollected;
    }

    private IEnumerable<Flower> GetFlowersForDifficulty(Difficulty difficulty)
    {
        List<Flower> flowers = new List<Flower>();
        flowers.AddRange(easyFlowers);

        if (difficulty == Difficulty.Medium || difficulty == Difficulty.Hard)
        {
            flowers.AddRange(mediumFlowers);
        }

        if (difficulty == Difficulty.Hard)
        {
            flowers.AddRange(hardFlowers);
        }

        return flowers;
    }
}
