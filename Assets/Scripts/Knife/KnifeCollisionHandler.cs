using System;
using UnityEngine;

namespace Knife
{
    public class KnifeCollisionHandler : MonoBehaviour
    {
        /// <summary>
        /// Triggers if knife entered SliceableRoot 
        /// </summary>
        public event Action<GameObject> OnRootEnter;
        
        /// <summary>
        /// Trigger if knife entered destination point
        /// </summary>
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