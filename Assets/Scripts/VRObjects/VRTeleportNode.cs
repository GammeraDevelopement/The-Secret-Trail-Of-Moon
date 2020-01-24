using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVR
{
    public class VRTeleportNode : VRObject
    {
        public Material FocusMaterial;

        private Material defaultMaterial;

        // Use this for initialization
        public override void Start()
        {
            base.Start();

            defaultMaterial = GetComponent<MeshRenderer>().material;
            this.gameObject.layer = LayerMask.NameToLayer("Teleport");
        }

        // Update is called once per frame
        void Update()
        {

        }

        protected override IEnumerator ActivateEventCo()
        {
            yield break;
        }

        public override void SelectObject(bool select)
        {
            base.SelectObject(select);

            if (select)
            {
                GetComponent<MeshRenderer>().material = FocusMaterial;
            }

            else
            {
                GetComponent<MeshRenderer>().material = defaultMaterial;

            }
        }
    }
}
