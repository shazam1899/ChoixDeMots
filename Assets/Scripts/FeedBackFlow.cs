using UnityEngine;

public class FeedBackFlow : MonoBehaviour
{
    public GameObject ButtonINT;
    public GameObject ButtonZone;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            insideCount++;

            if (insideCount == 1)
            {
                Debug.Log("bb");
                foreach (var obj in snapVolume)
                    obj?.SetActive(false);
            }
        }
    }

}
