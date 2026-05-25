using System.Collections.Generic;
using UnityEngine;

public class NpcRegistry : MonoBehaviour
{
    public static NpcRegistry Instance { get; private set; }

    private readonly List<NpcMemory> npcs = new List<NpcMemory>();
    public IReadOnlyList<NpcMemory> Npcs => npcs;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(NpcMemory npc)
    {
        if (npc == null || npcs.Contains(npc)) return;
        npcs.Add(npc);
    }

    public void Unregister(NpcMemory npc)
    {
        if (npc == null) return;
        npcs.Remove(npc);
    }
}