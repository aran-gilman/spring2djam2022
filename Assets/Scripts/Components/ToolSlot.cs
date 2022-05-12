using UnityEngine;

public class ToolSlot : MonoBehaviour
{
    public void SetHeldTool(Tool tool)
    {
        if (playerState.selectedItem == tool)
        {
            playerState.selectedItem = null;
        }
        else
        {
            playerState.selectedItem = tool;
        }
    }

    private PlayerState playerState;

    private void Awake()
    {
        playerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>();
    }
}
