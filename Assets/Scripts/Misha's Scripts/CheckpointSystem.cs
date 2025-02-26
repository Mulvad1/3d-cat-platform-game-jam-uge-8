using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    private RespawnScript respawn;
    private void Awake()
    {
        respawn = GameObject.FindGameObjectWithTag("Respawn").GetComponent<RespawnScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) //sets respawnPoint to the last Checkpoint, Player standed on
        {
            respawn.respawnPoint = this.gameObject;
        }
    }
}
