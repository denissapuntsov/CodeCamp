using UnityEngine;

public class UILetter : MonoBehaviour
{
    public int index;
    public char character;
    private LetterInventory _letterInventory;
    private WordInteractionManager _wordInteractionManager;

    private void Start()
    {
        _letterInventory = FindAnyObjectByType<LetterInventory>();
        _wordInteractionManager = FindAnyObjectByType<WordInteractionManager>();
    }

    public void CheckForWord()
    {
        _wordInteractionManager.TryLetter(_letterInventory.heldLetter, index);
    }
}
