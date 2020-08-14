using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausController : MonoBehaviour
{
    private float defaultTimeScale;
    private float defaultFixedDeltaTime;

    void Start()
    {
        defaultTimeScale = Time.timeScale;
        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    public void EnableSlowmotion(float modifier)
    {
        Debug.Log("Enabling slowmotion");
        Time.timeScale = modifier;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    public void DisableSlowmotion()
    {
        Time.timeScale = defaultTimeScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }
}
