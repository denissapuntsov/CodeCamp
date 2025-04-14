using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordInteractionManager : MonoBehaviour
{
    [SerializeField] string activeName;
    [SerializeField] public List<Neighbour> neighbours;
    [SerializeField] private Canvas interactionCanvas;
    [SerializeField] private GridLayoutGroup wordGrid;
    [SerializeField] private GameObject letterButton;

    private List<string> _sameLengthWords;

    private void Start()
    {
        _sameLengthWords = new List<string>();
        FilterNeighbours();
    }
    private void FilterNeighbours()
    {
        neighbours.Clear();

        // check that active word is in dictionary

        if (activeName == String.Empty) return;
        
        if (!WordDictionary.words.Contains(activeName))
        {
            Debug.LogError($"The word {activeName} is invalid. Please check spelling or add word to the Dictionary");
            return;
        }

        DisplayActiveWord();

        // filter all unique words with length different to active word, with the exception of the active word

        foreach (string word in WordDictionary.words)
        {
            if (word.Length == activeName.Length && word != activeName && !_sameLengthWords.Contains(word))
            {
                _sameLengthWords.Add(word);
            }
        }

        // filter all words that are only different from the active word by 1 character

        foreach (string word in _sameLengthWords)
        {
            int differenceCount = 0;
            Tuple<char, int> difference = Tuple.Create('N', 0);

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != activeName[i])
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
        if (activeName == null || !WordDictionary.words.Contains(activeName)) { return; }
        
        // Populate the word grid letter by letter
        ClearActiveWord();

        for (int i = 0; i < activeName.Length; i++)
        {
            var charBox = Instantiate(letterButton, wordGrid.transform, false);
            charBox.GetComponentInChildren<TextMeshProUGUI>().text = activeName[i].ToString();
            charBox.GetComponent<UILetter>().character = activeName[i];
            charBox.GetComponent<UILetter>().index = i;
        }
    }
    public void TryLetter(char letter, int index)
    {
        // create a temporary version of the proposed word

        string wordToTry = "";
        for (int i = 0; i < activeName.Length; i++)
        {
            if (i == index) { wordToTry += letter.ToString(); }
            else
            {
                wordToTry += activeName[i];
            }
        }

        // check if the proposed word exists, and if it does, replace the active word

        foreach (Neighbour word in neighbours)
        {
            if (word.name == wordToTry)
            {
                FindAnyObjectByType<Inventory>().SetLetter(activeName[index]);
                activeName = word.name;
                FilterNeighbours();
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

    public void SetActiveWord(string word)
    {
        interactionCanvas.gameObject.SetActive(true);
        activeName = word;
        DisplayActiveWord();
        FilterNeighbours();
    }
}
