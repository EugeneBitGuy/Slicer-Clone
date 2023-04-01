using AbstractClasses;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Knife
{
    public sealed class KnifeMovement : BaseMovement
    {
        private const float UpMovementSpeedFactor = 25f;
        
        [Tooltip("Speed of rotation")]
        [SerializeField] private float rotationSpeed = 90f;

        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.NotStarted) return;

            ProcessMovement();
        }
        /// <summary>
        /// Computes movement in current frame for Knife
        /// </summary>
        protected override void ProcessMovement()
        {
            bool isButtonPressed = InputManager.Instance.KnifeMoving 
                                   && GameManager.Instance.GameState != GameState.Finished;
            
            var targetPosition = isButtonPressed ? endTransform.position : startTransform.position;
            var targetRotation = isButtonPressed ? endTransform.rotation : startTransform.rotation;
            var targetMovementSpeed = isButtonPressed ? movementSpeed : UpMovementSpeedFactor * movementSpeed;
            var targetRotationSpeed = isButtonPressed ? rotationSpeed : UpMovementSpeedFactor * rotationSpeed;


            transform.position = Vector3.MoveTowards
            (
                transform.position,
                targetPosition,
                targetMovementSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.RotateTowards
            (
                transform.rotation,
                targetRotation,
                targetRotationSpeed * Time.deltaTime
            );
        }
    }
}