using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    public InputAction rawPos;
    public Grid grid;

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
        Vector2 mpos = rawPos.ReadValue<Vector2>();
        Vector3 worldpos = Camera.main.ScreenToWorldPoint(mpos) + new Vector3(0.5f, 0.5f, 0) ;
        Vector3Int cellpos = grid.WorldToCell(worldpos);
        transform.position = grid.CellToWorld(cellpos);
    }
}
