using UnityEngine;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un objeto VR en la escena
    /// </summary>
    public abstract class VRObject : MonoBehaviour
    {
        protected Vector3 _originPosition;
        protected Vector3 _originRotation;

        public bool CanInteract = true;

        public string TutorialDescription = "btn_cross";

        private Outline _outline;

        public virtual void Start()
        {
            this._outline = this.GetComponent<Outline>();
            if (this._outline)
                this._outline.enabled = false;

            this._originPosition = this.transform.position;
            this._originRotation = this.transform.localEulerAngles;

            this.gameObject.layer = LayerMask.NameToLayer("Interactive");
        }

        /// <summary>
        /// Metodo para iluminar el objeto
        /// </summary>
        /// <param name="select"></param>
        public virtual void SelectObject(bool select)
        {
            if (this._outline)
                this._outline.enabled = select;

            ////Si no hay tutorial
            //if (this.TutorialDescription == "")
            //    return;

            //if (select)
            //    VRUIObject.Instance.ActiveUI(this.TutorialDescription, 2);
            //else
            //    VRUIObject.Instance.DisableUI();
        }

        /// <summary>
        /// Metodo para hacer interactuable el objeto
        /// </summary>
        /// <param name="active"></param>
        public void SetInteractive(bool active)
        {
            this.CanInteract = active;
        }

        /// <summary>
        /// Metodo para realizar el evento del objeto
        /// </summary>
        /// <returns></returns>
        public IEnumerator ActivateEvent()
        {
            if (!this.CanInteract)
                yield break;

            VRSceneController.Instance.FinishAction = false;
            yield return StartCoroutine(this.ActivateEventCo());
            VRSceneController.Instance.FinishAction = true;
        }

        protected abstract IEnumerator ActivateEventCo();
    }
}