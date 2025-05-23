using DG.Tweening;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public float speed = 1.0f;
    
    public void SetTarget(Transform target)
    {
        transform.DOMove(target.position, speed).SetEase(Ease.InOutSine);
    }

    public void SetTargetWithDuration(Transform target)
    {
        var duration = Vector3.Distance(transform.position, target.position) / speed;
        transform.DOMove(target.position, speed).SetEase(Ease.InOutSine);
    }
}
