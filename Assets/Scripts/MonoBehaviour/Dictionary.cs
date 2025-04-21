using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private List<Interaction> interactions;

    public List<string> words;

    private void Start()
    {
        words = interactions.Select(interaction => interaction.name).ToList();
    }

    public Interaction GetInteractionByName(string nameToMatch)
    {
        return interactions.Find(interaction => interaction.id == nameToMatch.ToLower());
    }
}
