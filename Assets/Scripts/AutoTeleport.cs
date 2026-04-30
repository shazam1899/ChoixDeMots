using UnityEngine;

public class AutoTeleport : MonoBehaviour
{
    [SerializeField] private GameObject TeleportPoint;
    [SerializeField] private Transform player;

    public void TeleportToHUB()
    {
        if (TeleportPoint == null || player == null)
        {
            return;
        }

        player.position = TeleportPoint.transform.position;
        player.rotation = TeleportPoint.transform.rotation;
    }
}