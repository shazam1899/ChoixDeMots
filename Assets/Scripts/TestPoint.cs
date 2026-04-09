using UnityEngine;

public class TestPoint : MonoBehaviour
{
    public GameFlow progressionlocal;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player") && !triggered)
        {
            triggered = true;
            progressionlocal.progression += 1;
        }
    }
}
