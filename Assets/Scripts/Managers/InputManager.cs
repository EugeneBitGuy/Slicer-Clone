using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Gets tge singletone instance of InputManager
        /// </summary>
        public static InputManager Instance { get; private set; }

        /// <summary>
        /// Gets the value indicating whether knife should move
        /// </summary>
        public bool KnifeMoving => _knifeMoving;

        private bool _knifeMoving;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Reading value of Knife Moving from Input Actions
        /// </summary>
        /// <param name="context">Context of reading value of Knife Moving</param>
        public void ReadKnifeMovingInput(InputAction.CallbackContext context)
        {
            _knifeMoving = !context.canceled;
        }
    }
}
