using System;
using UnityEngine;
using System.Collections;

public class ChatApparition : MonoBehaviour
{
    private bool Dialogue = false;

    public GameObject Chat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player") && !Dialogue)
        {
            Dialogue = true;
            StartCoroutine(SpawnChat());
        }
    }

    private IEnumerator SpawnChat()
    {
        yield return new WaitForSecondsRealtime(2f);
        Debug.Log("?");
        Chat.SetActive(true);
    }
}
