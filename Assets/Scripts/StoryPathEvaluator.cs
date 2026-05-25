using UnityEngine;

public class StoryPathEvaluator : MonoBehaviour
{
    public static StoryPathEvaluator Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public StoryPathType GetCurrentPath()
    {
        if (ReputationManager.Instance == null)
            return StoryPathType.Neutral;

        float oleshky = ReputationManager.Instance.GetLocationRep(LocationId.Oleshky);
        float volunteer = ReputationManager.Instance.GetFactionRep(FactionType.Volunteer);
        float civilian = ReputationManager.Instance.GetFactionRep(FactionType.Civilian);
        float marauder = ReputationManager.Instance.GetFactionRep(FactionType.Marauder);

        float good = ReputationManager.Instance.GoodPathScore;
        float bad = ReputationManager.Instance.BadPathScore;

        float goodWeight =
            good * 12f +
            Mathf.Max(0f, volunteer) * 0.8f +
            Mathf.Max(0f, civilian) * 0.6f +
            Mathf.Max(0f, oleshky) * 0.4f;

        float badWeight =
            bad * 12f +
            Mathf.Max(0f, -volunteer) * 0.8f +
            Mathf.Max(0f, -civilian) * 0.7f +
            Mathf.Max(0f, -oleshky) * 0.4f +
            Mathf.Max(0f, marauder) * 0.2f;

        if (bad >= 4f && badWeight >= goodWeight + 6f)
            return StoryPathType.Bad;

        if (good >= 4f && goodWeight >= badWeight + 6f)
            return StoryPathType.Good;

        if (badWeight > goodWeight + 8f)
            return StoryPathType.Bad;

        if (goodWeight > badWeight + 8f)
            return StoryPathType.Good;

        return StoryPathType.Neutral;
    }

    public string GetPathLabel()
    {
        switch (GetCurrentPath())
        {
            case StoryPathType.Good: return "Гарна лінія";
            case StoryPathType.Bad: return "Погана лінія";
            default: return "Нейтральна лінія";
        }
    }
}