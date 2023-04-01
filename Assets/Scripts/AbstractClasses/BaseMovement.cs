using Managers;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class BaseMovement : MonoBehaviour
    {
        [Header("Points of movement clamping")]
        
        [Tooltip("Transform that will be applied to object on start")]
        [SerializeField] protected Transform startTransform;
        
        [Tooltip("Destination point of movement")]
        [SerializeField] protected Transform endTransform;

        [Header("Speed variables")]
        
        [Tooltip("Movement speed")]
        [SerializeField] protected float movementSpeed = 10f;
        
        protected virtual void Start()
        {
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
        }
        
        /// <summary>
        /// Computes movement on frame
        /// </summary>
        protected abstract void ProcessMovement();
    }
}
