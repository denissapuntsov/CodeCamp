using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
[Serializable]

public struct Neighbour
{
    public string name;
    [InspectorName("Character")] public char differenceChar;
    [InspectorName("Index")] public int differenceIndex;
}
