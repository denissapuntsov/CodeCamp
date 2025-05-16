using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class WordInteractionManager : MonoBehaviour
{
    [Header("Properties of Active Word Interaction")]
    public InteractableFramework lastActiveFramework;
    [SerializeField] private InteractionData lastActiveInteractionData;
    [SerializeField] private List<Neighbour> neighbours;
    public Tile currentTile;
    
    [Header("UI Elements")]
    [SerializeField] private Menu interactionUIGroup;
    [SerializeField] private GridLayoutGroup wordGrid;
    [SerializeField] private GameObject letterButton, inventoryLetter;
    //public GameObject usePopupPrefab, approachPopupPrefab;

    [Header("Animation Controls")] 
    [SerializeField] private float positionAnimationDuration = 0.3f;
    [SerializeField] private float scaleAnimationDuration = 0.2f;
    [SerializeField] ParticleSystem smokeParticlePrefab;
    
    private string _activeName;
    private List<string> _sameLengthWords;
    private Dictionary _dictionary;
    private MenuManager _menuManager;
    private LetterInventory _letterInventory;
    private Vector3 _inventoryLetterDefaultPosition;
    private Vector3 _defaultLetterScale;
    private bool _isTryingLetter = false;
    private Player _player;

    public static WordInteractionManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _sameLengthWords = new List<string>();
        _dictionary = FindAnyObjectByType<Dictionary>();
        _menuManager = MenuManager.instance;
        _letterInventory = FindAnyObjectByType<LetterInventory>();
        _inventoryLetterDefaultPosition = inventoryLetter.transform.localPosition;
        _defaultLetterScale = inventoryLetter.transform.localScale;
        _player = FindAnyObjectByType<Player>();
    }
    private void FilterNeighbours()
    {
        neighbours.Clear();

        // check that active word is in dictionary

        if (_activeName == String.Empty) return;
        
        if (!_dictionary.GetInteractionByName(_activeName))
        {
            Debug.LogError($"No Interaction with name {_activeName} found. Please check spelling or add Interaction to the Dictionary");
            return;
        }

        DisplayActiveWord();

        // filter all unique words with length different to active word, except the active word

        _sameLengthWords = _dictionary.Words.Where(word => word.Length == _activeName.Length && word != _activeName).ToList();

        // filter all words that are only different from the active word by 1 character

        foreach (string word in _sameLengthWords)
        {
            int differenceCount = 0;
            Tuple<char, int> difference = Tuple.Create(' ', 0);

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != _activeName[i])
                {
                    differenceCount++;
                    difference = Tuple.Create(word[i], i);
                }
            }

            if (differenceCount == 1)
            {
                var newNeighbour = new Neighbour()
                {
                    name = word,
                    differenceChar = difference.Item1,
                    differenceIndex = difference.Item2
                };

                neighbours.Add(newNeighbour);
            }
        }
    }
    private void DisplayActiveWord()
    {
        if (_activeName == null || !_dictionary.Words.Contains(_activeName)) return;
        
        // Populate the word grid letter by letter
        ClearGrid();

        for (int i = 0; i < _activeName.Length; i++)
        {
            var charBox = Instantiate(letterButton, wordGrid.transform, false);
            charBox.GetComponentInChildren<TextMeshProUGUI>().text = _activeName[i].ToString().ToUpper();
            charBox.GetComponent<UILetter>().character = _activeName[i];
            charBox.GetComponent<UILetter>().index = i;
        }

        inventoryLetter.transform.localPosition = _inventoryLetterDefaultPosition;
    }
    public void TryLetter(char letter, int index)
    {
        if (_isTryingLetter) return;
        _isTryingLetter = true;
        
        // create a temporary version of the proposed word
        string wordToTry = "";
        for (int i = 0; i < _activeName.Length; i++)
        {
            if (i == index) { wordToTry += letter.ToString(); }
            else
            {
                wordToTry += _activeName[i];
            }
        }

        // sequence and play animation of rotating letters + either letters going back or snapping in place
        Sequence letterChangeSequence = DOTween.Sequence();
        
        GameObject currentLetter = wordGrid.GetComponentsInChildren<UILetter>().ToList().Find(match:uiLetter => uiLetter.index == index).gameObject;
        Vector3 currentLetterDefaultPosition = currentLetter.transform.localPosition;
        
        // scale subsequence
        Sequence scaleSubsequence = DOTween.Sequence();
        scaleSubsequence.Join(currentLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), scaleAnimationDuration));
        scaleSubsequence.Join(inventoryLetter.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), scaleAnimationDuration));
        letterChangeSequence.Append(scaleSubsequence);
        
        // position subsequence
        Sequence positionSubsequence = DOTween.Sequence();
        positionSubsequence.Join(currentLetter.transform.DOLocalMove(_inventoryLetterDefaultPosition, positionAnimationDuration));
        positionSubsequence.Join(inventoryLetter.gameObject.transform.DOLocalMove(currentLetterDefaultPosition, positionAnimationDuration));
        letterChangeSequence.Append(positionSubsequence);
        
        // reverse scale subsequence
        Sequence scaleSubsequenceReverse = DOTween.Sequence();
        scaleSubsequenceReverse.Join(currentLetter.transform.DOScale(_defaultLetterScale, scaleAnimationDuration));
        scaleSubsequenceReverse.Join(inventoryLetter.gameObject.transform.DOScale(_defaultLetterScale, scaleAnimationDuration));
        letterChangeSequence.Append(scaleSubsequenceReverse);
        
        letterChangeSequence.Play().OnComplete(() =>
        {
            // check if the proposed word exists, and if it does, replace the active word
            bool isWordFound = neighbours.Exists(match: word => word.name == wordToTry);
            
            if (isWordFound)
            {
                // SUCCESS
                Sequence successSequence = DOTween.Sequence();
                
                // make all the letters except for the switched one pop a little bit
                Sequence popSubsequence = DOTween.Sequence();
                foreach (Image child in wordGrid.GetComponentsInChildren<Image>())
                {
                    popSubsequence.Join(child.transform.DOShakeScale(strength:new Vector3(0.1f, 0.1f, 0.1f), duration:1f));
                }
                successSequence.Append(popSubsequence);
                
                successSequence.Play().OnComplete(() =>
                {
                    _letterInventory.SetLetter(_activeName[index]);
                    
                    _activeName = wordToTry;
                    SetActiveInteraction(_dictionary.GetInteractionByName(_activeName));
                    Instantiate(smokeParticlePrefab, lastActiveFramework.transform, worldPositionStays:false);
                    _isTryingLetter = false;
                    _menuManager.CloseActiveMenu();
                    TutorialManager.Instance.ClearTutorialByID("change");
                    TutorialManager.Instance.SetActiveTutorial("changeBack", timeToDisappear: 5f);
                });
            }
            else
            {
                // FAIL
                Sequence failSequence = DOTween.Sequence();
                
                // shake the inventory letter
                failSequence.Append(inventoryLetter.transform.DOShakePosition(duration:0.5f, strength:new Vector3(10f, 0f)));
                
                // reverse position subsequence
                Sequence positionSubsequenceReverse = DOTween.Sequence();
                
                positionSubsequenceReverse.Join(currentLetter.transform.DOLocalMove(currentLetterDefaultPosition, positionAnimationDuration));
                positionSubsequenceReverse.Join(inventoryLetter.gameObject.transform.DOLocalMove(_inventoryLetterDefaultPosition, positionAnimationDuration));
                failSequence.Append(positionSubsequenceReverse);
                
                failSequence.Play().OnComplete(() => _isTryingLetter = false);
            }
        });
    }
    private void ClearGrid()
    {
        var gridChildren =
            from child in wordGrid.transform.GetComponentsInChildren<UILetter>()
            where !child.gameObject.CompareTag("InventoryLetter")
            select child;
        
        foreach (UILetter child in gridChildren)
        {
            Destroy(child.gameObject);
        }
    }
    
    private void SetActiveInteraction(InteractionData interactionData)
    {
        _player.animator.SetBool(_player.IsInteracting, true);
        _menuManager.OpenMenu(interactionUIGroup);
        
        if (lastActiveInteractionData != interactionData)
        {
            lastActiveInteractionData = interactionData;
            currentTile.currentInteractable.ReplaceInteraction(lastActiveInteractionData);
            _activeName = interactionData.id.ToLower();
            currentTile.GetComponent<ItemTrigger>()?.CheckForItem(interactionData);
        }
        FilterNeighbours();
    }
    
    public void SetActiveInteraction(Tile tile)
    {
        currentTile = tile;
        var interactionData = tile.currentInteractable.activeInteractionData;
        
        _player.animator.SetBool(_player.IsInteracting, true);
        _menuManager.OpenMenu(interactionUIGroup);
        
        interactionUIGroup.gameObject.SetActive(true);
        if (lastActiveInteractionData != interactionData)
        {
            lastActiveInteractionData = interactionData;
            _activeName = interactionData.id.ToLower();
        }
        FilterNeighbours();
    }
}
