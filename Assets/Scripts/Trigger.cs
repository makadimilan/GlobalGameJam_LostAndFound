using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    [SerializeField] string TagFilter = string.Empty;

    public UnityEvent OnTriggerEnter2DEvent;
    public UnityEvent OnTriggerExit2DEvent;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (string.IsNullOrWhiteSpace(TagFilter) || TagFilter == other.tag)
        {
            OnTriggerEnter2DEvent.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (string.IsNullOrWhiteSpace(TagFilter) || TagFilter == other.tag)
        {
            OnTriggerExit2DEvent.Invoke();
        }
    }
}
