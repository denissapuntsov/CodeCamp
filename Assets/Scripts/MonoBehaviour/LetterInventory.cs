using UnityEngine;
using TMPro;

public class LetterInventory : MonoBehaviour
{
    [SerializeField] public char heldLetter;
    [SerializeField] GameObject inventoryLetter;
    private void Start()
    {
        SetLetter(heldLetter);
    }

    public void SetLetter(char letter)
    {
        inventoryLetter.GetComponentInChildren<TextMeshProUGUI>().text = letter.ToString();
        heldLetter = letter;
    }
}
