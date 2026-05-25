using UnityEngine;

public enum FactionType
{
    Player,
    Volunteer,
    Marauder,
    Civilian
}

public enum LocationId
{
    Oleshky
}

public enum RumorType
{
    HelpedAnimal,
    HelpedVolunteer,
    RobbedCivilian,
    AttackedVolunteer,
    RespectfulApproach,
    AggressiveApproach
}

public enum NpcReactionType
{
    Friendly,
    Neutral,
    Suspicious,
    RefuseHelp,
    TradeWithPenalty,
    Lie,
    Flee,
    Hostile
}

public enum StoryPathType
{
    Good,
    Neutral,
    Bad
}