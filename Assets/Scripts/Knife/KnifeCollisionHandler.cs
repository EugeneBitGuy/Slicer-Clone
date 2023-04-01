using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeCollisionHandler : MonoBehaviour
{
    public event Action<GameObject> OnRootEnter;
    public event Action OnEndPointEnter; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SliceableRoot"))
        {
            OnRootEnter?.Invoke(other.gameObject);
        }
        else if (other.gameObject.CompareTag("KnifeEndPoint"))
        {
            OnEndPointEnter?.Invoke();
        }
    }
    
}
