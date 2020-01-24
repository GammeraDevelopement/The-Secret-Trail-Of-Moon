using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class FixedPosition : MonoBehaviour
    {
        private Vector3 _originLocalPosition;

        void Start()
        {
            this._originLocalPosition = this.transform.localPosition;
        }

        void Update()
        {
            this.transform.localPosition = Vector3.MoveTowards(
                this.transform.localPosition, 
                new Vector3(this.transform.localPosition.x, this._originLocalPosition.y, this.transform.localPosition.z), 
                Time.deltaTime);
        }
    }
}