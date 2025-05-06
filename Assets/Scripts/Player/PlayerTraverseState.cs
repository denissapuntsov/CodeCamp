using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class PlayerTraverseState : PlayerBaseState
{
    public override void EnterState(Player player)
    {
        player.name = "Player (Traversing)";
    }

    public override void UpdateState(Player player)
    {
        return;
    }

    public override void OnMouseOver(Player player)
    {
        return;
    }

    public override void OnMouseExit(Player player)
    {
        return;
    }

    public override void HandleLeftClick(Player player)
    {
        return;
    }

    public override void HandleKeyboardInput(Player player)
    {
        return;
    }

    public override void ExitState(Player player)
    {
        
    }
}
