using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HookManager : MonoBehaviour
{
    private List<HookPoint> hookPoints;

    public static HookManager Instance;

    public event EventHandler OnHookPointDetected;
    public event EventHandler OnHookPointLeft;

    private void Awake()
    {
        Instance = this;
        hookPoints = new List<HookPoint>(FindObjectsOfType<HookPoint>());
        foreach (HookPoint point in hookPoints)
        {
            point.OnPlayerDetected += HookPoint_OnPlayerDetected;
            point.OnPlayerLeft += HookPoint_OnPlayerLeft;
        }
        Debug.Log(hookPoints.Count);
    }

    void HookPoint_OnPlayerDetected(object sender, EventArgs e)
    {
        OnHookPointDetected?.Invoke(this, EventArgs.Empty);
    }

    void HookPoint_OnPlayerLeft(object sender, EventArgs e)
    {
        OnHookPointLeft?.Invoke(this, EventArgs.Empty);
    }
}
