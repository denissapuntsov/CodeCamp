using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Interaction", menuName = "Scriptable Objects/Interaction")]
public class Interaction : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public UnityEvent onRightClick, onLeftClick;
}
