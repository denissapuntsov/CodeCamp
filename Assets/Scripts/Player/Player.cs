using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IPointerClickHandler
{
    [Header("Level Position")] 
    public Tile activeTile;

    [Header("Item Slots")]
    public GameObject headgear;
    
    [Header("Transforms")]
    [SerializeField] private Transform headGearParent;

    #region Animations

    [Header("Animations")] 
    public Animator animator;
    
    [System.NonSerialized] public int IsIdle = Animator.StringToHash("isIdle");
    [System.NonSerialized] public int IsWalking = Animator.StringToHash("isWalking");
    [System.NonSerialized] public int IsJumping = Animator.StringToHash("isJumping");
    [System.NonSerialized] public int IsInteracting = Animator.StringToHash("isInteracting");

    #endregion
    
    #region Hidden Public References
    /*[System.NonSerialized]*/ public Popup popup;
    [System.NonSerialized] public AIPath aiPath;
    [System.NonSerialized] public Seeker seeker;
    [System.NonSerialized] public float distanceThreshold;
    
    private Collider[] _hitColliders = new Collider[18];
    private List<Tile> _hitTiles;
    
    #endregion
    
    #region Finite State Machine
    
    private PlayerBaseState _currentState;
    public PlayerBaseState CurrentState
    {
        get => _currentState;

        set
        {
            _currentState?.ExitState(player: this);
            _currentState = value;
            _currentState.EnterState(player: this);
        }
    }
    
    // Event-relevant version of setter
    public void SetActiveState(string stateName)
    {
        _currentState?.ExitState(player: this);
        switch (stateName)
        {
            case "Idle":
                _currentState = IdleState;
                break;
            case "Walk":
                _currentState = WalkState;
                break;
            case "Place":
                _currentState = PlaceState;
                break;
            case "Traverse":
                _currentState = TraverseState;
                break;
        }
        _currentState?.EnterState(player: this);
    }
    
    public readonly PlayerIdleState IdleState = new PlayerIdleState();
    public readonly PlayerWalkState WalkState = new PlayerWalkState();
    public readonly PlayerPlaceState PlaceState = new PlayerPlaceState();
    public readonly PlayerTraverseState TraverseState = new PlayerTraverseState();
    
    #endregion

    private void Start()
    {
        popup = FindAnyObjectByType<Popup>();
        _hitTiles = new List<Tile>();
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        CurrentState = IdleState;
    }

    private void Update()
    {
        CurrentState.UpdateState(player:this);
    }

    #region Headgear
    public void PutOnHeadgear(GameObject item)
    {
        TutorialManager.Instance.ClearTutorialByID("wear");
        TutorialManager.Instance.SetActiveTutorial("remove");
        
        // turn off trigger for tiles and collider for pointers
        if (item.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in item.GetComponentsInChildren<Collider>())
            {
                c.enabled = false;
            }
        }

        item.GetComponentInChildren<Light>().enabled = false;
        
        item.transform.SetParent(headGearParent, false);
        //item.GetComponent<Rigidbody>().isKinematic = true;
        ResetTransform(item);
        item.GetComponentInChildren<HeadgearEx>()?.SwitchModels(true);
        headgear = item;
        
        // reset player destination to current position
        aiPath.destination = transform.position;
    }

    public void RemoveHeadgear(Tile tileTarget)
    {
        if (!headgear) return;

        headgear.transform.SetParent(p: null);
        //headgear.GetComponent<Rigidbody>().isKinematic = false;
        ResetTransform(headgear);
        
        if (headgear.GetComponentsInChildren<Collider>() != null)
        {
            foreach (Collider c in headgear.GetComponentsInChildren<Collider>()) c.enabled = true;
        }
        
        headgear.GetComponentInChildren<Light>().enabled = true;
        
        headgear.transform.SetParent(p: tileTarget.parent.transform);
        headgear.transform.localPosition = new Vector3(0, 0.65f, 0);
        headgear.GetComponentInChildren<HeadgearEx>()?.SwitchModels(false);
        headgear = null;
        
        ClearPlacementTiles();
        tileTarget.CurrentState = tileTarget.HoldState;
        
        TutorialManager.Instance.ClearTutorialByID("remove");
        TutorialManager.Instance.SetActiveTutorial("change");
    }
    
    #endregion
    
    #region Traverse
    public void EnterTraversal(GameObject item)
    {
        aiPath.destination = transform.position;
        Traversal traversal = item.GetComponentInChildren<Traversal>();
        traversal.Use();
        aiPath.destination = transform.position;
        CurrentState = TraverseState;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleLeftClick();
        }
    }

    private void OnMouseOver()
    {
        CurrentState.OnMouseOver(this);
    }

    private void OnMouseExit()
    {
        CurrentState.OnMouseExit(this);
    }

    private void HandleLeftClick()
    {
        CurrentState.HandleLeftClick(this);
    }
    
    #endregion
    
    #region PlaceState
    public void CalculatePlacementTiles()
    {
        _hitTiles.Clear();

        //Collider[] hitColliders = new Collider[8];
        int collidersHit = Physics.OverlapSphereNonAlloc(
            position: transform.position, 
            radius: 2.6f, 
            results: _hitColliders);

        for (int i = 0; i < collidersHit; i++)
        {
            Tile tile = _hitColliders[i].GetComponentInChildren<Tile>();
            if (!tile) continue;
            if (!tile.hasPlayer && !tile.currentInteractable && !tile.gameObject.GetComponent<Portal>()) _hitTiles.Add(tile);
        }
        
        foreach (Tile tile in _hitTiles) tile.CurrentState = tile.PlaceState;
    }

    public void ClearPlacementTiles()
    {
        foreach (Tile tile in _hitTiles)
        {
            tile.cross.SetActive(false);
            tile.CurrentState = tile.WalkState;
        }
        CurrentState = IdleState;
    }
    
    #endregion
    
    #region Helpers
    private void ResetTransform(GameObject obj)
    { 
        obj.transform.localPosition = Vector3.zero; 
        obj.transform.localRotation = Quaternion.identity;
    }

    public void ShowPopup(string text)
    {
        popup.UpdateSelection("Player", null);
        popup.SetText(text);
        popup.Appear();
    }
    
    #endregion
}
