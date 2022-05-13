using UnityEngine;
using UnityEngine.UI;

public class ToolSlot : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public Image buttonImage;

    public Tool tool;

    public void SetHeldTool()
    {
        if (playerState.GetSelectedItem() != null && playerState.GetSelectedItem().Equals(tool))
        {
            playerState.SetSelectedItem(null);
        }
        else
        {
            playerState.SetSelectedItem(tool);
        }
    }

    private PlayerState playerState;

    private void Awake()
    {
        playerState = PlayerState.Get();
    }

    private void Update()
    {
        PlayerState.ToolInfo info = playerState.GetToolInfo(tool);
        if (info.level == 0)
        {
            buttonImage.color = Color.black;
            buttonText.gameObject.SetActive(false);
            button.interactable = false;
        }
        else
        {
            buttonImage.color = Color.white;
            buttonText.gameObject.SetActive(true);
            buttonText.text = $"LV{info.level}";
            button.interactable = true;
        }
    }
}
