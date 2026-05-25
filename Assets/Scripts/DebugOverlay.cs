using UnityEngine;

public class DebugOverlay : MonoBehaviour
{
    private void OnGUI()
    {
        if (ReputationManager.Instance == null || RumorSystem.Instance == null)
            return;

        GUILayout.BeginArea(new Rect(10, 10, 460, 480), GUI.skin.box);

        GUILayout.Label("DEBUG ACTIONS");
        GUILayout.Label("F5 - Helped Animal");
        GUILayout.Label("F6 - Helped Volunteer");
        GUILayout.Label("F7 - Robbed Civilian");
        GUILayout.Label("F8 - Attacked Volunteer");

        GUILayout.Space(10);

        string pathLabel = StoryPathEvaluator.Instance != null
            ? StoryPathEvaluator.Instance.GetPathLabel()
            : "Невідомо";

        GUILayout.Label($"Story Path: {pathLabel}");
        GUILayout.Label($"Good Path Score: {ReputationManager.Instance.GoodPathScore:F2}");
        GUILayout.Label($"Bad Path Score: {ReputationManager.Instance.BadPathScore:F2}");

        GUILayout.Space(10);
        GUILayout.Label($"Oleshky Rep: {ReputationManager.Instance.GetLocationRep(LocationId.Oleshky):F1}");
        GUILayout.Label($"Volunteer Rep: {ReputationManager.Instance.GetFactionRep(FactionType.Volunteer):F1}");
        GUILayout.Label($"Marauder Rep: {ReputationManager.Instance.GetFactionRep(FactionType.Marauder):F1}");
        GUILayout.Label($"Civilian Rep: {ReputationManager.Instance.GetFactionRep(FactionType.Civilian):F1}");

        GUILayout.Space(10);
        GUILayout.Label("Active Rumors:");
        foreach (var rumor in RumorSystem.Instance.ActiveRumors)
        {
            GUILayout.Label($"- {rumor.type} | {rumor.remainingLifetime:F1}s");
        }

        GUILayout.EndArea();
    }
}