using UnityEngine;
using System.Collections;

public class CheckBlock : MonoBehaviour
{
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject BadPropsHere1;
    [SerializeField] private GameObject BadPropsHere2;
    [SerializeField] private GameObject BadPropsHere3;
    [SerializeField] private GameObject BadPropsHere4;
    [SerializeField] private GameObject BadPropsHere5;
    [SerializeField] private GameObject AudioNorm;
    [SerializeField] private GameObject AudiDistordu1;
    [SerializeField] private GameObject AudiDistordu2;
    private float startTime;
    private bool wasBlockActive = false;

    public float t;

    void Update()
    {
        Debug.Log(Block.activeSelf);
        
        if (Block.activeSelf && !wasBlockActive)
        {
            Debug.Log("Block activé !");
            startTime = Time.time;
            wasBlockActive = true;
        }

        if (Block.activeSelf)
        {
            t = Time.time - startTime;

            if (t > 0) 
            {
                BadPropsHere1.SetActive(true);
                Debug.Log("Commence");
            }

            if (t > 5) 
            {
                BadPropsHere2.SetActive(true);
            }

            if (t > 15)
            {
                AudioNorm.SetActive(false);
                AudiDistordu1.SetActive(true);
                Debug.Log("Distorsion1");
                BadPropsHere3.SetActive(true);
            }

            if (t > 20)
            {
                BadPropsHere4.SetActive(true);
            }

            if (t > 25)
            {
                AudiDistordu1.SetActive(false);
                AudiDistordu2.SetActive(true);
                Debug.Log("Distorsion2");
                BadPropsHere5.SetActive(true);
            }
        }
        else if (wasBlockActive)
        {
            ResetProps();
            wasBlockActive = false;
        }
    }

    void ResetProps()
    {
        AudiDistordu1.SetActive(false);
        AudiDistordu2.SetActive(false);
        AudioNorm.SetActive(false);
        BadPropsHere1.SetActive(false);
        BadPropsHere2.SetActive(false);
        BadPropsHere3.SetActive(false);
        BadPropsHere4.SetActive(false);
        BadPropsHere5.SetActive(false);
    }
}

