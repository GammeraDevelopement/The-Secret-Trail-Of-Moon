using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4.VR;
#endif

namespace PlayStationVRExample
{
    public class VRPostReprojection : MonoBehaviour
    {
        public GameObject busySpinner;

#if UNITY_PS4
        int m_CurrentEye;
        RenderTexture m_PostReprojectionTexture;
        Camera m_Cam;

        void Update()
        {
            if (m_Cam == null)
                m_Cam = GetComponent<Camera>();

            // Reset which eye we're adjusting at the start of every frame
            m_CurrentEye = 0;
        }

        void OnPostRender()
        {
            if (UnityEngine.XR.XRSettings.loadedDeviceName == VRDeviceNames.playStationVR)
            {
                if (PlayStationVRSettings.postReprojectionType == PlayStationVRPostReprojectionType.None)
                {
                    DisablePostReprojection();
                }
                else
                {
                    m_PostReprojectionTexture = PlayStationVR.GetCurrentFramePostReprojectionEyeTexture(m_CurrentEye == 0 ? UnityEngine.XR.XRNode.LeftEye : UnityEngine.XR.XRNode.RightEye);

                    if (RenderTexture.active.antiAliasing > 1)
                    {
                        RenderTexture.active.ResolveAntiAliasedSurface(m_PostReprojectionTexture);
                    }
                    else
                    {
                        Graphics.Blit(RenderTexture.active, m_PostReprojectionTexture);
                    }

                    m_CurrentEye++;
                }
            }
        }
             
        void DisablePostReprojection()
        {
            // If post-reprojection isn't supported (either because it wasn't turned on, or else we're in
            // Deferred) then disable this script and re-parent the reticle to the main camera instead
            Debug.LogError("You're trying to use Post Reprojection, but it is not enabled in your PlayStationVRSettings!");

            if (transform.childCount > 0)
            {
                var reticle = transform.GetChild(0);
                reticle.gameObject.layer = 0;
                reticle.parent = Camera.main.transform;
            }

            gameObject.SetActive(false);
            busySpinner.SetActive(false);
        }
#endif // UNITY_PS4
    }
}
