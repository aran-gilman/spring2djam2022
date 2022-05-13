using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public Button button;
    public Text buttonText;
    public Tool tool;

    public void UpgradeTool()
    {
        PlayerState.ToolInfo info = playerState.GetToolInfo(tool);
        if (info.isOwned)
        {
            playerState.playerMoney -= tool.upgrades[info.level].cost;
            info.level += 1;
        }
        {
            playerState.playerMoney -= tool.purchaseCost;
            info.isOwned = true;
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
        if (info.level >= tool.upgrades.Count)
        {
            button.interactable = false;
            buttonText.text = "MAX";
        }
        else
        {
            int upgradeCost = info.isOwned ? tool.upgrades[info.level].cost : tool.purchaseCost;
            button.interactable = playerState.playerMoney >= upgradeCost;
            buttonText.text = $"{upgradeCost}G";
        }
    }
}
