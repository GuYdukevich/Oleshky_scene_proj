using UnityEngine;

[RequireComponent(typeof(NpcMemory))]
public class NpcDecisionResolver : MonoBehaviour
{
    private NpcMemory memory;

    private void Awake()
    {
        memory = GetComponent<NpcMemory>();
    }

    public void Reevaluate()
    {
        if (memory.Profile == null || ReputationManager.Instance == null)
            return;

        float score = BuildScore();
        NpcReactionType reaction = ResolveByFaction(score, memory.Profile.faction);
        memory.SetReaction(reaction);
    }

    private float BuildScore()
    {
        NpcProfileSO profile = memory.Profile;

        float score = 0f;

        score += ReputationManager.Instance.GetLocationRep(LocationId.Oleshky);
        score += ReputationManager.Instance.GetFactionRep(profile.faction) * 0.6f;

        score += (profile.trust - 50f) * 0.4f;
        score -= (profile.hostility - 50f) * 0.3f;

        if (memory.HasRumor(RumorType.HelpedAnimal) && profile.empathy >= 60f)
            score += 15f;

        if (memory.HasRumor(RumorType.HelpedVolunteer))
            score += 20f;

        if (memory.HasRumor(RumorType.RobbedCivilian))
            score -= 25f;

        if (memory.HasRumor(RumorType.AttackedVolunteer))
        {
            if (profile.faction == FactionType.Volunteer)
                score -= 40f;
            else if (profile.faction == FactionType.Marauder)
                score += 10f;
            else
                score -= 15f;
        }

        return score;
    }

    private NpcReactionType ResolveByFaction(float score, FactionType faction)
    {
        switch (faction)
        {
            case FactionType.Volunteer:
                if (score >= 40f) return NpcReactionType.Friendly;
                if (score >= 10f) return NpcReactionType.Neutral;
                if (score >= -10f) return NpcReactionType.Suspicious;
                if (score >= -30f) return NpcReactionType.RefuseHelp;
                return NpcReactionType.Hostile;

            case FactionType.Marauder:
                if (score >= 35f) return NpcReactionType.Neutral;
                if (score >= 10f) return NpcReactionType.TradeWithPenalty;
                if (score >= -10f) return NpcReactionType.Lie;
                return NpcReactionType.Hostile;

            case FactionType.Civilian:
                if (score >= 25f) return NpcReactionType.Friendly;
                if (score >= 0f) return NpcReactionType.Neutral;
                if (score >= -20f) return NpcReactionType.Suspicious;
                return NpcReactionType.Flee;

            default:
                return NpcReactionType.Neutral;
        }
    }
}