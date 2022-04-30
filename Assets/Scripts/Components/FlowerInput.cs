using UnityEngine;
using UnityEngine.InputSystem;

public class FlowerInput : MonoBehaviour
{
    public Cursor cursor;
    public SpriteRenderer selectedFlowerDisplay;

    public InputAction placeFlower;
    public InputAction removeFlower;
    public InputAction sellFlower;

    public void OnEnable()
    {
        placeFlower.Enable();
        removeFlower.Enable();
        sellFlower.Enable();
    }

    public void OnDisable()
    {
        placeFlower.Disable();
        removeFlower.Disable();
        sellFlower.Disable();
    }
}
