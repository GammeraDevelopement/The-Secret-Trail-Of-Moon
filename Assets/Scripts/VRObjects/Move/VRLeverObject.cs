using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa una palanca VR en la escena. Objeto de rotacion en estrella
    /// </summary>
    public class VRLeverObject : VRMoveObject
    {
        public List<Vector2> InputSolution;
        private int indexSolution = 0;

        protected override IEnumerator ActivateEventCo()
        {
            Vector2 input = CorgiTools.GetInputAxisNormalizedCross(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            if (input != Vector2.zero)
            {
                yield return StartCoroutine(CorgiTools.RotateLocalAngle(this.gameObject, new Vector3(-30 * input.x, 30 * input.y, 0), 1));
                this.CheckSolution(input);
            }                
            else
                yield break;

            yield return StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.gameObject.transform.localEulerAngles, new Vector3(0, -30, 0), 1));

            if (indexSolution == this.InputSolution.Count)
                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        /// <summary>
        /// Metodo para comprobar la solucion de la palanca
        /// </summary>
        /// <param name="input"></param>
        public void CheckSolution(Vector2 input)
        {
            if (input == this.InputSolution[this.indexSolution])
            {
                this.indexSolution = this.indexSolution + 1;
                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
            }                
            else
                this.indexSolution = 0;
        }
    }
}