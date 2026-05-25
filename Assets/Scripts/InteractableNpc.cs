using UnityEngine;

[RequireComponent(typeof(NpcMemory))]
public class InteractableNpc : MonoBehaviour
{
    [SerializeField] private string npcDisplayName = "NPC";

    private NpcMemory memory;
    private bool clueGiven = false;

    private void Awake()
    {
        memory = GetComponent<NpcMemory>();
    }

    public string GetDisplayName()
    {
        if (!string.IsNullOrWhiteSpace(npcDisplayName))
            return npcDisplayName;

        return memory.Profile != null ? memory.Profile.faction.ToString() : "NPC";
    }

    public string GetOption1Text()
    {
        if (memory.Profile == null) return "1. Ввічливо звернутись";

        switch (memory.Profile.faction)
        {
            case FactionType.Volunteer: return "1. Ввічливо попросити допомогу";
            case FactionType.Marauder: return "1. Спробувати домовитись";
            case FactionType.Civilian: return "1. Спокійно розпитати";
            default: return "1. Ввічливо звернутись";
        }
    }

    public string GetOption2Text()
    {
        if (memory.Profile == null) return "2. Грубо звернутись";

        switch (memory.Profile.faction)
        {
            case FactionType.Volunteer: return "2. Тиснути й вимагати відповідь";
            case FactionType.Marauder: return "2. Погрожувати / тиснути";
            case FactionType.Civilian: return "2. Грубо вимагати відповідь";
            default: return "2. Грубо звернутись";
        }
    }

    public InteractionResult Interact(bool respectful)
    {
        InteractionResult result = new InteractionResult
        {
            respectful = respectful,
            gaveUsefulInfo = false,
            wasLie = false,
            targetFaction = memory.Profile != null ? memory.Profile.faction : FactionType.Civilian
        };

        if (memory.Profile == null)
        {
            result.responseText = "У NPC немає профілю.";
            return result;
        }

        PlayerActionTracker.Instance.RegisterInteraction(memory.Profile.faction, respectful);

        NpcDecisionResolver resolver = GetComponent<NpcDecisionResolver>();
        if (resolver != null)
            resolver.Reevaluate();

        NpcReactionType reactionAfter = memory.CurrentReaction;
        result.responseText = BuildResponse(reactionAfter, respectful, result);

        return result;
    }

    private string BuildResponse(NpcReactionType reaction, bool respectful, InteractionResult result)
    {
        switch (memory.Profile.faction)
        {
            case FactionType.Volunteer:
                return BuildVolunteerResponse(reaction, respectful, result);

            case FactionType.Marauder:
                return BuildMarauderResponse(reaction, respectful, result);

            case FactionType.Civilian:
                return BuildCivilianResponse(reaction, respectful, result);

            default:
                return "NPC мовчить.";
        }
    }

    private string BuildVolunteerResponse(NpcReactionType reaction, bool respectful, InteractionResult result)
    {
        if (respectful)
        {
            switch (reaction)
            {
                case NpcReactionType.Friendly:
                    if (!clueGiven)
                    {
                        clueGiven = true;
                        result.gaveUsefulInfo = true;
                        return "Волонтер: Бачу, ти допомагаєш, а не грабуєш. У східній частині Олешок бачили слід дітей. Це хороша лінія.";
                    }
                    return "Волонтер: Я вже розповів, що знав. Тримайся правильної дороги.";

                case NpcReactionType.Neutral:
                    return "Волонтер: Спершу покажи, що ти не мародер. Тоді поговоримо.";

                case NpcReactionType.Suspicious:
                    return "Волонтер: Я тобі не довіряю. Зараз ти дуже близько до поганої лінії.";

                case NpcReactionType.RefuseHelp:
                    return "Волонтер: Після твоїх дій допомоги не буде.";

                case NpcReactionType.Hostile:
                    return "Волонтер: Відійди зараз же.";
            }
        }
        else
        {
            switch (reaction)
            {
                case NpcReactionType.Friendly:
                case NpcReactionType.Neutral:
                    return "Волонтер: Не тисни на мене. Так ти сам псуєш свою лінію.";

                default:
                    return "Волонтер: Нічого ти від мене не отримаєш.";
            }
        }

        return "Волонтер мовчить.";
    }

    private string BuildMarauderResponse(NpcReactionType reaction, bool respectful, InteractionResult result)
    {
        if (respectful)
        {
            switch (reaction)
            {
                case NpcReactionType.Neutral:
                    return "Мародер: Говорити можна. Але безкоштовно тут нічого не дають.";

                case NpcReactionType.TradeWithPenalty:
                    return "Мародер: Якщо хочеш знати більше — плати.";

                case NpcReactionType.Lie:
                    result.wasLie = true;
                    return "Мародер: Чув, що слід треба шукати на заході. Може правда, а може і ні.";

                case NpcReactionType.Hostile:
                    return "Мародер: Забирайся звідси.";
            }
        }
        else
        {
            switch (reaction)
            {
                case NpcReactionType.Neutral:
                case NpcReactionType.TradeWithPenalty:
                    return "Мародер: О, жорсткий тон. Так і стають своїми для темної лінії.";

                case NpcReactionType.Lie:
                    result.wasLie = true;
                    return "Мародер: Натискай далі — я ще більше 'допоможу' брехнею.";

                case NpcReactionType.Hostile:
                    return "Мародер: Ще слово — і буде бійка.";
            }
        }

        return "Мародер мовчить.";
    }

    private string BuildCivilianResponse(NpcReactionType reaction, bool respectful, InteractionResult result)
    {
        if (respectful)
        {
            switch (reaction)
            {
                case NpcReactionType.Friendly:
                    result.gaveUsefulInfo = true;
                    return "Цивільний: Волонтери в східному секторі знають більше. Іди до них — це правильний шлях.";

                case NpcReactionType.Neutral:
                    return "Цивільний: Я чув, що волонтери шукають дітей. Поговори з ними.";

                case NpcReactionType.Suspicious:
                    return "Цивільний: Я не впевнений, що можу тобі щось казати.";

                case NpcReactionType.Flee:
                    return "Цивільний: Я боюсь тебе... не підходь.";
            }
        }
        else
        {
            return "Цивільний: Я нічого не знаю! Відійди!";
        }

        return "Цивільний мовчить.";
    }
}