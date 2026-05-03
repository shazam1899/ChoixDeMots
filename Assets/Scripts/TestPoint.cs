using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public GameFlow progressionlocal;
    public GameObject ButtonHUB;
    

    private bool triggered = false;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("[TestPoint] Trigger détecté avec : " + collider.name);

        if (!collider.CompareTag("Player"))
        {
            Debug.Log("[TestPoint] Ignoré : pas le Player");
            return;
        }

        if (triggered)
        {
            Debug.Log("[TestPoint] Déjà déclenché, on ignore");
            return;
        }

        Debug.Log("[TestPoint] Player détecté, déclenchement !");

        triggered = true;

        if (progressionlocal == null)
        {
            Debug.LogError("[TestPoint] ERREUR : progressionlocal est NULL");
            return;
        }

        progressionlocal.AddProgress();
        Debug.Log("[TestPoint] Progression incrémentée : " + progressionlocal.prog);

        if (ButtonHUB != null)
        {
            ButtonHUB.SetActive(false);
            Debug.Log("[TestPoint] ButtonHUB désactivé");
        }
        else
        {
            Debug.LogWarning("[TestPoint] ButtonHUB est NULL");
        }

        Debug.Log("[TestPoint] Niveau actuel : " + progressionlocal.CurrentLevel);

        // Lancer le niveau suivant
        progressionlocal.LaunchCurrentLevel();
        Debug.Log("[TestPoint] LaunchCurrentLevel appelé");

        // Désactive uniquement le TestPoint lui-même
        gameObject.SetActive(false);
        Debug.Log("[TestPoint] Objet désactivé");
    }
}