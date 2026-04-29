using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    [Header("Block")]
    [SerializeField] private GameObject Block;
    [SerializeField] private float t;

    [Header("Props")]
    [SerializeField] private GameObject[] BadPropsHere1;
    [SerializeField] private GameObject[] BadPropsHere2;
    [SerializeField] private GameObject[] BadPropsHere3;
    [SerializeField] private GameObject[] BadPropsHere4;
    [SerializeField] private GameObject[] BadPropsHere5;
    [SerializeField] private GameObject[] BadPropsHere6;
    [SerializeField] private GameObject[] BadPropsHere7;
    [SerializeField] private GameObject[] BadPropsHere8;
    [SerializeField] private GameObject[] BadPropsHere9;
    [SerializeField] private GameObject[] BadPropsHere10;

    [Header("Audio")]
    [SerializeField] private GameObject AudioNorm;
    [SerializeField] private GameObject AudiDistordu1;
    [SerializeField] private GameObject AudiDistordu2;

    private float startTime;
    private bool wasBlockActive;

    // Step control
    private bool step1Done, step2Done, step3Done, step4Done, step5Done;

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
        if (t > 0f && !step1Done)
        {
            SetActiveArray(BadPropsHere1, false);
            SetActiveArray(BadPropsHere6, true);
            step1Done = true;
            Debug.Log("Commence");
        }

        if (t > 5f && !step2Done)
        {
            SetActiveArray(BadPropsHere2, false);
            SetActiveArray(BadPropsHere7, true);
            step2Done = true;
        }

        if (t > 15f && !step3Done)
        {
            AudioNorm.SetActive(false);
            AudiDistordu1.SetActive(true);

            SetActiveArray(BadPropsHere3, false);
            SetActiveArray(BadPropsHere8, true);

            step3Done = true;
            Debug.Log("Distorsion1");
        }

        if (t > 20f && !step4Done)
        {
            SetActiveArray(BadPropsHere4, false);
            SetActiveArray(BadPropsHere9, true);
            step4Done = true;
        }

        if (t > 25f && !step5Done)
        {
            AudiDistordu1.SetActive(false);
            AudiDistordu2.SetActive(true);

            SetActiveArray(BadPropsHere5, false);
            SetActiveArray(BadPropsHere10, true);

            step5Done = true;
            Debug.Log("Distorsion2");
        }
    }

    void ResetProps()
    {
        AudioNorm.SetActive(false);
        AudiDistordu1.SetActive(false);
        AudiDistordu2.SetActive(false);

        SetActiveArray(BadPropsHere1, true);
        SetActiveArray(BadPropsHere6, false);

        SetActiveArray(BadPropsHere2, true);
        SetActiveArray(BadPropsHere7, false);

        SetActiveArray(BadPropsHere3, true);
        SetActiveArray(BadPropsHere8, false);

        SetActiveArray(BadPropsHere4, true);
        SetActiveArray(BadPropsHere9, false);

        SetActiveArray(BadPropsHere5, true);
        SetActiveArray(BadPropsHere10, false);

        //Reset steps
        step1Done = false;
        step2Done = false;
        step3Done = false;
        step4Done = false;
        step5Done = false;
    }

    void SetActiveArray(GameObject[] array, bool state)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
                array[i].SetActive(state);
        }
    }
}

