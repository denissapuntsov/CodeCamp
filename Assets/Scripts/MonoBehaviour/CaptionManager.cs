using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CaptionManager : MonoBehaviour
{
    public static CaptionManager Instance;

    public List<Caption> captionList;
    [SerializeField] private TextMeshProUGUI captions;
    private CanvasGroup _captionsCanvasGroup;
    private Sequence _sequence;

    private void Awake()
    {
        Instance = this;
        _captionsCanvasGroup = captions.GetComponentInParent<CanvasGroup>();
        _captionsCanvasGroup.alpha = 0;
        _sequence = DOTween.Sequence();
    }

    public void ProcessInteractable(InteractionData objectData)
    {
        //Debug.Log(objectData.inspectionText);
        DisplaySubtitle(objectData.inspectionText, duration: 4f);
    }
    
    public void DisplaySubtitle(string subtitle, float duration)
    {
        DisplaySubtitle(subtitle);
        _sequence.AppendInterval(duration);
        _sequence.Append(_captionsCanvasGroup.DOFade(endValue: 0, duration: 0.5f));
    }

    public void ClearSubtitle()
    {
        DisplaySubtitle(String.Empty);
    }

    public void DisplaySubtitle(string subtitle)
    {
        _captionsCanvasGroup.gameObject.SetActive(true);
        _sequence = DOTween.Sequence();
        
        if (_captionsCanvasGroup.alpha != 0 || captions.text.Length != 0)
        {
            _sequence.Append(_captionsCanvasGroup.DOFade(endValue: 0, duration: 0.5f));
        }
        
        _sequence.AppendCallback(() =>
        {
            captions.text = String.Empty;
            captions.text = subtitle;
        });

        if (subtitle == String.Empty) return;
        _sequence.Append(_captionsCanvasGroup.DOFade(endValue: 1, duration: 1f));
    }

    public string GetCaptionTextByID(string id)
    {
        string textToReturn = String.Empty;
        foreach (Caption caption in captionList)
        {
            if (!caption) continue;
            if (caption.id == id)
            {
               textToReturn = caption.text;
            }
        }
        return textToReturn;
    }
}
