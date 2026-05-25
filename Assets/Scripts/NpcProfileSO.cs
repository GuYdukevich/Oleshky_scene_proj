using UnityEngine;

[CreateAssetMenu(menuName = "AI/NPC Profile", fileName = "NPC_Profile")]
public class NpcProfileSO : ScriptableObject
{
    public FactionType faction;

    [Range(0, 100)] public float empathy = 50f;
    [Range(0, 100)] public float trust = 50f;
    [Range(0, 100)] public float hostility = 50f;

    public bool canTrade = false;
    public bool canSpreadRumors = true;
}