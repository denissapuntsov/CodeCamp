using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]
public class InteractionData : ScriptableObject
{
    public string id;
    public GameObject prefab;
    public string leftMouseText = "Change"; 
    public string rightMouseText = "Use";
}
