using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AtomicTimer : MonoBehaviour
{
    public float runEvery = 5;
    public bool runOnAwake = false;
    public bool loop = true;
    public UnityEvent OnTimerEnd;

    private void OnEnable()
    {
        if (runOnAwake)
        {
            Invoke("Execute", 0);
        }

        if (loop)
        {
            InvokeRepeating("Execute", runEvery, runEvery);
        }
        else
        {
            Invoke("Execute", runEvery);
        }
    }

    public void Execute()
    {
        OnTimerEnd.Invoke();
    }
}