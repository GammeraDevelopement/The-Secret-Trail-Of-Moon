using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using PlayStationVRExample;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa el control de la interfaz de la escena
    /// </summary>
    public class VRUIController : MonoBehaviour
    {
        public LayerMask uiLayerMask;

        private float sliderFillSpeed = 0.75f;

        private RaycastHit hit;
        private Slider currentSlider;
        private Button currentButton;

        private AudioSource audioSrc;

        void Start()
        {
            audioSrc = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, Camera.main.farClipPlane, uiLayerMask))
            {
                //Check to make sure we haven't already got the current object
                if (EventSystem.current.currentSelectedGameObject != hit.collider.gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(hit.collider.gameObject);

                    if (EventSystem.current.currentSelectedGameObject.GetComponent<Slider>())
                        currentSlider = EventSystem.current.currentSelectedGameObject.GetComponent<Slider>();
                    else if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                        currentButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                    else
                        currentSlider = null;
                }
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
                hit.distance = 0;

                //Reset slider
                if (currentSlider && currentSlider.value != 1)
                {
                    currentSlider.value = 0;
                    currentSlider = null;
                }
                //Reset button
                if (currentButton)
                {
                    currentButton = null;
                }
            }

            //Handles sliders
            if (currentSlider)
            {
                if ((Input.GetButton("Cross")) && currentSlider && currentSlider.value != 1 && currentSlider.interactable)
                {
                    currentSlider.value += Time.deltaTime * sliderFillSpeed;

                    if (currentSlider.value == 1)
                    {
                        if (audioSrc)
                            audioSrc.Play();

                        if (currentSlider.gameObject.GetComponent<VRUIComplete>())
                            currentSlider.gameObject.GetComponent<VRUIComplete>().Complete();
                    }
                }
                else if (Input.GetButtonUp("Cross") && currentSlider.value != 1)
                {
                    currentSlider.value = 0;
                }
            }

            //Handles buttons
            if (currentButton)
            {
                if ((Input.GetButtonDown("Cross")) && currentButton && currentButton.interactable)
                {
                    if (currentButton.gameObject.GetComponent<VRUIComplete>())
                    {
                        currentButton.gameObject.GetComponent<VRUIComplete>().Complete();
                    }
                }
            }
        }
    }
}