using UnityEngine;

public class ChatApparition : MonoBehaviour
{
    private bool Dialogue = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && !Dialogue)
        {
            WaitForSeconds(3f);
            Debug.Log("?");
            //DialogueApparition.Enable
            Dialogue = true;
        }
    }
}
