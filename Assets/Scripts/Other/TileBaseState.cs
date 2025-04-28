using UnityEngine;

public abstract class TileBaseState
{
    public abstract void EnterState(Tile tile);

    public abstract void UpdateState(Tile tile);

    public abstract void OnMouseOver(Tile tile);

    public abstract void HandleLeftClick(Tile tile);
}
