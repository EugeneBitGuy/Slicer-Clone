using System;
using AbstractClasses;
using Managers;
using UnityEngine;

namespace SliceableRoot
{
    public sealed class SliceableRootMovement : BaseMovement
    { 
        private void Update()
        {
            if (GameManager.Instance.GameState == GameState.NotStarted ||
                GameManager.Instance.GameState == GameState.SlicingEvent || 
                InputManager.Instance.KnifeMoving) return;

            ProcessMovement();
        }

        /// <summary>
        /// Computes movement in current frame for SlicableRoot
        /// </summary>
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