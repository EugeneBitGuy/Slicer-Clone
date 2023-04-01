using System;
using AbstractClasses;
using Managers;
using UnityEngine;

namespace SliceableRoot
{
    public sealed class SliceableRootMovement : BaseMovement
    {
        private SliceableRootCollisionHandler _collisionHandler;


        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Start()
        {
            base.Start();
        }

        private void Update()
        {
            if (GameState == GameState.NotStarted || GameState == GameState.SlicingEvent || InputManager.Instance.KnifeMoving) return;

                ProcessMovement();
        }

        protected override void ProcessMovement()
        {
            transform.position = Vector3.MoveTowards
            (
                transform.position,
                endTransform.position,
                Time.deltaTime * movementSpeed
            );

            if (transform.position == endTransform.position)
            {
                GameManager.Instance.ChangeGameState(GameState.Finished);
            }
        }
    }
}