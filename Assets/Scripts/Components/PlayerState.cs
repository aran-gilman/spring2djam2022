using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Serializable]
    public class InventoryInfo
    {
        public Flower flower;
        public int seedCount;
        public int flowerCount;
    }

    public List<InventoryInfo> inventory = new List<InventoryInfo>();
    public int baseSeedCost = 5;
    public int playerMoney = 10;

    public InventoryInfo GetInventoryInfo(Flower flower) => inventory.Find(it => it.flower == flower);
}
