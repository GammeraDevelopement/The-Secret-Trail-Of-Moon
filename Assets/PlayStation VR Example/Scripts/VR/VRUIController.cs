using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

namespace PlayStationVRExample
{
    public class VRUIController : MonoBehaviour
    {
        public LayerMask uiLayerMask;
        float m_SliderFillSpeed = 0.75f;
        RaycastHit m_Hit;
        Slider m_CurrentSlider;
        AudioSource m_AudioSrc;

        void Start()
        {
            // Audio source for UI interaction feedback
            m_AudioSrc = GetComponent<AudioSource>();
        }

        void Update()
        {
            // Continuously check what's in front of the camera
            if (Physics.Raycast(transform.position, transform.forward, out m_Hit, Camera.main.farClipPlane, uiLayerMask))
            {
                // Check to make sure we haven't already got the current object
                if (EventSystem.current.currentSelectedGameObject != m_Hit.collider.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(m_Hit.collider.gameObject);

                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
                        m_CurrentSlider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
                    else
                        m_CurrentSlider = null;
                }
            }
            else
            {
                // For when there's no UI elements to look at
                EventSystem.current.SetSelectedGameObject(null);
                m_Hit.distance = 0;

                // If we were just looking at a slider, make sure we reset it now we're looking away
                if (m_CurrentSlider && m_CurrentSlider.value != 1)
                {
                    m_CurrentSlider.value = 0;
                    m_CurrentSlider = null;
                }
            }

            // Handles filling in sliders
            if (m_CurrentSlider)
            {
                // WORKAROUND: Right now there's a problem where the Move controller isn't maintaining the 'down' state of buttons
                // when queried via GetButton. For now this workaround should allow normal functionality

                #region PSMoveWorkaround

                // Loop through all 4 possible players, and both of their slots, to see if we have *any* move controller Cross buttons pressed
                var hasMoveButton = false;
                for (var slot = 0; slot < 4; slot++)
                {
                    for (var controller = 0; controller < 2; controller++)
                    {
#if UNITY_PS4
                        if (PS4Input.MoveIsConnected(slot, controller))
                        {
                            if (PS4Input.MoveGetButtons(slot, controller) == 64)
                                hasMoveButton = true;
                        }
#endif
                    }
                }

                #endregion

                if ((Input.GetButton("Fire1") || hasMoveButton) && m_CurrentSlider && m_CurrentSlider.value != 1 && m_CurrentSlider.interactable)
                {
                    m_CurrentSlider.value += Time.deltaTime * m_SliderFillSpeed;

                    if (m_CurrentSlider.value == 1)
                    {
                        m_AudioSrc.Play();

                        if (m_CurrentSlider.gameObject.GetComponent<VRUIComplete>())
                            m_CurrentSlider.gameObject.GetComponent<VRUIComplete>().Complete();
                    }
                }
                else if (Input.GetButtonUp("Fire1") && m_CurrentSlider.value != 1)
                {
                    m_CurrentSlider.value = 0;
                }
            }
        }
    }
}