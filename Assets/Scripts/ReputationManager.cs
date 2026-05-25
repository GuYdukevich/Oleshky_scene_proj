using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    public static ReputationManager Instance { get; private set; }

    [Header("Current Reputation")]
    [SerializeField] private float oleshkyRep = 0f;
    [SerializeField] private float volunteerRep = 0f;
    [SerializeField] private float marauderRep = 0f;
    [SerializeField] private float civilianRep = 0f;

    [Header("Story Drift")]
    [SerializeField] private float goodPathScore = 0f;
    [SerializeField] private float badPathScore = 0f;

    public float GoodPathScore => goodPathScore;
    public float BadPathScore => badPathScore;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public float GetLocationRep(LocationId location)
    {
        return location == LocationId.Oleshky ? oleshkyRep : 0f;
    }

    public float GetFactionRep(FactionType faction)
    {
        switch (faction)
        {
            case FactionType.Volunteer: return volunteerRep;
            case FactionType.Marauder: return marauderRep;
            case FactionType.Civilian: return civilianRep;
            default: return 0f;
        }
    }

    public void ApplyAction(RumorType action)
    {
        switch (action)
        {
            case RumorType.HelpedAnimal:
                oleshkyRep += 15f;
                volunteerRep += 10f;
                marauderRep -= 5f;
                goodPathScore += 1f;
                break;

            case RumorType.HelpedVolunteer:
                oleshkyRep += 20f;
                volunteerRep += 15f;
                goodPathScore += 1.5f;
                break;

            case RumorType.RobbedCivilian:
                oleshkyRep -= 25f;
                volunteerRep -= 20f;
                civilianRep -= 25f;
                marauderRep += 10f;
                badPathScore += 2f;
                break;

            case RumorType.AttackedVolunteer:
                oleshkyRep -= 35f;
                volunteerRep -= 35f;
                marauderRep += 10f;
                badPathScore += 3f;
                break;
        }

        ClampAll();
        LogState();
    }

    public void ApplyInteraction(FactionType targetFaction, bool respectful)
    {
        if (respectful)
        {
            switch (targetFaction)
            {
                case FactionType.Volunteer:
                    oleshkyRep += 5f;
                    volunteerRep += 10f;
                    civilianRep += 2f;
                    goodPathScore += 0.75f;
                    break;

                case FactionType.Civilian:
                    oleshkyRep += 5f;
                    civilianRep += 10f;
                    volunteerRep += 5f;
                    goodPathScore += 0.75f;
                    break;

                case FactionType.Marauder:
                    oleshkyRep -= 2f;
                    marauderRep += 5f;
                    badPathScore += 0.25f;
                    break;
            }
        }
        else
        {
            switch (targetFaction)
            {
                case FactionType.Volunteer:
                    oleshkyRep -= 12f;
                    volunteerRep -= 15f;
                    civilianRep -= 5f;
                    marauderRep += 5f;
                    badPathScore += 1.5f;
                    break;

                case FactionType.Civilian:
                    oleshkyRep -= 10f;
                    civilianRep -= 15f;
                    volunteerRep -= 8f;
                    marauderRep += 5f;
                    badPathScore += 1.25f;
                    break;

                case FactionType.Marauder:
                    oleshkyRep -= 8f;
                    volunteerRep -= 5f;
                    civilianRep -= 5f;
                    marauderRep += 8f;
                    badPathScore += 0.75f;
                    break;
            }
        }

        ClampAll();
        LogState();
    }

    private void ClampAll()
    {
        oleshkyRep = Mathf.Clamp(oleshkyRep, -100f, 100f);
        volunteerRep = Mathf.Clamp(volunteerRep, -100f, 100f);
        marauderRep = Mathf.Clamp(marauderRep, -100f, 100f);
        civilianRep = Mathf.Clamp(civilianRep, -100f, 100f);

        goodPathScore = Mathf.Clamp(goodPathScore, 0f, 100f);
        badPathScore = Mathf.Clamp(badPathScore, 0f, 100f);
    }

    private void LogState()
    {
        Debug.Log(
            $"[Reputation] Oleshky={oleshkyRep}, Volunteers={volunteerRep}, Marauders={marauderRep}, Civilians={civilianRep}, GoodPath={goodPathScore}, BadPath={badPathScore}"
        );
    }
}