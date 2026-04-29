using UnityEngine;

public class FeedbackProg : MonoBehaviour
{
    [SerializeField] private GameObject AnnonceHUB;
    [SerializeField] private GameObject AnnonceIntermediaire;


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            AnnonceHUB.SetActive(false);
            AnnonceIntermediaire.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            AnnonceIntermediaire.SetActive(false);
            AnnonceHUB.SetActive(true);
        }
    }
}
