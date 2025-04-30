public abstract class TileBaseState
{
    public abstract void EnterState(Tile tile);

    public abstract void UpdateState(Tile tile);

    public abstract void OnMouseEnter(Tile tile);

    public abstract void OnMouseExit(Tile tile);

    public abstract void HandleLeftClick(Tile tile);

    public abstract void HandleRightClick(Tile tile);
}
