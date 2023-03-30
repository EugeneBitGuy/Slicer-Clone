using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeSlicer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.Slice(other.gameObject);
    }
    
}
