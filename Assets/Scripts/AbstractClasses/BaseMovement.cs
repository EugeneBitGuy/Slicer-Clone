using Managers;
using UnityEngine;

namespace AbstractClasses
{
    public abstract class BaseMovement : MonoBehaviour
    {
        [SerializeField] protected Transform startTransform;
        [SerializeField] protected Transform endTransform;

        [SerializeField] protected float movementSpeed = 10f;

        protected GameState GameState = GameState.NotStarted;

        protected virtual void OnEnable()
        {
            GameManager.Instance.OnGameStateChange += ChangeGameState;
        }

        protected virtual void OnDisable()
        {
            GameManager.Instance.OnGameStateChange -= ChangeGameState;
        }

        protected virtual void Start()
        {
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
        }

        protected void ChangeGameState(GameState newGameState) => GameState = newGameState;

        protected abstract void ProcessMovement();
    }
}
