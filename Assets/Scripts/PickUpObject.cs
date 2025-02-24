using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Transform holdPosition; // Empty GameObject above player's head
    public float pickupRange = 2.5f; // Adjust the distance for pickup detection
    public KeyCode pickupKey = KeyCode.E; // Key to pick up/drop objects

    public float dropForwardForce;
    //public float throwForce;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
            if (heldObject == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryPickup()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Pickup")) // Make sure object has the "Pickup" tag
            {
                heldObject = col.gameObject;
                heldObjectRb = heldObject.GetComponent<Rigidbody>();

                if (heldObjectRb)
                {
                    heldObjectRb.isKinematic = true; // Stop physics interactions
                }

                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.SetParent(holdPosition); // Attach to player
                return;
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            if (heldObjectRb)
            {
                heldObjectRb.isKinematic = false; // Restore physics
                //heldObjectRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }
            heldObject = null;

            heldObjectRb.AddForce(holdPosition.forward * dropForwardForce, ForceMode.Impulse);

            float random = Random.Range(-1f, 1f);

            heldObjectRb.AddTorque(new Vector3(random, random, random) * 10);
        }
    }
}