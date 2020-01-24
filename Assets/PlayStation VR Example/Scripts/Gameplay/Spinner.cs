using UnityEngine;
using System.Collections;

namespace PlayStationVRExample
{
    public class Spinner : MonoBehaviour
    {
        public bool local = true;
        public bool allowManual = false;
        public bool allowAutomatic = true;
        public bool lockZ = true;

        public float autoTimer = 2f;
        public Vector3 spinDirection = Vector3.up;
        public float speed = 50f;

        float m_GoAutomaticTime;
        Vector3 m_AutoSpinDirection;

        void Start()
        {
            m_AutoSpinDirection = spinDirection;
        }

        void Update()
        {
            if (allowManual && (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f))
            {
                m_GoAutomaticTime = Time.time + autoTimer;
            }

            if (Time.time > m_GoAutomaticTime && allowAutomatic)
            {
                spinDirection = m_AutoSpinDirection;
            }
            else
            {
                spinDirection = new Vector3(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"), 0);
            }
        }

        void FixedUpdate()
        {
            if (local)
                transform.Rotate(spinDirection, Time.deltaTime * speed);
            else
                transform.RotateAround(transform.position, spinDirection, Time.deltaTime * speed);

            if (!lockZ)
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
            else
                transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f));
        }
    }
}