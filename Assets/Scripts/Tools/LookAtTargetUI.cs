using UnityEngine;

namespace ProjectVR
{
    public class LookAtTargetUI : MonoBehaviour
    {
        public Transform lookAtTarget;

        private Vector3 targetPosition;

        void Start()
        {
            if (!this.lookAtTarget)
                this.lookAtTarget = GameObject.Find("VRCharacterController").transform;
        }

        void LateUpdate()
        {
            targetPosition = lookAtTarget.position;
            targetPosition.y = transform.position.y;
            this.transform.LookAt(targetPosition);
        }
    }
}