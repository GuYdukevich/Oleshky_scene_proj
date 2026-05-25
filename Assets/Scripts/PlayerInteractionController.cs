using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private PlayerInteractionDetector detector;

    private bool interactionOpen = false;
    private string lastResponse = "";

    public bool InteractionOpen => interactionOpen;
    public string LastResponse => lastResponse;

    public InteractableNpc CurrentNpc
    {
        get
        {
            if (detector == null) return null;
            return detector.CurrentNpc;
        }
    }

    private void Update()
    {
        if (detector == null) return;

        if (CurrentNpc == null)
        {
            interactionOpen = false;
            return;
        }

        if (!interactionOpen && Input.GetKeyDown(KeyCode.E))
        {
            interactionOpen = true;
        }

        if (interactionOpen && Input.GetKeyDown(KeyCode.Alpha1))
        {
            MakeChoice(true);
        }

        if (interactionOpen && Input.GetKeyDown(KeyCode.Alpha2))
        {
            MakeChoice(false);
        }

        if (interactionOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            interactionOpen = false;
        }
    }

    private void MakeChoice(bool respectful)
    {
        if (CurrentNpc == null) return;

        InteractionResult result = CurrentNpc.Interact(respectful);
        lastResponse = result.responseText;
        interactionOpen = false;
    }
}