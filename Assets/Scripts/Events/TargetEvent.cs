using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un puzzle basado en VRTargetObject
    /// </summary>
    public class TargetEvent : MonoBehaviour
    {
        public List<VRTargetObject> TargetObjects;

        /// <summary>
        /// Metodo para comprobar el puzzle
        /// </summary>
        public void CheckPuzzle()
        {
            bool check = this.TargetObjects.Any(x => !x.HasBeenActivated);
            if(!check)
            {
                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                this.ActivateTargetObjects(false);
                foreach (VRTargetObject to in this.TargetObjects)
                    to.VRTargetObjectCollider.enabled = false;
            }
            else
            {

            }
        }

        /// <summary>
        /// Metodo para activar los TargetObjects
        /// </summary>
        /// <param name="active"></param>
        public void ActivateTargetObjects(bool active)
        {
            foreach (VRTargetObject target in this.TargetObjects)
            {
                target.GetComponent<BoxCollider>().enabled = active;
            }                
        }
    }
}