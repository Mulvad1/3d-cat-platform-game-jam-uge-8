using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour
{
    public Text timerText;
    private float elapsedTime;
    private bool isRunning;

    void Start()
    {
        elapsedTime = 0f;
        isRunning = false;
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            timerText.text = elapsedTime.ToString("F2"); // Show time with 2 decimals
        }
    }

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public float GetElapsedTime() => elapsedTime;
}