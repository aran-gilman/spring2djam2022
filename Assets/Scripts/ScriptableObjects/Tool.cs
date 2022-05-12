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

    [SerializeField]
    private Sprite sprite;
}
