using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un objeto VR en la escena. Objeto de movimiento en loop
    /// </summary>
    public class VRLoopObject : VRMoveObject
    {
        public int NumPosibilities = 10;
        public int NumActivated { get; set; }
        public float Time = 0.25f;

        protected override IEnumerator ActivateEventCo()
        {
            Vector2 input = CorgiTools.GetInputAxisNormalized(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            if (input == this.InputAxis)
            {
                if (this.MovementRotation != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.RotateLocalAngle(this.gameObject, this.MovementRotation, this.Time));
                if (this.MovementDistance != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, this.transform.position + this.MovementDistance, this.Time));

                this.NumActivated = (this.NumActivated < this.NumPosibilities - 1) ? this.NumActivated + 1 : 0;
            }
            else if (input == -this.InputAxis)
            {
                if (this.MovementRotation != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.RotateLocalAngle(this.gameObject, -this.MovementRotation, this.Time));
                if (this.MovementDistance != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, this.transform.position - this.MovementDistance, this.Time));

                this.NumActivated = (this.NumActivated > 0) ? this.NumActivated - 1 : this.NumPosibilities - 1;
            }
            else
                yield break;

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }
}