using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private List<Interaction> interactions;

    private List<string> _words;
    public List<string> Words => _words;

    private void Start()
    {
        _words = interactions.Select(x => x.id).ToList();
    }

    public Interaction GetInteractionByName(string nameToMatch)
    {
        return interactions.Find(interaction => interaction.id == nameToMatch.ToLower());
    }
}
