using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa el inventario del personaje
    /// </summary>
    public class VRCharacterInventory : MonoBehaviour
    {
        public int NumMaxObjects = 6;

        [System.Serializable]
        public class InventoryObject
        {
            public GameObject ObjectScene { get; set; }
            public GameObject ObjectContainer;
            public Image ObjectImage;
        }

        public List<InventoryObject> InventoryObjects;
        private GameObject currentObject;

        public static VRCharacterInventory Instance = null;

        private Sprite trasparentSprite;
        private CanvasGroup canvas;
        private bool isMoving;

        void Start()
        {
            Instance = this;

            this.canvas = this.GetComponent<CanvasGroup>();
            this.trasparentSprite = Resources.Load<Sprite>("TrasparentImage");

            /*Desactivo el LookAt si el modo VR esta activado
            if (VRSceneController.Instance.Player.VRMode)
            {
                this.GetComponent<LookAtTargetUI>().enabled = false;
            }*/
        }

        void Update()
        {
            if (Input.GetButtonDown("Touchpad"))
            {
                this.ShowPanel();
            }
        }

        /// <summary>
        /// Metodo para hacer aparecer el panel de inventario
        /// </summary>
        public void ShowPanel()
        {
            if (this.isMoving || (VRSceneController.Instance.PlayerCurrentObject && VRSceneController.Instance.PlayerCurrentObject.IsZooming))
                return;

            StartCoroutine(this.ShowPanelCo());
        }

        /// <summary>
        /// Metodo para hacer aparecer el panel de inventario
        /// </summary>
        /// <returns></returns>
        private IEnumerator ShowPanelCo()
        {
            this.isMoving = true;

            EventSystem.current.SetSelectedGameObject(null);

            //Activo/Desactivo el personaje
            if (VRSceneController.Instance.Player.VRMode)
                VRSceneController.Instance.ActivePlayer(this.canvas.alpha == 1);

            //Recoloco el inventario delante de la camara si esta oculto y si esta el modo VR activado
            if(this.canvas.alpha == 0 && VRSceneController.Instance.Player.VRMode)
            {
                this.transform.position = VRSceneController.Instance.Player.transform.position + VRSceneController.Instance.Player.transform.forward * 3;
                this.transform.position += new Vector3(0, 0.5f, 0);

                this.transform.LookAt(Camera.main.transform);
            }

            yield return StartCoroutine(CorgiTools.FadeCanvasGroup(this.canvas, 0.25f, (this.canvas.alpha == 0) ? 1 : 0));
            this.isMoving = false;
        }

        /// <summary>
        /// Metodo para ocultar el panel del inventario
        /// </summary>
        public void HidePanel()
        {
            EventSystem.current.SetSelectedGameObject(null);
            this.canvas.alpha = 0;
        }

        /// <summary>
        /// Metodo para añadir un objeto al inventario
        /// </summary>
        /// <param name="vrObject"></param>
        public void PickObject(GameObject vrObject)
        {
            int index = this.FindIndexEmptyContainer();

            this.InventoryObjects[index].ObjectScene = vrObject;
            this.InventoryObjects[index].ObjectImage.sprite = vrObject.GetComponent<VRRotateObject>().SpriteObject;
            this.InventoryObjects[index].ObjectScene.SetActive(false);

            VRSceneController.Instance.PlayerCurrentObject = null;
            VRSceneController.Instance.ActivePlayer(true);
        }

        /// <summary>
        /// Metodo que elimina el objeto del inventario
        /// </summary>
        public void DeleteObject()
        {
            int index = this.FindIndexSelectedObject();

            this.InventoryObjects[index].ObjectScene = null;
            this.InventoryObjects[index].ObjectImage.sprite = this.trasparentSprite;
        }

        /// <summary>
        /// Metodo que devuelve true si se puede coger un objeto nuevo
        /// </summary>
        /// <returns></returns>
        public bool CanPickObject(VRRotateObject vrObject)
        {
            return this.FindIndexEmptyContainer() != -1 && vrObject.IsZooming && vrObject.CanAddToInventory;
        }

        /// <summary>
        /// Metodo para encontrar el primer hueco vacio del inventario
        /// </summary>
        /// <returns>Indice del contenedor vacio, -1 si esta lleno el inventario</returns>
        private int FindIndexEmptyContainer()
        {
            for (int i = 0; i < this.NumMaxObjects; i++)
                if (this.InventoryObjects[i].ObjectScene == null)
                    return i;

            return -1;
        }

        /// <summary>
        /// Metodo para encontrar el objeto seleccionado del inventario
        /// </summary>
        /// <returns>Indice del objeto, -1 si no lo encuentra</returns>
        private int FindIndexSelectedObject()
        {
            for (int i = 0; i < this.NumMaxObjects; i++)
                if (this.InventoryObjects[i].ObjectContainer &&
                    this.InventoryObjects[i].ObjectContainer.name == EventSystem.current.currentSelectedGameObject.name &&
                    this.InventoryObjects[i].ObjectScene)
                    return i;

            return -1;
        }

        /// <summary>
        /// Metodo para encontrar el container seleccionado del inventario
        /// </summary>
        /// <returns>Indice del objeto, -1 si no lo encuentra</returns>
        private int FindIndexSelectedContainer()
        {
            for (int i = 0; i < this.NumMaxObjects; i++)
                if (this.InventoryObjects[i].ObjectContainer.name == EventSystem.current.currentSelectedGameObject.name)
                    return i;

            return -1;
        }

        /// <summary>
        /// Metodo para encontrar el objeto por nombre del inventario
        /// </summary>
        /// <returns>Indice del objeto, -1 si no lo encuentra</returns>
        public int FindIndexNameObject(string nameObject)
        {
            for (int i = 0; i < this.NumMaxObjects; i++)
                if (this.InventoryObjects[i].ObjectScene && this.InventoryObjects[i].ObjectScene.name == nameObject)
                    return i;

            return -1;
        }

        /// <summary>
        /// Metodo para cambiar el objeto seleccionado
        /// </summary>
        public void SelectObject()
        {
            if (this.canvas.alpha == 0 || this.isMoving || !VRSceneController.Instance.FinishAction)
                return;

            int index = this.FindIndexSelectedObject();
            //Si el slot no esta vacio, saco el objeto del inventario
            if (index != -1 && !VRSceneController.Instance.PlayerCurrentObject)
            {
                //Activo el objeto en la escena
                this.InventoryObjects[index].ObjectScene.SetActive(true);

                //Asocio el objeto a la mano y lo posiciono correctamente
                VRSceneController.Instance.PlayerCurrentObject = this.InventoryObjects[index].ObjectScene.GetComponent<VRRotateObject>();
                this.InventoryObjects[index].ObjectScene.GetComponent<VRRotateObject>().MoveObjectToHand();

                this.InventoryObjects[index].ObjectScene = null;
                this.InventoryObjects[index].ObjectImage.sprite = this.trasparentSprite;
            }
            //Si el slot no esta vacio y tengo un objeto en la mano, los intercambio
            else if (index != -1 && VRSceneController.Instance.PlayerCurrentObject && VRSceneController.Instance.PlayerCurrentObject.CanAddToInventory)
            {
                GameObject vrObject = VRSceneController.Instance.PlayerCurrentObject.gameObject;

                //Activo el objeto en la escena
                this.InventoryObjects[index].ObjectScene.SetActive(true);

                //Asocio el objeto a la mano y lo posiciono correctamente
                VRSceneController.Instance.PlayerCurrentObject = this.InventoryObjects[index].ObjectScene.GetComponent<VRRotateObject>();
                this.InventoryObjects[index].ObjectScene.GetComponent<VRRotateObject>().MoveObjectToHand();

                this.InventoryObjects[index].ObjectScene = vrObject;
                this.InventoryObjects[index].ObjectImage.sprite = vrObject.GetComponent<VRRotateObject>().SpriteObject;
                this.InventoryObjects[index].ObjectScene.SetActive(false);
            }
            //Si el slot esta vacio, meto el objeto en el inventario
            else if(index == -1 && VRSceneController.Instance.PlayerCurrentObject)
            {
                int indexContainer = this.FindIndexSelectedContainer();
                GameObject vrObject = VRSceneController.Instance.PlayerCurrentObject.gameObject;

                this.InventoryObjects[indexContainer].ObjectScene = vrObject;
                this.InventoryObjects[indexContainer].ObjectImage.sprite = vrObject.GetComponent<VRRotateObject>().SpriteObject;
                this.InventoryObjects[indexContainer].ObjectScene.SetActive(false);

                VRSceneController.Instance.PlayerCurrentObject = null;
            }
        }
    }
}