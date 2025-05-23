using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private List<string> words;
    
    private List<InteractionData> _interactions;
    public List<string> Words => words;

    private void Awake()
    {
        _interactions = new List<InteractionData>();
        foreach (var data in Resources.LoadAll<InteractionData>(path: "InteractionData"))
        {
            _interactions.Add(data);
        }
    }

    private void Start()
    {
        words = _interactions.Select(x => x.id.ToLower()).ToList();
    }

    public InteractionData GetInteractionByName(string nameToMatch)
    {
        return _interactions.Find(interaction => interaction.id.ToLower() == nameToMatch.ToLower());
    }
}
