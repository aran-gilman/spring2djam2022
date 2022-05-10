using UnityEngine;

public abstract class Tool : ScriptableObject, IItem
{
    public Sprite Sprite => sprite;

    public abstract void Activate(PlayerState playerState);

    [SerializeField]
    private Sprite sprite;
}
