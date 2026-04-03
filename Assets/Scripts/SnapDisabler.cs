using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SnapDisabler : MonoBehaviour
{
    public GameObject SnapVolume;
    public GameObject SnapVolume1;
    public GameObject SnapVolume2;

    private void Awake()
    {
        SnapVolume.SetActive(true);
        SnapVolume1.SetActive(true);
        SnapVolume2.SetActive(true);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Hiiiii");
            SnapVolume.SetActive(false);
            SnapVolume1.SetActive(false);
            SnapVolume2.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Byeeee");
            SnapVolume.SetActive(true);
            SnapVolume1.SetActive(true);
            SnapVolume2.SetActive(true);
        }
    }
}
