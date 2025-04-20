using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordInteractionManager : MonoBehaviour
{
    [Header("Properties of Active Word Interaction")]
    public InteractableFramework lastActiveFramework;
    [SerializeField] private Interaction lastActiveInteraction;
    [SerializeField] private List<Neighbour> neighbours;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject interactionUIGroup;
    [SerializeField] private GridLayoutGroup wordGrid;
    [SerializeField] private GameObject letterButton;

    private string _activeName;
    private List<string> _sameLengthWords;
    private Dictionary _dictionary;
    private MenuManager _menuManager;

    private void Start()
    {
        _sameLengthWords = new List<string>();
        _dictionary = FindAnyObjectByType<Dictionary>();
        _menuManager = FindAnyObjectByType<MenuManager>();
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

        foreach (string word in _dictionary.words)
        {
            if (word.Length == _activeName.Length && word != _activeName && !_sameLengthWords.Contains(word))
            {
                _sameLengthWords.Add(word);
            }
        }

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
        if (_activeName == null || !_dictionary.words.Contains(_activeName)) { return; }
        
        // Populate the word grid letter by letter
        ClearActiveWord();

        for (int i = 0; i < _activeName.Length; i++)
        {
            var charBox = Instantiate(letterButton, wordGrid.transform, false);
            charBox.GetComponentInChildren<TextMeshProUGUI>().text = _activeName[i].ToString();
            charBox.GetComponent<UILetter>().character = _activeName[i];
            charBox.GetComponent<UILetter>().index = i;
        }
    }
    public void TryLetter(char letter, int index)
    {
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

        // check if the proposed word exists, and if it does, replace the active word

        foreach (Neighbour word in neighbours)
        {
            if (word.name == wordToTry)
            {
                FindAnyObjectByType<Inventory>().SetLetter(_activeName[index]);
                _activeName = word.name;
                SetActiveInteraction(_dictionary.GetInteractionByName(_activeName));
                _menuManager.CloseActiveMenu();
                break;
            }
        }
    }
    private void ClearActiveWord()
    {
        foreach (Transform child in wordGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetActiveInteraction(Interaction interaction)
    {
        // honestly not even sure this is elegant enough, but it works?
        _menuManager.SetMenu(interactionUIGroup);
        
        interactionUIGroup.gameObject.SetActive(true);
        lastActiveInteraction = interaction;
        lastActiveFramework.ReplaceInteraction(lastActiveInteraction);
        _activeName = interaction.id;
        DisplayActiveWord();
        FilterNeighbours();
    }
}
