using UnityEngine;
using System.Collections;

public class AutoTeleport : MonoBehaviour
{
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private Transform Player;
    [SerializeField] private Transform Head; // Main Camera
    [SerializeField] private Transform TeleportUI;
    [SerializeField] private Transition Transition;
    [SerializeField] private float distance = 10f;

    private void Start()
    {
        distance = Vector3.Distance(Head.position, TeleportUI.position);
    }

    public void TeleportToHUB()
    {
        if (Transition == null || Player == null || TeleportPoint == null || Head == null)
        {
            Debug.LogWarning("Références manquantes !");
            return;
        }

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        Debug.Log("yhdd");
    
        yield return Transition.Play(() =>
        {
            Player.position = TeleportPoint.position;
            Player.rotation = TeleportPoint.rotation;

            Vector3 forward = Head.forward;
            forward.y = 0;
            forward.Normalize();

            TeleportUI.position = Head.position + forward * distance;

            TeleportUI.LookAt(Head);
            TeleportUI.Rotate(0, 180, 0); // sinon inversé
        });
    }
}