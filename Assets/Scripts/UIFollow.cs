using UnityEngine;

public class UIFollow : MonoBehaviour
{
    [SerializeField] private Transform Head; // Main Camera
    [SerializeField] private Transform UI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        UI.LookAt(Head);
        UI.Rotate(0, 180, 0); // corrige l’inversion si nécessaire
    }
}
