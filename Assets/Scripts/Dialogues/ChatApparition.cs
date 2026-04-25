using System;
using UnityEngine;
using System.Collections;

public class ChatApparition : MonoBehaviour
{
    private bool Dialogue = false;
    private bool Button = false;

    public GameObject Chat;
    public GameObject ButtonBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && !Dialogue && !Button)
        {
            Dialogue = true;
            Button = true;
            StartCoroutine(SpawnChat());
            StartCoroutine(SpawnBloque());
        }
    }

    private IEnumerator SpawnChat()
    {
        yield return new WaitForSecondsRealtime(3f);
        Chat.SetActive(true);
    }

        private IEnumerator SpawnBloque()
    {
        yield return new WaitForSecondsRealtime(5f);
        ButtonBlock.SetActive(true);
    }

    
}
