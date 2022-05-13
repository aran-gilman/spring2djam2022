using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : ScriptableObject, IItem
{
    [Serializable]
    public class Upgrade
    {
        public int cost;
    }

    public Sprite Sprite => sprite;
    public List<Upgrade> upgrades = new List<Upgrade>();

    public abstract void Activate(PlayerState playerState);

    public bool ShouldShowRange() => true;

    public bool IsTransparent() => false;

    protected IEnumerable<Vector3Int> GetCellsInRange(Vector3Int center, int range)
    {
        List<Vector3Int> cells = new List<Vector3Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                cells.Add(center + new Vector3Int(x, y, 0));
            }
        }
        return cells;
    }

    [SerializeField]
    private Sprite sprite;
}
