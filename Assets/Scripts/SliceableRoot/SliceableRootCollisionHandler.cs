using System;
using UnityEngine;

public class SliceableRootCollisionHandler: MonoBehaviour
{
    public event Action OnKnifeEnter;
    public event Action OnKnifeExit;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Knife"))
        {
            OnKnifeEnter?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Knife"))
        {
            OnKnifeExit?.Invoke();
        }
    }
}
