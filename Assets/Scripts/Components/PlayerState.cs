using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public int baseSeedCost = 5;
    public int baseFlowerPrice = 10;
    public float flowerSeedChance = 0.25f;
    public float flowerHybridChance = 0.2f;
    public float debtMultiplier = 2.0f;
    public int daysBetweenPayments = 7;

    public List<FlowerInfo> inventory = new List<FlowerInfo>();
    public int playerMoney = 10;
    public int amountDue = 25;
    public int daysRemaining = 7;
    public Mode mode;
    public Flower selectedFlower;
    public bool isSeed;

    public FlowerInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);

    public int GetSeedCost(Flower flower) => flower.valueMultiplier * baseSeedCost;

    public void OnSleep()
    {
        daysRemaining--;
        if (daysRemaining < 0)
        {
            playerMoney -= amountDue;
            if (playerMoney < 0)
            {
                SceneManager.LoadScene("TitleScene");
            }
            daysRemaining = daysBetweenPayments;
            amountDue = (int)(amountDue * debtMultiplier);
        }
    }
}
