using UnityEngine;

namespace PlayStationVRExample
{
    public class InputVisualiser : MonoBehaviour
    {
        // Custom class for holding all the gamepad sprites
        [System.Serializable]
        public class Controller
        {
            public Transform buttonCross;
            public Transform buttonCircle;
            public Transform buttonSquare;
            public Transform buttonTriangle;

            public Transform buttonOptions;
            public Transform buttonTouchpad;

            public Transform dpadDown;
            public Transform dpadRight;
            public Transform dpadUp;
            public Transform dpadLeft;

            public Transform buttonL1;
            public Transform buttonR1;
            public Transform buttonR12; // Aim controller only
            public Transform triggerL2;
            public Transform triggerR2;
            public Transform thumbstickL3;
            public Transform thumbstickR3;
        }

        public Controller controller;
        public float buttonPressDistance;
        public float triggerPullAngle;
        public float thumbstickAngle;

        void Update()
        {
            controller.buttonCross.localPosition = Input.GetButton("Fire1") ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.buttonCircle.localPosition = Input.GetButton("Fire2") ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.buttonSquare.localPosition = Input.GetButton("Fire3") ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.buttonTriangle.localPosition = Input.GetButton("Fire4") ? Vector3.up * buttonPressDistance : Vector3.zero;

            controller.buttonOptions.localPosition = Input.GetButton("Aux4") ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.buttonTouchpad.localPosition = Input.GetButton("Aux3") ? Vector3.up * buttonPressDistance : Vector3.zero;

            controller.dpadRight.localPosition = Input.GetAxisRaw("DPADHorizontal") > 0 ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.dpadLeft.localPosition = Input.GetAxisRaw("DPADHorizontal") < 0 ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.dpadUp.localPosition = Input.GetAxisRaw("DPADVertical") > 0 ? Vector3.up * buttonPressDistance : Vector3.zero;
            controller.dpadDown.localPosition = Input.GetAxisRaw("DPADVertical") < 0 ? Vector3.up * buttonPressDistance : Vector3.zero;

            controller.buttonL1.localPosition = Input.GetButton("ShoulderLeft") ? -Vector3.forward * buttonPressDistance : Vector3.zero;
            controller.buttonR1.localPosition = Input.GetButton("ShoulderRight") ? Vector3.left * buttonPressDistance : Vector3.zero;
            if (controller.buttonR12 != null)
                controller.buttonR12.localPosition = Input.GetButton("ShoulderRight") ? Vector3.left * buttonPressDistance : Vector3.zero;

            controller.triggerL2.localRotation = Quaternion.Euler(new Vector3(Input.GetAxisRaw("TriggerLeft") * triggerPullAngle, 0, 0));
            controller.triggerR2.localRotation = Quaternion.Euler(new Vector3(-Input.GetAxisRaw("TriggerRight") * triggerPullAngle, 0, 0));

            controller.thumbstickL3.localRotation = Quaternion.Euler(new Vector3(Input.GetAxisRaw("Vertical") * thumbstickAngle, 0, -Input.GetAxisRaw("Horizontal") * thumbstickAngle));
            controller.thumbstickR3.localRotation = Quaternion.Euler(new Vector3(Input.GetAxisRaw("Vertical2") * thumbstickAngle, 0, -Input.GetAxisRaw("Horizontal2") * thumbstickAngle));
        }
    }
}