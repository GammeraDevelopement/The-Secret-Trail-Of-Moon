using UnityEngine;
using System.Collections;

namespace PlayStationVRExample
{
    public class InstructionsManager : MonoBehaviour
    {
        public CanvasGroup instructionsCanvasGroup;
        public float fadeSpeed = 2f;
        bool m_HasStarted = false;
        WeaponController[] m_WeaponControls;

        void Start()
        {
            m_WeaponControls = GameObject.FindObjectsOfType<WeaponController>();
        }

        public void BeginGame()
        {
            if (!m_HasStarted)
            {
                m_HasStarted = true;
                StartCoroutine(BeginGameCoroutine());
            }
        }

        // Fade down the instructions canvas, wait a couple seconds, and then start spawning targets and running the timer
        IEnumerator BeginGameCoroutine()
        {
            instructionsCanvasGroup.interactable = false;

            while (instructionsCanvasGroup.alpha > 0)
            {
                instructionsCanvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            for (var i = 0; i < m_WeaponControls.Length; i++)
                m_WeaponControls[i].WeaponCanShoot(true);

            FindObjectOfType<TargetsManager>().BeginSpawning();
            FindObjectOfType<ScoreManager>().StartTimer();
        }
    }
}