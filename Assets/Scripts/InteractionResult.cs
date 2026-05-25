using System;

[Serializable]
public class InteractionResult
{
    public string responseText;
    public bool respectful;
    public bool gaveUsefulInfo;
    public bool wasLie;
    public FactionType targetFaction;
}