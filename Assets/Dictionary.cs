using System;
using System.Collections.Generic;
using UnityEngine;

public class Dictionary : MonoBehaviour
{
    [SerializeField] private List<Interaction> interactions;

    public List<string> words;

    private void Start()
    {
        foreach (var interaction in interactions)
        {
            words.Add(interaction.name.ToLower());
        }
    }

    public Interaction GetInteractionByName(string nameToMatch)
    {
        return interactions.Find(x => x.id == nameToMatch.ToLower());
    }
}
