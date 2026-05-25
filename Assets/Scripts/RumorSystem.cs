using System.Collections.Generic;
using UnityEngine;

public class RumorSystem : MonoBehaviour
{
    public static RumorSystem Instance { get; private set; }

    [SerializeField] private List<RumorData> activeRumors = new List<RumorData>();
    public IReadOnlyList<RumorData> ActiveRumors => activeRumors;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        for (int i = activeRumors.Count - 1; i >= 0; i--)
        {
            activeRumors[i].remainingLifetime -= Time.deltaTime;

            if (activeRumors[i].remainingLifetime <= 0f)
                activeRumors.RemoveAt(i);
        }
    }

    public void CreateRumor(RumorType type, FactionType sourceFaction, string summary)
    {
        RumorData rumor = new RumorData
        {
            type = type,
            location = LocationId.Oleshky,
            impact = GetImpact(type),
            remainingLifetime = GetLifetime(type),
            summary = summary,
            sourceFaction = sourceFaction
        };

        activeRumors.Add(rumor);

        if (NpcRegistry.Instance != null)
        {
            foreach (NpcMemory npc in NpcRegistry.Instance.Npcs)
            {
                npc.ReceiveRumor(rumor.Clone());
            }
        }

        Debug.Log($"[Rumor] {rumor.summary}");
    }

    private float GetImpact(RumorType type)
    {
        switch (type)
        {
            case RumorType.HelpedAnimal: return 10f;
            case RumorType.HelpedVolunteer: return 15f;
            case RumorType.RobbedCivilian: return -20f;
            case RumorType.AttackedVolunteer: return -30f;
            case RumorType.RespectfulApproach: return 5f;
            case RumorType.AggressiveApproach: return -10f;
            default: return 0f;
        }
    }

    private float GetLifetime(RumorType type)
    {
        switch (type)
        {
            case RumorType.HelpedAnimal: return 25f;
            case RumorType.HelpedVolunteer: return 30f;
            case RumorType.RobbedCivilian: return 35f;
            case RumorType.AttackedVolunteer: return 40f;
            case RumorType.RespectfulApproach: return 20f;
            case RumorType.AggressiveApproach: return 25f;
            default: return 20f;
        }
    }
}