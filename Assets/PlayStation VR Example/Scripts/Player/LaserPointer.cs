using UnityEngine;

namespace PlayStationVRExample
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserPointer : MonoBehaviour
    {
        LineRenderer m_Line;
        RaycastHit m_Hit;

        void Start()
        {
            m_Line = GetComponent<LineRenderer>();
        }

        // Keep the pointer aimed at whatever it's hitting, or else just keep going for 100 units
        void Update()
        {
            if (Physics.Raycast(transform.position, transform.forward, out m_Hit))
                m_Line.SetPosition(1, Vector3.forward * m_Hit.distance);
            else
                m_Line.SetPosition(1, Vector3.forward * 100);
        }
    }
}