using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un objeto VR en la escena. Objeto target de otra accion
    /// </summary>
    public class VRTargetObject : VRObject
    {
        [System.Serializable]
        public struct TargetObject
        {
            public string NameObject;
            public Vector3 TargetDistance;
            public Vector3 TargetRotation;
            public bool IsCorrect;
        }
        public List<TargetObject> TargetObjects;

        public bool HasBeenActivated { get; set; }
        public BoxCollider VRTargetObjectCollider { get; set; }

        public override void Start()
        {
            base.Start();
            this.HasBeenActivated = false;
        }

        protected override IEnumerator ActivateEventCo()
        {
            //Si el personaje no tiene el objeto en la mano
            if (!VRSceneController.Instance.PlayerCurrentObject)
                yield break;
            //Si el personaje tiene el objeto en la mano pero esta en zoom
            if(VRSceneController.Instance.PlayerCurrentObject.IsZooming)
                yield break;

            //Busco si existe el objeto entre las posibilidades
            GameObject current = VRSceneController.Instance.PlayerCurrentObject.gameObject;
            if (!this.TargetObjects.Any(x => x.NameObject == current.name))
                yield break;

            //Desasocio el objeto del personaje
            VRSceneController.Instance.PlayerCurrentObject = null;

            TargetObject to = this.TargetObjects.Single(x => x.NameObject == current.name);

            //Muevo el objeto hacia el objetivo
            current.transform.parent = this.transform;
            StartCoroutine(CorgiTools.RotateLocalFromTo(current, current.transform.localEulerAngles, to.TargetRotation, 1));
            yield return StartCoroutine(CorgiTools.MoveFromTo(current, current.transform.position, this.transform.position + to.TargetDistance, 1));

            //Permito que sea seleccionable y asocio este evento al objeto            
            current.GetComponent<VRRotateObject>().ConfigObject(true, true);
            current.GetComponent<VRRotateObject>().TargetObject = this;

            this.VRTargetObjectCollider = current.GetComponent<BoxCollider>();            

            //Activo el flag de correcto si el objeto es el adecuado
            this.HasBeenActivated = to.IsCorrect;

            //Desactivo el collider del target
            this.GetComponent<BoxCollider>().enabled = false;
            //Ejecuto los eventos asociados
            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        /// <summary>
        /// Metodo para resetear el TargetObject
        /// </summary>
        public void ResetTarget()
        {
            this.HasBeenActivated = false;
            this.GetComponent<BoxCollider>().enabled = true;
        }
    }
}