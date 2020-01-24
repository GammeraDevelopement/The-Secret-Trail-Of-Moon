using System;
using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4.VR;
using UnityEngine.PS4;
#endif

namespace PlayStationVRExample
{
    public class VRManager : MonoBehaviour
    {
        public float renderScale = 1.4f; // 1.4 is Sony's recommended scale for PlayStation VR
        public bool showHmdViewOnMonitor = true; // Set this to 'false' to use the monitor/display as the Social Screen
        public bool doSetupOnAwake = false; // Useful for development, will attempt to setup VR immediately
        static VRManager s_Instance;

        public static VRManager instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<VRManager>();
                    DontDestroyOnLoad(s_Instance.gameObject);
                }

                return s_Instance;
            }
        }

        void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad(this);
            }
            else if (this != s_Instance)
            {
                // There can be only one!
                Destroy(gameObject);
            }

            if (doSetupOnAwake)
                BeginVRSetup();
        }

        public void BeginVRSetup()
        {
            StartCoroutine(SetupVR());
        }

        IEnumerator SetupVR()
        {
#if UNITY_PS4
            // Register the callbacks needed to detect resetting the HMD
            Utility.onSystemServiceEvent += OnSystemServiceEvent;
            PlayStationVR.onDeviceEvent += OnDeviceEvent;

            // Post-reproject for camera locked items, in this case the reticle. Must be
            // set before we change the VR Device. See VRPostReprojection.cs for more info
            if (Camera.main.actualRenderingPath == RenderingPath.Forward)
            {
                if (FindObjectOfType<VRPostReprojection>())
                {
#if UNITY_PS4
                    PlayStationVRSettings.postReprojectionType = PlayStationVRPostReprojectionType.PerEye;
                    PlayStationVRSettings.postReprojectionRenderScale = renderScale;
#endif
                }
                else
                    Debug.LogError("You are trying to enable support for post-reprojection, but no post-reprojection script was found!");
            }
            else
            {
                Debug.LogError("Post reprojection is not yet fully supported in non-Forward Rendering Paths.");
            }
#endif

            UnityEngine.XR.XRSettings.LoadDeviceByName(VRDeviceNames.playStationVR);

            // WORKAROUND: At the moment the device is created at the end of the frame so
            // changing almost any VR settings needs to be delayed until the next frame
            yield return null;
            UnityEngine.XR.XRSettings.enabled = true;
            UnityEngine.XR.XRSettings.eyeTextureResolutionScale = renderScale;
            UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
        }

        public void BeginShutdownVR()
        {
            StartCoroutine(ShutdownVR());
        }

        IEnumerator ShutdownVR()
        {
            UnityEngine.XR.XRSettings.LoadDeviceByName(VRDeviceNames.None);

            // WORKAROUND: At the moment the device is created at the end of the frame so
            // we need to wait a frame until the VR device is changed back to 'None', and
            // then reset the Main Camera's FOV and Aspect
            yield return null;

            UnityEngine.XR.XRSettings.enabled = false;

#if UNITY_PS4
            // Unregister the callbacks needed to detect resetting the HMD
            Utility.onSystemServiceEvent -= OnSystemServiceEvent;
            PlayStationVR.onDeviceEvent -= OnDeviceEvent;
#endif

            Camera.main.fieldOfView = 60f;
            Camera.main.ResetAspect();

            // WARNING: This is specific to the sample and essentially restarts the
            // game. Please remove this for your own title and handle shutdown gracefully
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void SetupHmdDevice()
        {
#if UNITY_PS4
            // The HMD Setup Dialog is not displayed on the social screen in separate
            // mode, so we'll force it to mirror-mode first
            UnityEngine.XR.XRSettings.showDeviceView = true;

            // Show the HMD Setup Dialog, and specify the callback for when it's finished
            HmdSetupDialog.OpenAsync(0, OnHmdSetupDialogCompleted);
#endif
        }

        public void ToggleHmdViewOnMonitor(bool showOnMonitor)
        {
            showHmdViewOnMonitor = showOnMonitor;
            UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
        }

        public void ToggleHmdViewOnMonitor()
        {
            showHmdViewOnMonitor = !showHmdViewOnMonitor;
            UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
        }

        public void ChangeRenderScale(float scale)
        {
            UnityEngine.XR.XRSettings.eyeTextureResolutionScale = scale;
#if UNITY_PS4
            PlayStationVRSettings.postReprojectionRenderScale = scale;
#endif
        }

#if UNITY_PS4
        // HMD recenter happens in this event
        void OnSystemServiceEvent(Utility.sceSystemServiceEventType eventType)
        {
            Debug.LogFormat("OnSystemServiceEvent: {0}", eventType);

            if(eventType == Utility.sceSystemServiceEventType.ResetVrPosition)
                UnityEngine.XR.InputTracking.Recenter();
        }
#endif

#if UNITY_PS4
        // Detect completion of the HMD dialog and either proceed to setup VR, or throw a warning
        void OnHmdSetupDialogCompleted(DialogStatus status, DialogResult result)
        {
            Debug.LogFormat("OnHmdSetupDialogCompleted: {0}, {1}", status, result);

            switch (result)
            {
                case DialogResult.OK:
                    StartCoroutine(SetupVR());
                    break;
                case DialogResult.UserCanceled:
                    Debug.LogWarning("User Cancelled HMD Setup!");
                    BeginShutdownVR();
                    break;
            }
        }
#endif

#if UNITY_PS4
        // This handles disabling VR in the event that the HMD has been disconnected
        bool OnDeviceEvent(PlayStationVR.deviceEventType eventType, int value)
        {
            var handledEvent = false;

            switch (eventType)
            {
                case PlayStationVR.deviceEventType.deviceStarted:
                    Debug.LogFormat("### OnDeviceEvent: deviceStarted: {0}", value);
                    break;
                case PlayStationVR.deviceEventType.deviceStopped:
                    BeginShutdownVR();
                    break;
                case PlayStationVR.deviceEventType.StatusChanged: // e.g. HMD unplugged
                    var devstatus = (VRDeviceStatus)value;
                    Debug.LogFormat("### OnDeviceEvent: VRDeviceStatus: {0}", devstatus);
                    if (devstatus != VRDeviceStatus.Ready)
                    {
                        // TRC R4026 suggests showing the HMD Setup Dialog if the device status becomes non-ready
                        if (UnityEngine.XR.XRSettings.loadedDeviceName == VRDeviceNames.None)
                            SetupHmdDevice();
                        else
                            BeginShutdownVR();
                    }
                    handledEvent = true;
                    break;
                case PlayStationVR.deviceEventType.MountChanged:
                    var status = (VRHmdMountStatus)value;
                    Debug.LogFormat("### OnDeviceEvent: VRHmdMountStatus: {0}", status);
                    handledEvent = true;
                    break;
                case PlayStationVR.deviceEventType.CameraChanged:
                    // If the event is for the camera and the value is 0, the camera has been disconnected
                    Debug.LogFormat("### OnDeviceEvent: CameraChanged: {0}", value);
                    if (value == 0)
                        SetupHmdDevice();
                    handledEvent = true;
                    break;
                case PlayStationVR.deviceEventType.HmdHandleInvalid:
                    // Unity will handle this automatically, please see API documentation
                    Debug.LogFormat("### OnDeviceEvent: HmdHandleInvalid: {0}", value);
                    break;
                case PlayStationVR.deviceEventType.DeviceRestarted:
                    // Unity will handle this automatically, please see API documentation
                    Debug.LogFormat("### OnDeviceEvent: DeviceRestarted: {0}", value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eventType", eventType, null);
            }

            return handledEvent;
        }
#endif
    }
}
