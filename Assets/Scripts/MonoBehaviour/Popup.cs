using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private List<GameObject> popupPanels;
    private List<Vector3> _defaultPopupPositions, _defaultPopupScales;
    private Color _defaultPopupColor, _defaultTextColor;
    public bool hasAppeared = false;
    
    private void Start()
    {
        _defaultPopupPositions = popupPanels.Select(mousePopup => mousePopup.transform.localPosition).ToList();
        _defaultPopupScales = popupPanels.Select(mousePopup => mousePopup.transform.localScale).ToList();
        _defaultTextColor = Color.white;
        _defaultPopupColor = Color.black;
        
        ResetPositionsToZero();
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    public void UpdateSelection(string mode)
    {
        foreach (GameObject popupPanel in popupPanels) popupPanel.SetActive(false);
        switch (mode)
        {
            case "Approach":
                popupPanels[0].SetActive(true);
                break;
            case "Use":
                popupPanels[1].SetActive(true);
                popupPanels[2].SetActive(true);
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
        
        foreach (GameObject popup in popupPanels)
        {
            popup.transform.DOLocalMove(_defaultPopupPositions[popupPanels.IndexOf(popup)], duration);
            popup.transform.DOScale(_defaultPopupScales[popupPanels.IndexOf(popup)], duration);
            popup.GetComponentInChildren<Image>().DOColor(_defaultPopupColor, duration);
            popup.GetComponentInChildren<TextMeshProUGUI>().DOColor(_defaultTextColor, duration);
        }
    }

    private void ResetPositionsToZero()
    {
        foreach (GameObject popup in popupPanels)
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
        
        foreach (GameObject popup in popupPanels)
        {
            popup.transform.DOLocalMove(Vector3.zero, duration);
            popup.transform.DOScale(Vector3.zero, duration);
            popup.GetComponentInChildren<Image>().DOColor(Color.clear, duration);
            popup.GetComponentInChildren<TextMeshProUGUI>().DOColor(Color.clear, duration);
        }
        hasAppeared = false;
    }

    public void SetText(string text)
    {
        popupPanels[0].GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void SetUseText(string leftMousePopupText, string rightMousePopupText)
    {
        popupPanels[1].GetComponentInChildren<TextMeshProUGUI>().text = leftMousePopupText;
        popupPanels[2].GetComponentInChildren<TextMeshProUGUI>().text = rightMousePopupText;
    }
}
