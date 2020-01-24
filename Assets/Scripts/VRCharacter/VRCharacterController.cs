using System;
using UnityEngine;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa el control del personaje
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class VRCharacterController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 2.5f;   // Speed when walking forward
            public float BackwardSpeed = 1.5f;  // Speed when walking backwards
            public float StrafeSpeed = 2.0f;    // Speed when walking sideways
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector]
            public float CurrentTargetSpeed = 8f;

            public bool CanMove = true;

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
                if (input == Vector2.zero) return;
                if (input.x > 0 || input.x < 0)
                {
                    //strafe
                    CurrentTargetSpeed = StrafeSpeed;
                }
                if (input.y < 0)
                {
                    //backwards
                    CurrentTargetSpeed = BackwardSpeed;
                }
                if (input.y > 0)
                {
                    //forwards
                    //handled last as if strafing and moving forward at the same time forwards speed should take precedence
                    CurrentTargetSpeed = ForwardSpeed;
                }
            }
        }

        [Serializable]
        public class MouseLook
        {
            public Vector2 Sensitivity = new Vector2(1.0f, 1.0f);
            public float SmoothTime = 18f;
            public bool VRMode;

            private Quaternion m_CharacterTargetRot;
            private Quaternion m_CameraTargetRot;

            public void Init(Transform character, Transform camera)
            {
                m_CharacterTargetRot = character.localRotation;
                m_CameraTargetRot = camera.localRotation;
            }

            public void LookRotation(Transform character, Transform camera)
            {
                float yRot, xRot;
                if (this.VRMode)
                {
                    yRot = Input.GetAxis("Horizontal2") * Sensitivity.x;
                    xRot = 0;
                }
                else //Modo PC
                {
                    yRot = Input.GetAxis("Horizontal2") * Sensitivity.x;
                    xRot = Input.GetAxis("Vertical2") * Sensitivity.y;
                }                

                m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

                if(this.VRMode)
                {
                    character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                        SmoothTime * Time.deltaTime);
                }
                else //Modo PC
                {
                    character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                        SmoothTime * Time.deltaTime);
                    camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                        SmoothTime * Time.deltaTime);
                }                
            }
        }

        [Serializable]
        public class AdvancedSettings
        {
            public float GroundCheckDistance = 0.1f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float StickToGroundHelperDistance = 0.6f; // stops the character
            public float SlowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float ShellOffset = 0; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }

        public Camera Cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();

        public Transform InventoryPosition;
        public Transform ZoomPosition;
        public VRRotateObject CurrentObject { get; set; }

        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_PreviouslyGrounded, m_IsGrounded;

        public Vector3 Velocity { get { return m_RigidBody.velocity; } }
        public bool Grounded { get { return m_IsGrounded; } }

        public bool VRMode { get { return this.mouseLook.VRMode; } }

        void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init(transform, Cam.transform);
        }

        void FixedUpdate()
        {
            RotateView();
            GroundCheck();

            if (!this.movementSettings.CanMove)
                return;

            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && m_IsGrounded)
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = Cam.transform.forward * input.y + Cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;

                if (m_RigidBody.velocity.sqrMagnitude < (movementSettings.CurrentTargetSpeed * movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove * SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;
                if (Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded)
                {
                    StickToGroundHelper();
                }
            }
        }

        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }

        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.ShellOffset), Vector3.down, out hitInfo,
                                    ((m_Capsule.height / 2f) - m_Capsule.radius) +
                                    advancedSettings.StickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }

        private Vector2 GetInput()
        {
            Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };
            movementSettings.UpdateDesiredTargetSpeed(input);

            return input;
        }

        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon)
                return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation(transform, Cam.transform);

            if (m_IsGrounded)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation * m_RigidBody.velocity;
            }
        }

        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;

            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.ShellOffset), Vector3.down, out hitInfo,
                                    ((m_Capsule.height / 2f) - m_Capsule.radius) + advancedSettings.GroundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                //m_GroundContactNormal = hitInfo.normal;
                m_GroundContactNormal = Vector3.up; //Para que nunca se quede atrapado
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
        }
    }
}