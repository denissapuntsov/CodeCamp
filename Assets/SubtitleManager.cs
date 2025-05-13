using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Serialization;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
    [SerializeField] private TextMeshProUGUI captions;
    private CanvasGroup _captionsCanvasGroup;
    private Sequence _sequence;

    private void Awake()
    {
        instance = this;
        _captionsCanvasGroup = captions.GetComponentInParent<CanvasGroup>();
        _captionsCanvasGroup.alpha = 0;
        _sequence = DOTween.Sequence();
    }

    public void ProcessInteractable(InteractionData objectData)
    {
        //Debug.Log(objectData.inspectionText);
        DisplaySubtitle(objectData.inspectionText);
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
}
