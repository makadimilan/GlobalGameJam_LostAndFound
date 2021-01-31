using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Grabable : MonoBehaviour
{
    public UnityEvent OnGrabStartedEvent;
    public UnityEvent OnGrabEndedEvent;
    HashSet<MonoBehaviour> grabbedBy = new HashSet<MonoBehaviour>();

    public void StartGrab(MonoBehaviour GrabberBehaviour) 
    {
        if (grabbedBy.Count == 0)
        {
            OnGrabStartedEvent.Invoke();
            OnGrabStarted();
        }

        grabbedBy.Add(GrabberBehaviour);
    }

    public void StopGrab(MonoBehaviour GrabberBehaviour) 
    {
        grabbedBy.Remove(GrabberBehaviour);

        if (grabbedBy.Count == 0)
        {
            OnGrabEndedEvent.Invoke();
            OnGrabEnded();
        }
    }

    protected virtual void OnGrabStarted() {}
    protected virtual void OnGrabEnded() {}
}
