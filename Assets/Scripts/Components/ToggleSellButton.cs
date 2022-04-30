using UnityEngine;
using UnityEngine.UI;

public class ToggleSellButton : MonoBehaviour
{
    public Image panelBackground;
    public Text buttonText;

    public Sprite placeBackgroundSprite;
    public Sprite sellBackgroundSprite;

    public void OnClick()
    {
        playerState.mode = playerState.mode == PlayerState.Mode.Place ? PlayerState.Mode.Sell : PlayerState.Mode.Place;
        UpdateInventoryPanel();
    }

    private PlayerState playerState;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
        UpdateInventoryPanel();
    }

    private void UpdateInventoryPanel()
    {
        switch(playerState.mode)
        {
            case PlayerState.Mode.Place:
                panelBackground.sprite = placeBackgroundSprite;
                break;

            case PlayerState.Mode.Sell:
                panelBackground.sprite = sellBackgroundSprite;
                break;
        }

        buttonText.text = playerState.mode == PlayerState.Mode.Place ? "Sell" : "Place";
    }
}
