using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public GameFlow progressionlocal;
    public GameObject Moi;
    private bool triggered = false;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !triggered)
        {
            triggered = true;

            Debug.Log("TestPoint déclenché — lancement du niveau : " + progressionlocal.CurrentLevel);

            Moi.SetActive(false);

            // 🔥 LANCER LE NIVEAU SUIVANT
            progressionlocal.LaunchCurrentLevel();
        }
    }
}
