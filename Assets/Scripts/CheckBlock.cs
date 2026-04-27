using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    [Header("Block")]
    [SerializeField] private GameObject Block;
    [SerializeField] private float t;

    [Header("Props")]
    [SerializeField] private GameObject BadPropsHere1;
    [SerializeField] private GameObject BadPropsHere2;
    [SerializeField] private GameObject BadPropsHere3;
    [SerializeField] private GameObject BadPropsHere4;
    [SerializeField] private GameObject BadPropsHere5;

    [Header("Audio")]
    [SerializeField] private GameObject AudioNorm;
    [SerializeField] private GameObject AudiDistordu1;
    [SerializeField] private GameObject AudiDistordu2;

    private float startTime;
    private bool wasBlockActive;

    void Update()
    {

        bool isActive = Block.activeInHierarchy;

        if (isActive)
        {
            if (!wasBlockActive)
            {
                startTime = Time.time;
                wasBlockActive = true;
                Debug.Log("Block activé !");
            }

            t = Time.time - startTime;

            HandleSequence();
        }
        else
        {
            if (wasBlockActive)
            {
                ResetProps();
                wasBlockActive = false;
            }
        }
    }

    void HandleSequence()
    {
        if (t > 0f && !BadPropsHere1.activeSelf)
        {
            BadPropsHere1.SetActive(true);
            Debug.Log("Commence");
        }

        if (t > 5f && !BadPropsHere2.activeSelf)
        {
            BadPropsHere2.SetActive(true);
        }

        if (t > 15f && !AudiDistordu1.activeSelf)
        {
            AudioNorm.SetActive(false);
            AudiDistordu1.SetActive(true);
            BadPropsHere3.SetActive(true);
            Debug.Log("Distorsion1");
            
        }

        if (t > 20f && !BadPropsHere4.activeSelf)
        {
            BadPropsHere4.SetActive(true);
        }

        if (t > 25f && !AudiDistordu2.activeSelf)
        {
            AudiDistordu1.SetActive(false);
            AudiDistordu2.SetActive(true);
            BadPropsHere5.SetActive(true);
            Debug.Log("Distorsion2");
        }
    }

    void ResetProps()
    {
        AudioNorm.SetActive(false);
        AudiDistordu1.SetActive(false);
        AudiDistordu2.SetActive(false);

        BadPropsHere1.SetActive(false);
        BadPropsHere2.SetActive(false);
        BadPropsHere3.SetActive(false);
        BadPropsHere4.SetActive(false);
        BadPropsHere5.SetActive(false);
    }
}

