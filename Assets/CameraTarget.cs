using DG.Tweening;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float speed = 1.0f;
    
    public void SetTarget(Transform target)
    {
        var duration = Vector3.Distance(transform.position, target.position) / speed;
        transform.DOMove(target.position, duration).SetEase(Ease.InOutSine);
    }
}
