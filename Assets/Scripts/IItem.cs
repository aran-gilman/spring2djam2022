using UnityEngine;

public interface IItem
{
    Sprite Sprite { get; }

    void Activate(PlayerState playerState);
}
