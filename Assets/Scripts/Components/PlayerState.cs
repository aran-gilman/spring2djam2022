using System;
using System.Collections.Generic;
using UnityEngine;

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

    public List<FlowerInfo> inventory = new List<FlowerInfo>();
    public int baseSeedCost = 5;
    public int playerMoney = 10;

    public Flower selectedFlower;
    public bool isSeed;

    public FlowerInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);

    public int GetSeedCost(Flower flower) => flower.valueMultiplier * baseSeedCost;
}
