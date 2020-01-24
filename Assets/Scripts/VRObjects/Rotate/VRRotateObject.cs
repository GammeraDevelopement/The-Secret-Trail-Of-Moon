using UnityEngine;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un objeto VR en la escena. Objeto de inventario o de observacion
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class VRRotateObject : VRObject
    {
        public Vector3 InventoryRotation;
        public Vector3 ZoomRotation;

        public Sprite SpriteObject;
        public bool CanAddToInventory
        {
            get { return this.SpriteObject; }
        }

        public string TutorialRotateDescription = "btn_joystick_right";

        private Rigidbody _rigidbody;
        private bool canRotate;

        public bool IsZooming { get; set; }
        public VRTargetObject TargetObject { get; set; }

        public override void Start()
        {
            base.Start();
            this._rigidbody = this.GetComponent<Rigidbody>();
            this._rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            this._originRotation = this.transform.eulerAngles;
        }

        public virtual void Update()
        {
            if (this.canRotate)
            {
                transform.localRotation *= Quaternion.Euler(new Vector3(Input.GetAxis("Vertical2"), 0, -Input.GetAxis("Horizontal2")));
            }
        }

        protected override IEnumerator ActivateEventCo()
        {
            //Si tengo un objeto en la mano y puedo añadirlo al inventario, lo añado
            if (VRSceneController.Instance.PlayerCurrentObject && VRSceneController.Instance.PlayerCurrentObject.CanAddToInventory)
                VRCharacterInventory.Instance.PickObject(VRSceneController.Instance.PlayerCurrentObject.gameObject);
            else if(VRSceneController.Instance.PlayerCurrentObject && !VRSceneController.Instance.PlayerCurrentObject.CanAddToInventory)
                yield break;

            VRSceneController.Instance.ActivePlayer(false);

            this.canRotate = false;
            this.ConfigObject(true, false);

            //Llevo el objeto a la mano del jugador
            this.gameObject.transform.parent = VRSceneController.Instance.PlayerInventoryPosition;

            StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, this.InventoryRotation, 1));
            yield return StartCoroutine(CorgiTools.MoveFromToTarget(this.gameObject, VRSceneController.Instance.PlayerInventoryPosition, 1));

            VRSceneController.Instance.ActivePlayer(true);            

            //Asocio el objeto al Player
            VRSceneController.Instance.PlayerCurrentObject = this.gameObject.GetComponent<VRRotateObject>();

            //Si tiene un evento asociado, lo reseteamos
            if (this.TargetObject)
            {
                this.TargetObject.ResetTarget();
                this.TargetObject = null;
            }
        }

        /// <summary>
        /// Metodo para activar el evento del objeto en zoom
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ActivateObjectEvent(bool active)
        {
            VRSceneController.Instance.FinishAction = false;
            yield return StartCoroutine(this.ActivateObjectEventCo(active));
            VRSceneController.Instance.FinishAction = true;
        }

        protected virtual IEnumerator ActivateObjectEventCo(bool active)
        {
            yield return null;
        }

        /// <summary>
        /// Metodo para hacer zoom in out de un objeto
        /// </summary>
        public IEnumerator ZoomObject()
        {
            VRSceneController.Instance.FinishAction = false;
            VRSceneController.Instance.ActivePlayer(false);

            if (!this.IsZooming)
            {
                VRSceneController.Instance.ActivePlayer(false);

                this.ConfigObject(true, false);

                //Oculto el panel del inventario si hago zoom en el objeto
                VRCharacterInventory.Instance.HidePanel();

                //Recoloco el PlayerZoom delante de la camara
                VRSceneController.Instance.PlayerZoomPosition.position = Camera.main.transform.position + Camera.main.transform.forward * 0.75f;
                VRSceneController.Instance.PlayerZoomPosition.LookAt(Camera.main.transform);

                this.gameObject.transform.parent = VRSceneController.Instance.PlayerZoomPosition;

                StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, this.ZoomRotation, 1));
                yield return StartCoroutine(CorgiTools.MoveFromToTarget(this.gameObject, VRSceneController.Instance.PlayerZoomPosition, 1));
                yield return StartCoroutine(this.ActivateObjectEvent(true));

                //Muestro el tutorial del VRRotateObject
                //VRUIObject.Instance.ActiveUI(this.TutorialRotateDescription, 0.5f);

                this.canRotate = true;
            }
            else
            {
                this.canRotate = false;

                this.gameObject.transform.parent = VRSceneController.Instance.PlayerInventoryPosition;

                //Desactivo el tutorial
                //VRUIObject.Instance.DisableUI();

                StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, this.InventoryRotation, 1));
                yield return StartCoroutine(CorgiTools.MoveFromToTarget(this.gameObject, VRSceneController.Instance.PlayerInventoryPosition, 1));
                yield return StartCoroutine(this.ActivateObjectEvent(false));

                VRSceneController.Instance.ActivePlayer(true);
                this.ConfigObject(true, false);
            }

            this.IsZooming = !this.IsZooming;
            VRSceneController.Instance.FinishAction = true;
        }

        /// <summary>
        /// Metodo para resetear la posicion del objeto
        /// </summary>
        public IEnumerator DropObject()
        {
            if (!this.IsZooming)
                yield break;

            VRSceneController.Instance.FinishAction = false;

            //Desactivo su comportamiento en zoom
            yield return StartCoroutine(this.ActivateObjectEvent(false));

            StartCoroutine(CorgiTools.RotateFromTo(this.gameObject, this.transform.eulerAngles, _originRotation, 1));
            yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, _originPosition, 1));

            this.ConfigObject(true, true);

            this.transform.parent = null;

            //No permito su rotacion e indico que ya no esta en zoom
            this.canRotate = this.IsZooming = false;

            VRSceneController.Instance.PlayerCurrentObject = null;
            VRSceneController.Instance.ActivePlayer(true);

            VRSceneController.Instance.FinishAction = true;
        }

        /// <summary>
        /// Metodo para posicionar el objeto en la mano directamente. Se usa al seleccionarlo desde el Inventario
        /// </summary>
        public void MoveObjectToHand()
        {
            this.canRotate = false;

            this.gameObject.transform.parent = VRSceneController.Instance.PlayerInventoryPosition;

            this.transform.localEulerAngles = this.InventoryRotation;
            this.gameObject.transform.position = VRSceneController.Instance.PlayerInventoryPosition.position;

            this.ConfigObject(true, false);

            this.IsZooming = false;
        }

        /// <summary>
        /// Metodo para configurar un objeto fisicamente
        /// </summary>
        /// <param name="rotate"></param>
        /// <param name="collisions"></param>
        public void ConfigObject(bool kinematic, bool collisions)
        {
            this._rigidbody.isKinematic = kinematic;
            this._rigidbody.detectCollisions = collisions;
        }
    }
}