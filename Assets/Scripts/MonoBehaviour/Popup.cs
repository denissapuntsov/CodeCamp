using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Linq;

public class Popup : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> mousePopups;
    private List<Vector3> _defaultPopupPositions, _defaultPopupScales;
    
    private void Start()
    {
        _defaultPopupPositions = mousePopups.Select(mousePopup => mousePopup.transform.position).ToList();
        _defaultPopupScales = mousePopups.Select(mousePopup => mousePopup.transform.localScale).ToList();

        foreach (TextMeshProUGUI popup in mousePopups)
        {
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.zero;
        }

        foreach (TextMeshProUGUI popup in mousePopups)
        {
            popup.transform.DOMove(_defaultPopupPositions[mousePopups.IndexOf(popup)], 0.2f);
            popup.transform.DOScale(_defaultPopupScales[mousePopups.IndexOf(popup)], 0.2f);
        }
    }

    public void Disappear()
    {
        Sequence sequence = DOTween.Sequence();
        
        foreach (TextMeshProUGUI popup in mousePopups)
        {
            sequence.Join(popup.transform.DOLocalMove(Vector3.zero, 0.2f));
            sequence.Join(popup.transform.DOScale(Vector3.zero, 0.2f));
        }

        sequence.Play().OnComplete(() => Destroy(gameObject));
    }

    public void SetUseText(string leftMousePopupText, string rightMousePopupText)
    {
        mousePopups[0].text = leftMousePopupText;
        mousePopups[1].text = rightMousePopupText;
    }
}
