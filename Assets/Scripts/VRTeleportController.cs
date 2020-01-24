using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVR
{
    public class VRTeleportController : MonoBehaviour
    {
        public float FadeSpeed;

        public LayerMask TeleportLayerMask;
        public float DistanceRaycast;

        /// <summary>
        /// Referencia al canvas que controla el Fade
        /// </summary>
        public CanvasGroup FadeCanvasGroup;

        //Objeto actual y componentes de VRObject
        private GameObject currentObject;
        private VRObject currentVRObject;
        private VRTeleportNode currentTeleportNode;

        private RaycastHit hit;

        void Update()
        {
            this.Raycast();
            //this.PlayerActions();
            this.RaycastActions();
        }

        /// <summary>
        /// Metodo para realizar el raycast
        /// </summary>
        private void Raycast()
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * this.DistanceRaycast, Color.cyan);

            if (Physics.Raycast(transform.position, transform.forward, out hit, this.DistanceRaycast, TeleportLayerMask))
            {
                if (currentObject != hit.collider.gameObject) //Si es distinto al anterior
                {
                    if (currentObject)
                        currentVRObject.SelectObject(false); //Deselecciono el anterior

                    currentObject = hit.collider.gameObject;
                    currentVRObject = hit.collider.GetComponent<VRObject>();
                    currentTeleportNode = hit.collider.GetComponent<VRTeleportNode>();

                    currentVRObject.SelectObject(true);
                }
            }
            else
            {
                if (currentObject) //Si existe, esta seleccionado y ha terminado su accion, lo deselecciono
                {
                    currentVRObject.SelectObject(false);
                    currentVRObject = null;
                    currentObject = null;
                }
            }
        }

        /// <summary>
        /// Metodo para realizar las acciones del raycast
        /// </summary>
        private void RaycastActions()
        {
            if (this.currentObject)
            {
                //MoveObject
                if (Input.GetButtonDown("Cross") && currentTeleportNode)
                {
                    StartCoroutine(FadeInRoutine());

                    StartCoroutine(currentVRObject.ActivateEvent());
                }              
            }
        }

        IEnumerator FadeInRoutine()
        {
            //TODO: Puede que a lo mejor haya que poner esto
            //if (FindObjectOfType<VRPostReprojection>())
            //    FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(true);

            while (FadeCanvasGroup.alpha < 1)
            {
                FadeCanvasGroup.alpha += Time.deltaTime * FadeSpeed;
                yield return null;
            }

            VRSceneController.Instance.Player.transform.position = new Vector3(currentTeleportNode.transform.position.x, VRSceneController.Instance.Player.transform.position.y, currentTeleportNode.transform.position.z);

            StartCoroutine(FadeOutRoutine());
        }

        IEnumerator FadeOutRoutine()
        {
            //TODO: Puede que a lo mejor haya que poner esto
            //if (FindObjectOfType<VRPostReprojection>())
            //    FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(false);

            while (FadeCanvasGroup.alpha > 0)
            {
                FadeCanvasGroup.alpha -= Time.deltaTime * FadeSpeed;
                yield return null;
            }

        }

    }
}