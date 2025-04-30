using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]

public class InteractionData : ScriptableObject
{
    public string id;
    public Type interactionType;
    public GameObject prefab;
    public string leftMouseText = "Change"; 
    public string rightMouseText = "Use";
}

public enum Type
{
    None,
    Headgear,
    Traversal,
    Special
};