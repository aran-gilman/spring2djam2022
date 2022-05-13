using UnityEngine;

public interface IItem
{
    Sprite Sprite { get; }

    bool IsTransparentWhenHeld();

    void Activate(PlayerState playerState);

    void UpdateRangeDisplay(SpriteRenderer rangeDisplay);

}
