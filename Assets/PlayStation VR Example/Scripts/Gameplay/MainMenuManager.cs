using UnityEngine;
using UnityEngine.UI;
#if UNITY_PS4
using UnityEngine.PS4;
using UnityEngine.PS4.VR;
#endif

namespace PlayStationVRExample
{
    public class MainMenuManager : MonoBehaviour
    {
        public Text ds4StatusText;
        public Slider ds4Slider;
        public Text moveStatusText;
        public Slider moveSlider;
        public Text aimStatusText;
        public Slider aimSlider;
        public Text renderScaleText;
        public Slider renderScaleSlider;
        public Text socialScreenText;
        public Slider socialScreenSlider;
        public Text minOutputColorText;
        public Slider minOutputColorSlider;

        void Start()
        {
            socialScreenText.text = string.Format("<b>Social Screen</b>: {0}", UnityEngine.XR.XRSettings.showDeviceView ? "Disabled" : "Enabled");
            renderScaleText.text = string.Format("<b>Renderscale</b>: {0:F1}", UnityEngine.XR.XRSettings.eyeTextureResolutionScale);
#if UNITY_PS4
            minOutputColorText.text = string.Format("<b>Minimum Output Color</b>: {0}%", PlayStationVRSettings.minOutputColor.maxColorComponent * 10);
#endif
        }

        void Update()
        {
            DualShock4Check();
            MoveCheck();
            AimCheck();
        }

        void DualShock4Check()
        {
#if UNITY_PS4
            if (PS4Input.PadIsConnected(0))
            {
                ds4StatusText.text = "<b>Status: Ready!</b>";
                ds4Slider.interactable = true;
            }
            else
            {
                ds4StatusText.text = "<b>Status: Error!</b>\n\nWireless controller not detected!";
                ds4Slider.interactable = false;
            }
#endif
        }

        void MoveCheck()
        {
            var connectedControllers = 0;

#if UNITY_PS4
            if (PS4Input.MoveIsConnected(0, 0))
                connectedControllers++;
            if (PS4Input.MoveIsConnected(0, 1))
                connectedControllers++;
#endif

            if (connectedControllers == 2)
            {
                moveStatusText.text = "<b>Status: Ready!</b>";
                moveSlider.interactable = true;
            }
            else
            {
                moveStatusText.text = string.Format("<b>Status: Error!</b>\n\n{0} motion controllers detected! 2 are required.", connectedControllers);
                moveSlider.interactable = false;
            }
        }

        void AimCheck()
        {
#if UNITY_PS4
            if (PS4Input.AimIsConnected(0))
            {
                aimStatusText.text = "<b>Status: Ready!</b>";
                aimSlider.interactable = true;
            }
            else
            {
                aimStatusText.text = "<b>Status: Error!</b>\n\nAim Controller not detected!";
                aimSlider.interactable = false;
            }
#endif
        }

        // Shooter Scene controlled by a DualShock 4 controller
        public void GoToDualShock4Scene()
        {
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_DualShock4");
        }

        // Shooter Scene controlled by the Move controllers
        public void GoToMoveScene()
        {
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MoveControllers");
        }

        // Shooter Scene controlled by an aim controller
        public void GoToAimScene()
        {
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_AimController");
        }

        // Toggle social screen on/off
        public void OptionsToggleShowHmdView()
        {
            VRManager.instance.ToggleHmdViewOnMonitor();

            socialScreenText.text = string.Format("<b>Social Screen</b>: {0}", UnityEngine.XR.XRSettings.showDeviceView ? "Disabled" : "Enabled");
            socialScreenSlider.value = 0;
        }

        // As an example, change the renderscale between low and recommended levels
        public void SwitchRenderScale()
        {
            VRManager.instance.ChangeRenderScale(UnityEngine.XR.XRSettings.eyeTextureResolutionScale == 1.4f ? 0.7f : 1.4f);

            renderScaleText.text = string.Format("<b>Renderscale</b>: {0:F1}", UnityEngine.XR.XRSettings.eyeTextureResolutionScale);
            renderScaleSlider.value = 0;
        }

        void OptionsToggleMinimumOutputColor()
        {
#if UNITY_PS4
            if (FindObjectOfType<VRPostReprojection>())
            {
                if (PlayStationVRSettings.minOutputColor.maxColorComponent == 0.1f)
                    PlayStationVRSettings.minOutputColor = Color.black;
                else
                    PlayStationVRSettings.minOutputColor = Color.white * 0.1f;

                minOutputColorText.text = string.Format("<b>Minimum Output Color</b>: {0}%", PlayStationVRSettings.minOutputColor.maxColorComponent * 10);
                minOutputColorSlider.value = 0;
            }
#endif
        }
    }
}
