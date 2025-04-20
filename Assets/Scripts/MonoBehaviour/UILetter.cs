using System;
using UnityEngine;

public class UILetter : MonoBehaviour
{
    public int index;
    public char character;
    Inventory inventory;
    WordInteractionManager wordInteractionManager;

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        wordInteractionManager = FindAnyObjectByType<WordInteractionManager>();
    }

    public void CheckForWord()
    {
        wordInteractionManager.TryLetter(inventory.heldLetter, index);
    }
}
