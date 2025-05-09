using System;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Portal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject selection;
    [SerializeField] private Popup popup;
    [SerializeField] private Tile playerEntryTile;
    [SerializeField] private GameObject inputBlockCanvas;
    private MenuManager _menuManager;
    private Player _player;
    private AIDestinationSetter _destinationSetter;

    private void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _destinationSetter = _player.GetComponent<AIDestinationSetter>();
        _menuManager = MenuManager.Instance;
        popup.SetText("Approach");
    }

    private void Update()
    {
        string popupText = playerEntryTile.hasPlayer ? "Enter" : "Approach";
        popup.SetText(popupText);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_menuManager.activeMenuGroup) return;
        if (_destinationSetter.target) return;

        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                HandleLeftClick();
                break;

            case PointerEventData.InputButton.Right:
                HandleRightClick();
                break;
        }
    }

    private void OnMouseEnter()
    {
        selection.SetActive(true);
        popup.Appear();
    }

    private void OnMouseExit()
    {
        selection.SetActive(false);
        popup.Disappear();
    }

    private void HandleLeftClick()
    {
        selection.SetActive(false);
        popup.Disappear();
        // Approach
        if (!playerEntryTile.hasPlayer)
        {
            playerEntryTile.SetNewPath();
            return;
        }
        
        // Enter
        inputBlockCanvas.SetActive(true);
        _player.CurrentState = _player.PlaceState;
        Debug.Log("Entering the portal");
    }

    private void HandleRightClick()
    {
        throw new NotImplementedException();
    }

}
