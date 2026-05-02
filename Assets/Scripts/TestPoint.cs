using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public GameFlow progressionlocal;
    public GameObject ButtonHUB;

    private bool triggered = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !triggered)
        {
            triggered = true;

            progressionlocal.prog += 1;

            if (ButtonHUB != null)
            {
                ButtonHUB.SetActive(false);
            }
            
            Debug.Log("TestPoint déclenché — lancement du niveau : " + progressionlocal.CurrentLevel);
            
            // Lancer le niveau suivant
            progressionlocal.LaunchCurrentLevel();
            
            // Désactive uniquement le TestPoint lui-même
            gameObject.SetActive(false);

            
        }
    }
}
