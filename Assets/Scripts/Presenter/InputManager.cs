using UnityEngine;

namespace Presenter {
    public class InputManager : MonoBehaviour {
        InputSystem_Actions inputSystem;

        public Vector2 CursorScreenPosition { 
            get {
                Vector2 screenPosition = inputSystem.Player.CursorPosition.ReadValue<Vector2>();
                return screenPosition;
            } 
        }

        void Awake() {
            inputSystem = new InputSystem_Actions();
            inputSystem.Enable();
        }

        void OnDestroy() {
            inputSystem.Disable();
            inputSystem.Dispose();
        }
    }
}

