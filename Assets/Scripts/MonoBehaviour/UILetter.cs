using UnityEngine;

public class UILetter : MonoBehaviour
{
    public int index;
    public char character;
    private Inventory _inventory;
    private WordInteractionManager _wordInteractionManager;

    private void Start()
    {
        _inventory = FindAnyObjectByType<Inventory>();
        _wordInteractionManager = FindAnyObjectByType<WordInteractionManager>();
    }

    public void CheckForWord()
    {
        _wordInteractionManager.TryLetter(_inventory.heldLetter, index);
    }
}
