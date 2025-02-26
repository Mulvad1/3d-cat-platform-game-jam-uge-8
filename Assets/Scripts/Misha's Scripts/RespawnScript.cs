using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{    
    public GameObject respawnPoint;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null) //if controller exists, turn it off, set its position to respawnPoint, turn controller back on
            {
                controller.enabled = false;
                other.transform.position = respawnPoint.transform.position;
                controller.enabled = true;
            }
            else
            {
                other.transform.position = respawnPoint.transform.position;
            }
            Debug.Log("Player Detected");
        }
    }
}
