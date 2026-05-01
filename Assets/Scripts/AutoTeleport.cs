using UnityEngine;
using System.Collections;

public class AutoTeleport : MonoBehaviour
{
    [SerializeField] private Transform TeleportPoint;
    [SerializeField] private Transform Player;
    [SerializeField] private Transition Transition;

    public void TeleportToHUB()
    {
        //if (transition == null || player == null || TeleportPoint == null)
        //{
            //Debug.LogWarning("Références manquantes !");
            //return;
        //}

        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        Debug.Log("yhdd");
    
        yield return Transition.Play(() =>
        {
            Player.position = TeleportPoint.position;
            Player.rotation = TeleportPoint.rotation;
        });
    }
}