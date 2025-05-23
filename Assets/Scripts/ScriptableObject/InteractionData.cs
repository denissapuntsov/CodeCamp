using UnityEngine;

[CreateAssetMenu(fileName = "InteractionData", menuName = "Scriptable Objects/InteractionData")]

public class InteractionData : ScriptableObject
{
    public string id;
    public Type interactionType;
    public GameObject prefab;
    public string leftMouseText = "Change"; 
    public string rightMouseText = "Use";
    public string inspectionText = "This is an Inspectable";
    public Quaternion headgearRotation = Quaternion.identity;
    public AudioClip audio;
}

public enum Type
{
    None,
    Headgear,
    Traversal,
    Inspectable
};