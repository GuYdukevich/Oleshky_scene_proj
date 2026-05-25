using UnityEngine;

public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 3f;

    public InteractableNpc CurrentNpc { get; private set; }

    private void Update()
    {
        FindClosestNpc();
    }

    private void FindClosestNpc()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);

        InteractableNpc bestNpc = null;
        float bestDistance = float.MaxValue;

        foreach (Collider hit in hits)
        {
            InteractableNpc npc = hit.GetComponent<InteractableNpc>();
            if (npc == null) continue;

            float dist = Vector3.Distance(transform.position, npc.transform.position);
            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestNpc = npc;
            }
        }

        CurrentNpc = bestNpc;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}