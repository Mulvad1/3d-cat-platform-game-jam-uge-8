using UnityEngine;

public class TimeTrialTriggers : MonoBehaviour
{
    public TimeTrial trialManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("StartLine"))
                trialManager.StartTimer();

            if (gameObject.CompareTag("FinishLine"))
                trialManager.StopTimer();
        }
    }
}
