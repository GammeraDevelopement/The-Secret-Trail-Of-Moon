using UnityEngine;
using UnityEngine.VR;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

namespace PlayStationVRExample
{
    public class SetupManager : MonoBehaviour
    {
        Animator m_StateMachine;

        void Start()
        {
            m_StateMachine = GetComponent<Animator>();

#if UNITY_PS4

            // Register the callback needed to detect resetting the HMD
            Utility.onSystemServiceEvent += OnSystemServiceEvent;
#endif

            if (UnityEngine.XR.XRSettings.enabled == false)
            {
                m_StateMachine.SetTrigger("Need HMD Setup");
            }
            else
            {
                m_StateMachine.SetTrigger("Start Instructions");
                VRManager.instance.BeginVRSetup();
            }
        }

        void Update()
        {
            var stateInfo = m_StateMachine.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.HMD Setup"))
            {
                if (UnityEngine.XR.XRSettings.enabled == true)
                    m_StateMachine.SetTrigger("Start Instructions");
                else if (Input.GetButtonDown("Fire1"))
                    VRManager.instance.SetupHmdDevice();
            }
        }

        public void FinishedUIInteraction()
        {
            m_StateMachine.SetTrigger("Recentering");
        }

        public void ProgressToMainMenu()
        {
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MainMenu");
        }

#if UNITY_PS4
        void OnSystemServiceEvent(Utility.sceSystemServiceEventType eventType)
        {
            if (m_StateMachine == null)
                return;

            var stateInfo = m_StateMachine.GetCurrentAnimatorStateInfo(0);

            if (eventType == Utility.sceSystemServiceEventType.ResetVrPosition && stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Recentering"))
            {
                m_StateMachine.SetTrigger("Finished");
            }
        }
#endif
    }
}