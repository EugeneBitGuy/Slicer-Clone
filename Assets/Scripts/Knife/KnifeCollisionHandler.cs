using System;
using UnityEngine;

namespace Knife
{
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
}