using System;
using UnityEngine;
using System.Collections;

public class ChatApparition : MonoBehaviour
{
    private bool Dialogue = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && !Dialogue)
        {
            WaitForSecondsRealtime(3f);
            Debug.Log("?");
            //DialogueApparition.Enable
            Dialogue = true;
        }
    }

    private void WaitForSecondsRealtime(float v)
    {
        throw new NotImplementedException();
    }
}
