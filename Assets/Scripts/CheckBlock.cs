using UnityEngine;

public class CheckBlock : MonoBehaviour
{
    [Header("Block")]
    [SerializeField] private GameObject Block;
    [SerializeField] private float t;

    [Header("DepopProps")]
    [SerializeField] private GameObject[] BadPropsHere1;
    [SerializeField] private GameObject[] BadPropsHere2;
    [SerializeField] private GameObject[] BadPropsHere3;
    [SerializeField] private GameObject[] BadPropsHere4;
    [SerializeField] private GameObject[] BadPropsHere5;

    [Header("Props")]
    [SerializeField] private GameObject[] BadPropsHere6;
    [SerializeField] private GameObject[] BadPropsHere7;
    [SerializeField] private GameObject[] BadPropsHere8;
    [SerializeField] private GameObject[] BadPropsHere9;
    [SerializeField] private GameObject[] BadPropsHere10;

    [Header("Audio")]
    [SerializeField] private GameObject AudioNorm;
    [SerializeField] private GameObject AudiDistordu1;
    [SerializeField] private GameObject AudiDistordu2;

    [SerializeField] private Renderer sphereRenderer;
    private Material mat;

    private float startTime;
    private bool wasBlockActive;

    // Step control
    private bool step1Done, step2Done, step3Done, step4Done, step5Done, step6Done;

    void Start()
    {
        // Sécurité
        if (sphereRenderer != null)
            mat = sphereRenderer.material;
    }

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
        if (mat != null)
        {
            float transitionValue = Mathf.Clamp01(Mathf.InverseLerp(40f, 65f, t)); //Fait augmenter progressivement la _transition
            mat.SetFloat("_Transition", transitionValue);
        }

        if (t > 40f && !step1Done)
        {
            SetActiveArray(BadPropsHere1, false);
            SetActiveArray(BadPropsHere6, true);
            step1Done = true;
            Debug.Log("Commence");
        }

        if (t > 50f && !step2Done)
        {
            if (AudioNorm != null) AudioNorm.SetActive(false);
            if (AudiDistordu1 != null) AudiDistordu1.SetActive(true);
            SetActiveArray(BadPropsHere2, false);
            SetActiveArray(BadPropsHere7, true);
            step2Done = true;
            Debug.Log("Distorsion1");
        }

        if (t > 55f && !step3Done)
        {
            SetActiveArray(BadPropsHere3, false);
            SetActiveArray(BadPropsHere8, true);
            Debug.Log("Distorsion2");

            step3Done = true;
        }

        if (t > 60f && !step4Done)
        {
            SetActiveArray(BadPropsHere4, false);
            SetActiveArray(BadPropsHere9, true);
            Debug.Log("Distorsion3");

            step4Done = true;
        }

        if (t > 65f && !step5Done)
        {
            SetActiveArray(BadPropsHere5, false);
            SetActiveArray(BadPropsHere10, true);

            step5Done = true;
            Debug.Log("Distorsion4");
        }

        if (t > 120f && !step6Done)
        {
            if (AudiDistordu1 != null) AudiDistordu1.SetActive(false);
            if (AudiDistordu2 != null) AudiDistordu2.SetActive(true);

            step6Done = true;
            Debug.Log("Distorsion5");
        }
    }

    void ResetProps()
    {
        if (AudioNorm != null) AudioNorm.SetActive(false);
        if (AudiDistordu1 != null) AudiDistordu1.SetActive(false);
        if (AudiDistordu2 != null) AudiDistordu2.SetActive(false);

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

        if (mat != null)
        {
            mat.SetFloat("_Transition", 0f);
        }

        //Reset steps
        step1Done = false;
        step2Done = false;
        step3Done = false;
        step4Done = false;
        step5Done = false;
        step6Done = false;
    }

    void SetActiveArray(GameObject[] array, bool state)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null)
            {
                array[i].SetActive(state);
            }
        }
    }
}

