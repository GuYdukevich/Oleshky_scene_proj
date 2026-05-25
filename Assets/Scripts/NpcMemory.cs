using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NpcDecisionResolver))]
public class NpcMemory : MonoBehaviour
{
    [SerializeField] private NpcProfileSO profile;
    [SerializeField] private List<RumorData> knownRumors = new List<RumorData>();
    [SerializeField] private NpcReactionType currentReaction = NpcReactionType.Neutral;

    private NpcDecisionResolver resolver;

    public NpcProfileSO Profile => profile;
    public NpcReactionType CurrentReaction => currentReaction;
    public List<RumorData> KnownRumors => knownRumors;

    private void Awake()
    {
        resolver = GetComponent<NpcDecisionResolver>();
    }

    private void OnEnable()
    {
        if (NpcRegistry.Instance != null)
            NpcRegistry.Instance.Register(this);
    }

    private void OnDisable()
    {
        if (NpcRegistry.Instance != null)
            NpcRegistry.Instance.Unregister(this);
    }

    private void Start()
    {
        resolver.Reevaluate();
    }

    private void Update()
    {
        bool removedSomething = false;

        for (int i = knownRumors.Count - 1; i >= 0; i--)
        {
            knownRumors[i].remainingLifetime -= Time.deltaTime;

            if (knownRumors[i].remainingLifetime <= 0f)
            {
                knownRumors.RemoveAt(i);
                removedSomething = true;
            }
        }

        if (removedSomething)
            resolver.Reevaluate();
    }

    public void ReceiveRumor(RumorData rumor)
    {
        knownRumors.Add(rumor);
        resolver.Reevaluate();
    }

    public bool HasRumor(RumorType type)
    {
        return knownRumors.Exists(r => r.type == type);
    }

    public void SetReaction(NpcReactionType reaction)
    {
        currentReaction = reaction;
        GetComponent<ColorStateView>()?.Refresh();
    }
}