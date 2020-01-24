using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un libro VR en la escena
    /// </summary>
    public class VRBellObject : VRRotateObject
    {
        private AudioSource _audio;
        private bool canMove;

        public override void Start()
        {
            base.Start();
            this._audio = this.GetComponentInChildren<AudioSource>();
        }

        public override void Update()
        {
            base.Update();

            if (!this.canMove)
                return;

            if(Input.GetButtonDown("Cross") && !this._audio.isPlaying)
            {
                this._audio.Play();
                ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }

        protected override IEnumerator ActivateObjectEventCo(bool active)
        {
            if(active)
            {
                this.canMove = true;
            }
            else
            {
                this.canMove = false;
            }

            yield return new WaitForSeconds(0);
        }
    }
}