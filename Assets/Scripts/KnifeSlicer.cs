using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSlicer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SliceableRoot"))
            GameManager.Instance.Slice(other.gameObject, transform);
    }
    
}
