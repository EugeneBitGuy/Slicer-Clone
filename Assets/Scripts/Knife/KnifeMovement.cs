using AbstractClasses;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Knife
{
    public sealed class KnifeMovement : BaseMovement
    {
        private const float UpMovementSpeedFactor = 10f;

        [SerializeField] private InputAction actions;

        [SerializeField] private float rotationSpeed = 90f;
    
        protected override void OnEnable()
        {
            base.OnEnable();

            actions.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            actions.Disable();
        }

        private void Update()
        {
            if (GameState == GameState.NotStarted) return;

            ProcessMovement();
        }

        protected override void ProcessMovement()
        {
            bool isButtonPressed = actions.inProgress && GameState != GameState.Finished;
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