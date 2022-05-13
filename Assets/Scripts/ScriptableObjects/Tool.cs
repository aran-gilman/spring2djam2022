using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : ScriptableObject, IItem
{
    [Serializable]
    public class Upgrade
    {
        public int cost;
        public int maxEffectRange;
    }

    public Sprite Sprite => sprite;
    public int purchaseCost;
    public int initialEffectRange = 0;
    public List<Upgrade> upgrades = new List<Upgrade>();

    public abstract void Activate(PlayerState playerState);

    public bool IsTransparentWhenHeld() => false;

    public void UpdateRangeDisplay(SpriteRenderer rangeDisplay)
    {
        PlayerState.ToolInfo info = PlayerState.Get().GetToolInfo(this);
        info.selectedRange = Mathf.Clamp(info.selectedRange, initialEffectRange, GetMaxEffectRange(info.level));

        if (info.selectedRange > 0)
        {
            int diameter = info.selectedRange * 2 + 1;
            rangeDisplay.size = new Vector2(diameter, diameter);
            rangeDisplay.gameObject.SetActive(true);
        }
        else
        {
            rangeDisplay.gameObject.SetActive(false);
        }
    }

    public int GetMaxEffectRange(int level)
    {
        if (level <= 0)
        {
            return initialEffectRange;
        }
        else if (level < upgrades.Count)
        {
            return upgrades[level - 1].maxEffectRange;
        }
        return upgrades[upgrades.Count - 1].maxEffectRange;
    }

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
