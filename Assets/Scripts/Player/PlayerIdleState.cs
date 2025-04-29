using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.gameObject.name = player.headgear ? $"Player (Idle) ({player.headgear.name})" : "Player (Idle)";
    }

    public override void UpdateState(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseOver(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseExit(Player player)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleLeftClick(Player player)
    {
        throw new System.NotImplementedException();
    }
}
