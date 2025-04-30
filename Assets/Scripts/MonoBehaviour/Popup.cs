using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Linq;

public class Popup : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> mousePopups;
    private List<Vector3> _defaultPopupPositions, _defaultPopupScales;
    private Color _defaultPopupColor;
    public bool hasAppeared = false;
    
    private void Start()
    {
        _defaultPopupPositions = mousePopups.Select(mousePopup => mousePopup.transform.localPosition).ToList();
        _defaultPopupScales = mousePopups.Select(mousePopup => mousePopup.transform.localScale).ToList();
        _defaultPopupColor = Color.white;
        
        ResetPositionsToZero();
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void UpdateSelection(string mode)
    {
        foreach (TextMeshProUGUI mousePopup in mousePopups) mousePopup.gameObject.SetActive(false);
        switch (mode)
        {
            case "Approach":
                mousePopups[0].gameObject.SetActive(true);
                break;
            case "Use":
                mousePopups[1].gameObject.SetActive(true);
                mousePopups[2].gameObject.SetActive(true);
                break;
        }
    }

    public void Appear()
    {
        Appear(0.2f);
    }
    
    public void Appear(float duration)
    {
        if (hasAppeared) return;
        hasAppeared = true;

        ResetPositionsToZero();
        
        foreach (TextMeshProUGUI popup in mousePopups)
        {
            popup.transform.DOLocalMove(_defaultPopupPositions[mousePopups.IndexOf(popup)], duration);
            popup.transform.DOScale(_defaultPopupScales[mousePopups.IndexOf(popup)], duration);
            popup.GetComponent<TextMeshProUGUI>().DOColor(_defaultPopupColor, duration);
        }
    }

    private void ResetPositionsToZero()
    {
        foreach (TextMeshProUGUI popup in mousePopups)
        {
            popup.transform.localPosition = Vector3.zero;
            popup.transform.localScale = Vector3.zero;
        }
    }

    public void Disappear()
    {
        Disappear(0.2f);
    }

    public void Disappear(float duration)
    {
        
        foreach (TextMeshProUGUI popup in mousePopups)
        {
            popup.transform.DOLocalMove(Vector3.zero, duration);
            popup.transform.DOScale(Vector3.zero, duration);
            popup.GetComponent<TextMeshProUGUI>().DOColor(Color.clear, duration);
        }
        hasAppeared = false;
    }

    public void SetPlayerText(string text)
    {
        mousePopups[0].text = text;
    }

    public void SetUseText(string leftMousePopupText, string rightMousePopupText)
    {
        mousePopups[1].text = leftMousePopupText;
        mousePopups[2].text = rightMousePopupText;
    }
}
