using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(NpcMemory))]
public class ColorStateView : MonoBehaviour
{
    private Renderer rend;
    private NpcMemory memory;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        memory = GetComponent<NpcMemory>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        rend.material.color = GetColor(memory.CurrentReaction);
    }

    private Color GetColor(NpcReactionType reaction)
    {
        switch (reaction)
        {
            case NpcReactionType.Friendly: return Color.green;
            case NpcReactionType.Neutral: return Color.white;
            case NpcReactionType.Suspicious: return Color.yellow;
            case NpcReactionType.RefuseHelp: return Color.gray;
            case NpcReactionType.TradeWithPenalty: return new Color(1f, 0.6f, 0f);
            case NpcReactionType.Lie: return new Color(1f, 0.5f, 0f);
            case NpcReactionType.Flee: return Color.cyan;
            case NpcReactionType.Hostile: return Color.red;
            default: return Color.white;
        }
    }
}