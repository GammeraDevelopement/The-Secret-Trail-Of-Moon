using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un objeto VR en la escena. Objeto de movimiento
    /// </summary>
    public class VRMoveObject : VRObject
    {
        public Vector3 MovementDistance;
        public Vector3 MovementRotation;
        public Vector2 InputAxis;
        public bool UseAxis = false;

        private bool hasMoved;

        public override void Start()
        {
            base.Start();
        }

        protected override IEnumerator ActivateEventCo()
        {
            Vector2 input = CorgiTools.GetInputAxisNormalized(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
            if (!this.hasMoved || (this.UseAxis && input == this.InputAxis))
            {
                if (this.MovementRotation != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, this.MovementRotation, 1));
                if (this.MovementDistance != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, this._originPosition + this.MovementDistance, 1));

                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
            else if (this.hasMoved || (this.UseAxis && input == -this.InputAxis))
            {
                if (this.MovementRotation != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, this._originRotation, 1));
                if (this.MovementDistance != Vector3.zero)
                    yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, this._originPosition, 1));

                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
            }
            else
                yield break;

            this.hasMoved = !this.hasMoved;
        }
    }
}