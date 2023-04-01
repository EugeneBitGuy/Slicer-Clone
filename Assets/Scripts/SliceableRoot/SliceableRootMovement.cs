using System;
using AbstractClasses;
using Managers;
using UnityEngine;

namespace SliceableRoot
{
    public sealed class SliceableRootMovement : BaseMovement
    {
        private SliceableRootCollisionHandler _collisionHandler;

        private bool _canMove;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_collisionHandler == null)
                _collisionHandler = GetComponent<SliceableRootCollisionHandler>();

            if (_collisionHandler == null)
                throw new Exception("Movement of SliceableRoot cannot exist without collision handler");
                
            _collisionHandler.OnKnifeEnter += ForbidMovement;
            _collisionHandler.OnKnifeExit += AllowMovement;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _collisionHandler.OnKnifeEnter -= ForbidMovement;
            _collisionHandler.OnKnifeExit -= AllowMovement;
        }

        protected override void Start()
        {
            base.Start();

            _canMove = true;
        }

        private void Update()
        {
            if (GameState == GameState.NotStarted || GameState == GameState.SlicingEvent) return;

            if (_canMove)
                ProcessMovement();
        }

        private void AllowMovement() => _canMove = true;
        private void ForbidMovement() => _canMove = false;

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
                _canMove = false;
                GameManager.Instance.ChangeGameState(GameState.Finished);
            }
        }
    }
}