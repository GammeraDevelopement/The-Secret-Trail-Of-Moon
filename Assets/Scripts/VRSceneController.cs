using UnityEngine;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa el control de la escena
    /// </summary>
    public class VRSceneController : MonoBehaviour
    {
        public LayerMask SceneLayerMask;
        public float DistanceRaycast = 3;

        //Objeto actual y componentes de VRObject
        private GameObject currentObject;
        private VRObject currentVRObject;
        private VRRotateObject currentVRRotateObject;
        private VRMoveObject currentVRMoveObject;
        private VRTargetObject currentVRTargetObject;

        private RaycastHit hit;

        //Flag de movimiento de Player
        private bool canMovePlayer = true;

        //Player
        public VRCharacterController Player { get; set; }
        public VRRotateObject PlayerCurrentObject
        {
            get { return this.Player.CurrentObject; }
            set
            {
                this.PlayerInventoryPosition.GetComponent<SphereCollider>().enabled = (value) ? true : false;
                this.Player.CurrentObject = value;
            }
        }
        public Transform PlayerInventoryPosition
        {
            get { return this.Player.InventoryPosition; }
        }
        public Transform PlayerZoomPosition
        {
            get { return this.Player.ZoomPosition; }
        }

        //Indica si ha terminado la accion actual
        public bool FinishAction { get; set; }
        //Instancia
        public static VRSceneController Instance = null;

        //Indica si se esta usando el L2
        private bool isL2AxisUsing;

        void Start()
        {
            Instance = this;
            this.FinishAction = true;

            this.Player = FindObjectOfType<VRCharacterController>();
        }

        void Update()
        {
            //Si no ha terminado la accion actual, no hago nada
            if (!this.FinishAction)
                return;

            this.Raycast();
            this.PlayerActions();
            this.RaycastActions();
        }

        /// <summary>
        /// Metodo para realizar el raycast
        /// </summary>
        private void Raycast()
        {
            //Si tengo un objeto en zoom, no hay raycast
            if (this.PlayerCurrentObject && this.PlayerCurrentObject.IsZooming)
            {
                if (currentObject) //Si existe, esta seleccionado y ha terminado su accion, lo deselecciono
                {
                    currentVRObject.SelectObject(false);
                    currentVRObject = null;
                    currentObject = null;                    
                }
                return;
            }                

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * this.DistanceRaycast, Color.green);
            if (Physics.Raycast(transform.position, transform.forward, out hit, this.DistanceRaycast, SceneLayerMask))
            {
                if (currentObject != hit.collider.gameObject) //Si es distinto al anterior
                {
                    if (currentObject)
                        currentVRObject.SelectObject(false); //Deselecciono el anterior

                    currentObject = hit.collider.gameObject;
                    currentVRObject = hit.collider.GetComponent<VRObject>();
                    currentVRMoveObject = hit.collider.GetComponent<VRMoveObject>();
                    currentVRRotateObject = hit.collider.GetComponent<VRRotateObject>();
                    currentVRTargetObject = hit.collider.GetComponent<VRTargetObject>();

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
        /// Metodo para realizar las acciones del personaje
        /// </summary>
        private void PlayerActions()
        {
            //Si tengo un objeto en la mano
            if (this.PlayerCurrentObject)
            {
                if (Input.GetButtonDown("Fire2") && VRCharacterInventory.Instance.CanPickObject(this.PlayerCurrentObject)) //PickObject
                {
                    VRCharacterInventory.Instance.PickObject(this.PlayerCurrentObject.gameObject);

                    //Desactivo el tutorial
                    //VRUIObject.Instance.DisableUI();
                }
                else if (Input.GetButtonDown("Fire2")) // ResetObject
                {
                    StartCoroutine(this.PlayerCurrentObject.DropObject());

                    //Desactivo el tutorial
                    //VRUIObject.Instance.DisableUI();
                }
                else if (Input.GetAxisRaw("TriggerLeft") > 0 && !this.isL2AxisUsing && !currentVRObject /*&& !this.PlayerCurrentObject.IsZooming*/) //ZoomObject
                {
                    StartCoroutine(this.PlayerCurrentObject.ZoomObject());
                    this.isL2AxisUsing = true;
                }

                if (Input.GetAxisRaw("TriggerLeft") == 0 /*&& this.PlayerCurrentObject.IsZooming*/)
                {
                    //StartCoroutine(this.PlayerCurrentObject.ZoomObject());
                    this.isL2AxisUsing = false;                    
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
                if(Input.GetButtonDown("Cross") && currentVRMoveObject && !currentVRMoveObject.UseAxis)
                {
                    StartCoroutine(currentVRObject.ActivateEvent());
                }
                else if(Input.GetAxisRaw("L2") > 0 && currentVRMoveObject && currentVRMoveObject.UseAxis)
                {
                    StartCoroutine(currentVRObject.ActivateEvent());
                }
                //RotateObject / TargetObject
                else if (Input.GetButtonDown("Cross") && (currentVRRotateObject || currentVRTargetObject))
                {
                    StartCoroutine(currentVRObject.ActivateEvent());
                }

                //Bloqueo del jugador si estoy usando el L2 para mover un objeto si el personaje esta desbloqueado o si estoy moviendo un objeto en zoom
                if (Input.GetAxisRaw("L2") > 0 && currentVRMoveObject && currentVRMoveObject.UseAxis && this.canMovePlayer)
                {
                    this.Player.enabled = false;
                }
            }

            //Desbloqueo del jugador si suelto el R2 si el personaje esta desbloqueado
            if (Input.GetAxisRaw("TriggerLeft") == 0 && this.canMovePlayer)
            {
                this.Player.enabled = true;
            }
        }

        /// <summary>
        /// Metodo para activar / desactivar el personaje
        /// </summary>
        /// <param name="active"></param>
        public void ActivePlayer(bool active)
        {
            this.Player.enabled = active;

            //Flag
            this.canMovePlayer = active;
        }
    }
}