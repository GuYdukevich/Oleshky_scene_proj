using UnityEngine;

public class PlayerActionTracker : MonoBehaviour
{
    public static PlayerActionTracker Instance { get; private set; }

    [Header("Debug Hotkeys")]
    [SerializeField] private bool enableDebugHotkeys = true;
    [SerializeField] private KeyCode helpedAnimalKey = KeyCode.F5;
    [SerializeField] private KeyCode helpedVolunteerKey = KeyCode.F6;
    [SerializeField] private KeyCode robbedCivilianKey = KeyCode.F7;
    [SerializeField] private KeyCode attackedVolunteerKey = KeyCode.F8;

    private PlayerInteractionController interactionController;

    private void Awake()
    {
        Instance = this;
        interactionController = GetComponent<PlayerInteractionController>();
    }

    private void Update()
    {
        if (!enableDebugHotkeys)
            return;

        if (interactionController != null && interactionController.InteractionOpen)
            return;

        if (Input.GetKeyDown(helpedAnimalKey))
            RegisterHelpedAnimal();

        if (Input.GetKeyDown(helpedVolunteerKey))
            RegisterHelpedVolunteer();

        if (Input.GetKeyDown(robbedCivilianKey))
            RegisterRobbedCivilian();

        if (Input.GetKeyDown(attackedVolunteerKey))
            RegisterAttackedVolunteer();
    }

    public void RegisterHelpedAnimal()
    {
        ReputationManager.Instance.ApplyAction(RumorType.HelpedAnimal);
        RumorSystem.Instance.CreateRumor(RumorType.HelpedAnimal, FactionType.Player, "Гравець врятував тварину");
    }

    public void RegisterHelpedVolunteer()
    {
        ReputationManager.Instance.ApplyAction(RumorType.HelpedVolunteer);
        RumorSystem.Instance.CreateRumor(RumorType.HelpedVolunteer, FactionType.Player, "Гравець допоміг волонтеру");
    }

    public void RegisterRobbedCivilian()
    {
        ReputationManager.Instance.ApplyAction(RumorType.RobbedCivilian);
        RumorSystem.Instance.CreateRumor(RumorType.RobbedCivilian, FactionType.Player, "Гравець пограбував цивільного");
    }

    public void RegisterAttackedVolunteer()
    {
        ReputationManager.Instance.ApplyAction(RumorType.AttackedVolunteer);
        RumorSystem.Instance.CreateRumor(RumorType.AttackedVolunteer, FactionType.Player, "Гравець атакував волонтера");
    }

    public void RegisterInteraction(FactionType targetFaction, bool respectful)
    {
        ReputationManager.Instance.ApplyInteraction(targetFaction, respectful);

        RumorType rumorType = respectful ? RumorType.RespectfulApproach : RumorType.AggressiveApproach;
        string summary = respectful
            ? $"Гравець звернувся мирно до {targetFaction}"
            : $"Гравець звернувся агресивно до {targetFaction}";

        RumorSystem.Instance.CreateRumor(rumorType, FactionType.Player, summary);
    }
}