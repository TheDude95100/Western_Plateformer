using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class InputDisplay : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        HookManager.Instance.OnHookPointDetected += HookManager_OnHookPointDetected;
        HookManager.Instance.OnHookPointLeft += HookManager_OnHookPointLeft;
    }

    private void Update()
    {
        transform.localScale = transform.parent.localScale;
    }
    void HookManager_OnHookPointDetected(object sender, EventArgs e)
    {
        text.gameObject.SetActive(true);
    }
    void HookManager_OnHookPointLeft(object sender, EventArgs e)
    {
        text.gameObject.SetActive(false);
    }

}
