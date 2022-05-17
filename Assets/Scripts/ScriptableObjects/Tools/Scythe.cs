using UnityEngine;

[CreateAssetMenu(menuName = "Tools/Scythe")]
public class Scythe : Tool
{
    public override void Activate(PlayerState playerState)
    {
        PlayerState.ToolInfo toolInfo = playerState.GetToolInfo(this);
        FlowerState flowerState = playerState.flowerState;

        foreach (Vector3Int cell in GetCellsInRange(playerState.cursor.GetSelectedCell(), toolInfo.selectedRange))
        {
            flowerState.PutInInventory(cell);
        }
        playerState.audioSource.PlayOneShot(activationSound);
    }
}
