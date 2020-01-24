using UnityEngine;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

namespace PlayStationVRExample
{
    public class WeaponController : MonoBehaviour
    {
        public float fireRate = 0.5f;
        public ParticleSystem shotEmitter;
        public bool isMoveController = false;
        public bool isSecondaryMoveController = false;
        public bool isAimController = false;
        public AudioSource shotSound;
        public AudioSource missSound;
        LaserPointer m_LaserPointer;
        RaycastHit m_Hit;
        float m_LastShotTime = 0;
        bool m_CanShoot = false;
        bool m_HasShootInput = false;
        [HideInInspector]
        public int shotsFired = 0;

        // Used for initialization
        void Start()
        {
            m_LaserPointer = FindObjectOfType<LaserPointer>();
            WeaponCanShoot(false);
            shotsFired = 0;
        }

        // Get input from DualShock 4 or Move controller(s), then shoot if possible
        void Update()
        {
            m_HasShootInput = CheckForInput();

            if (m_CanShoot && m_HasShootInput && Time.time > m_LastShotTime + fireRate)
            {
                Fire();
            }
        }

        // Move controllers use an API for their analog button, DualShock 4 and Aim controller use an axis name for R2
        bool CheckForInput()
        {
#if UNITY_PS4
            if (isMoveController)
            {
                if (!isSecondaryMoveController)
                {
                    return(PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
                }
                else
                {
                    return(PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
                }
            }
            else
            {
                return(Input.GetAxisRaw("TriggerRight") > 0 ? true : false);
            }
#else
		return Input.GetButton("Fire1");
#endif
        }

        // Fire the weapon, including a particle and audio effect
        void Fire()
        {
            StartCoroutine(Vibrate());

            m_LastShotTime = Time.time;
            shotEmitter.Emit(1);
            shotSound.Play();
            shotsFired++;

            if (Physics.Raycast(transform.position, transform.forward, out m_Hit))
            {
                if (m_Hit.transform.GetComponentInParent<TargetObject>())
                    m_Hit.transform.GetComponentInParent<TargetObject>().DestroyTarget();
                else
                    missSound.Play();
            }
        }

        // When fired the controller will vibrate for 0.1 seconds
        IEnumerator Vibrate()
        {
#if UNITY_PS4
            if (isMoveController)
            {
                PS4Input.MoveSetVibration(0, isSecondaryMoveController ? 1 : 0, 128);
            }
            else if (isAimController)
            {
                PS4Input.AimSetVibration(0, 0, 255);
            }
            else
            {
                PS4Input.PadSetVibration(0, 0, 255);
            }
#endif

            yield return new WaitForSeconds(0.1f);

#if UNITY_PS4
            if (isMoveController)
            {
                PS4Input.MoveSetVibration(0, isSecondaryMoveController ? 1 : 0, 0);
            }
            else if (isAimController)
            {
                PS4Input.AimSetVibration(0, 0, 0);
            }
            else
            {
                PS4Input.PadSetVibration(0, 0, 0);
            }
#endif
        }

        // Public call for toggling the weapon state
        public void WeaponCanShoot(bool shootBool)
        {
            m_CanShoot = shootBool;
            m_LaserPointer.gameObject.SetActive(shootBool);
        }
    }
}