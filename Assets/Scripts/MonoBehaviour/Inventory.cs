using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField] public char heldLetter;
    [SerializeField] GameObject inventoryLetter;
    private void Start()
    {
        SetLetter(heldLetter);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetLetter('g');
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetLetter('o');
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SetLetter('l');
        }
    }

    public void SetLetter(char letter)
    {
        inventoryLetter.GetComponentInChildren<TextMeshProUGUI>().text = letter.ToString();
        heldLetter = letter;
    }
}
