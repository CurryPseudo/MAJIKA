﻿using UnityEngine;
using System.Linq;
using System.Collections;

public class PageContainer : MonoBehaviour
{
    private void Start()
    {
        Reset();
    }

    public void Show(UIPage page)
    {
        GetComponentsInChildren<UIPage>(true).ForEach(p => p.gameObject.SetActive(false));
        page.gameObject.SetActive(true);
    }

    public void PageNext()
    {
    }

    public void PageBack()
    {

    }

    public void Reset()
    {
        GetComponentsInChildren<UIPage>(true)
            .ForEach(p => p.gameObject.SetActive(false));

        GetComponentsInChildren<UIPage>(true)
            .Where(p => p.IsDefault)
            .FirstOrDefault()
            ?.gameObject.SetActive(true);
    }
}