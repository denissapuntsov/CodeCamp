using UnityEngine;

public abstract class PlayerBaseState
{
    public abstract void EnterState(Player player);

    public abstract void UpdateState(Player player);

    public abstract void OnMouseOver(Player player);

    public abstract void OnMouseExit(Player player);
    
    public abstract void HandleLeftClick(Player player);

    public abstract void HandleKeyboardInput(Player player);
}
