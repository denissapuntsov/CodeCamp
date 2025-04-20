using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leftMousePopup, rightMousePopup;

    public void SetText(string leftMousePopupText, string rightMousePopupText)
    {
        leftMousePopup.text = leftMousePopupText;
        rightMousePopup.text = rightMousePopupText;
    }
}
