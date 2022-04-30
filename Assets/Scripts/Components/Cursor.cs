using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    public InputAction rawPos;
    public Grid grid;

    public Vector3Int GetSelectedCell()
    {
        Vector2 mpos = rawPos.ReadValue<Vector2>();
        Vector3 worldpos = Camera.main.ScreenToWorldPoint(mpos) + new Vector3(0.5f, 0.5f, 0);
        return grid.WorldToCell(worldpos);
    }

    private void OnEnable()
    {
        rawPos.Enable();
    }

    private void OnDisable()
    {
        rawPos.Disable();
    }

    private void Update()
    {
        transform.position = grid.CellToWorld(GetSelectedCell());
    }
}
