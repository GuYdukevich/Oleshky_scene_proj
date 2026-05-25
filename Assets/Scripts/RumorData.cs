using System;
using UnityEngine;

[Serializable]
public class RumorData
{
    public RumorType type;
    public LocationId location;
    public float impact;
    public float remainingLifetime;
    public string summary;
    public FactionType sourceFaction;

    public RumorData Clone()
    {
        return (RumorData)MemberwiseClone();
    }
}