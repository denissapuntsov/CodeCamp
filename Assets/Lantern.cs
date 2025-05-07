using DG.Tweening;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    private float _swingRange, _swingDuration;
    private void Start()
    {
        _swingRange = Random.Range(0.5f, 0.7f);
        _swingDuration = Random.Range(3.5f, 5f);
        transform.rotation = Quaternion.Euler(_swingRange, 0, 0);
        
        Sequence sequence = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo);
        sequence.Append(transform.DOLocalRotate(new Vector3(-transform.rotation.x, 0, 0), _swingDuration)).SetEase(Ease.InSine);
    }
}
