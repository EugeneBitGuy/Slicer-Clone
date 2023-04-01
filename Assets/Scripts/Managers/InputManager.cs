using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

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

        public void ReadKnifeMovingInput(InputAction.CallbackContext context)
        {
            _knifeMoving = !context.canceled;
        }
    }
}
