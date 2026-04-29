using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public GameFlow progressionlocal;
    private bool triggered = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !triggered)
        {
            triggered = true;

            Debug.Log("TestPoint déclenché — lancement du niveau : " + progressionlocal.CurrentLevel);

            // Désactive uniquement le TestPoint lui-même
            gameObject.SetActive(false);

            // Lancer le niveau suivant
            progressionlocal.LaunchCurrentLevel();
        }
    }
}
