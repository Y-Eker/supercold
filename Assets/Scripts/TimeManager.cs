using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float timeVal;
    public float timeSlowed;
    private Coroutine slowdown, speedUpLerpRoutine;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
        Time.timeScale = timeSlowed;
    }

    public void SpeedUpInstant(float speedAmount, float speedDuration)
    {
        Time.timeScale = speedAmount;
        if (slowdown != null)
        {
            StopCoroutine(slowdown);
        }
        slowdown = StartCoroutine(SlowDownRoutine(speedDuration));
    }

    IEnumerator SlowDownRoutine(float duration)
    {
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.unscaledDeltaTime;
            yield return null; 
        }
        Time.timeScale = timeSlowed;
    }

    private void Update()
    {
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        timeVal = Time.timeScale;
    }
}
