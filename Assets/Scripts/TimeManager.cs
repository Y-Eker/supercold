using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    [SerializeField] float timeSlowed;
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

    public void SpeedUpLerp(float speedAmount, float speedDuration)
    {

    }

    public void SpeedUpInstant(float speedAmount, float speedDuration)
    {
        Time.timeScale = speedAmount;
        Invoke("SlowDownSpeed", speedDuration);
    }

    void SlowDownSpeed()
    {
        Time.timeScale = timeSlowed;
    }

    private void Update()
    {
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
