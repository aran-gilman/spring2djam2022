using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Watering Can")]
public class WateringCan : Tool
{
    public override void Activate(PlayerState playerState)
    {
        PlayerState.ToolInfo toolInfo = playerState.GetToolInfo(this);

        foreach (Vector3Int cell in GetCellsInRange(playerState.cursor.GetSelectedCell(), toolInfo.selectedRange))
        {
            playerState.flowerState.Water(cell);
        }
    }
}
