using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private PlayerInteractionController controller;

    private void OnGUI()
    {
        if (StoryPathEvaluator.Instance != null)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 360, 10, 340, 120), GUI.skin.box);
            GUILayout.Label("СЮЖЕТНА ЛІНІЯ");
            GUILayout.Label($"Поточний шлях: {StoryPathEvaluator.Instance.GetPathLabel()}");
            GUILayout.EndArea();
        }

        if (controller == null || controller.CurrentNpc == null)
            return;

        InteractableNpc npc = controller.CurrentNpc;
        NpcMemory memory = npc.GetComponent<NpcMemory>();

        if (!controller.InteractionOpen)
        {
            GUILayout.BeginArea(new Rect(10, Screen.height - 120, 520, 100), GUI.skin.box);
            GUILayout.Label($"Поруч: {npc.GetDisplayName()}");
            GUILayout.Label($"Стан NPC: {memory.CurrentReaction}");
            GUILayout.Label("Натисни E для взаємодії");
            GUILayout.EndArea();
        }
        else
        {
            GUILayout.BeginArea(new Rect(10, Screen.height - 220, 620, 200), GUI.skin.box);
            GUILayout.Label($"Розмова з: {npc.GetDisplayName()}");
            GUILayout.Label($"Поточний стан NPC: {memory.CurrentReaction}");
            GUILayout.Space(10);
            GUILayout.Label(npc.GetOption1Text());
            GUILayout.Label(npc.GetOption2Text());
            GUILayout.Space(10);
            GUILayout.Label("Натисни 1 або 2. ESC — вийти.");
            GUILayout.EndArea();
        }

        if (!string.IsNullOrEmpty(controller.LastResponse))
        {
            GUILayout.BeginArea(new Rect(10, Screen.height - 340, 720, 100), GUI.skin.box);
            GUILayout.Label("ВІДПОВІДЬ NPC");
            GUILayout.Label(controller.LastResponse);
            GUILayout.EndArea();
        }
    }
}