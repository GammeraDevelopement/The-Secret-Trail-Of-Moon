using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

namespace PlayStationVRExample
{
    public class ScoreManager : MonoBehaviour
    {
        public Text timerText;
        public Image timerDisplay;
        public Text scoreText;
        public Text accuracyText;
        public Text highScoreText;
        public float totalTime = 60f;
        public int scoreValue = 100;
        int m_TheScore = 0;
        static int s_HighScore = 0;
        float m_Accuracy = 100;
        int m_RollingScore = 0;
        float m_CurrentTime = 0f;
        bool m_TimerIsRunning = false;
        TargetsManager m_TargetsManager;
        WeaponController[] m_WeaponControls;

        // Used for initialization
        void Start()
        {
            timerText.text = m_CurrentTime.ToString("F0");
            scoreText.text = m_RollingScore.ToString();
            accuracyText.text = m_Accuracy.ToString("F0") + "%";
            highScoreText.text = s_HighScore.ToString();

            m_TargetsManager = FindObjectOfType<TargetsManager>();
            m_WeaponControls = FindObjectsOfType<WeaponController>();
        }

        void Update()
        {
            if (m_TimerIsRunning)
            {
                if (m_CurrentTime <= 0)
                    StartCoroutine(EndTimer());
                else
                    Timer();
            }

            UpdateScoreDisplay();
        }

        // Countdown the timer, text, and a graphical representation of the timer
        void Timer()
        {
            timerDisplay.fillAmount = m_CurrentTime / totalTime;
            timerText.text = m_CurrentTime.ToString("F0");
            m_CurrentTime -= Time.deltaTime;
        }

        // Once the timer is finished, turn off the weapon(s), stop spawning new targets, and restart the game
        IEnumerator EndTimer()
        {
            m_TimerIsRunning = false;
            m_CurrentTime = 0;

            if (m_TheScore > s_HighScore)
                s_HighScore = m_TheScore;

            CalculateAccuracy();

            for (var i = 0; i < m_WeaponControls.Length; i++)
                m_WeaponControls[i].WeaponCanShoot(false);

            m_TargetsManager.StopSpawning();

            // Exit to main menu after a delay
            yield return new WaitForSeconds(3f);
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MainMenu");
        }

        // Keep the score and text display updated
        void UpdateScoreDisplay()
        {
            if (m_RollingScore < m_TheScore)
            {
                m_RollingScore = (int)Mathf.Lerp(m_RollingScore, m_TheScore, Time.deltaTime * 10f);
                m_RollingScore += 1;
            }
            else if (m_RollingScore > m_TheScore)
            {
                m_RollingScore = m_TheScore;
            }

            CalculateAccuracy();

            scoreText.text = m_RollingScore.ToString();
            accuracyText.text = m_Accuracy.ToString("F0") + "%";
            highScoreText.text = s_HighScore.ToString();
        }

        public void StartTimer()
        {
            m_TheScore = 0;
            m_TimerIsRunning = true;
            m_CurrentTime = totalTime;
        }

        void CalculateAccuracy()
        {
            var totalShotsFired = 0;

            for (var i = 0; i < m_WeaponControls.Length; i++)
                totalShotsFired += m_WeaponControls[i].shotsFired;

            if (m_TheScore != 0)
            {
                m_Accuracy = ((float)m_TheScore / ((float)scoreValue * (float)totalShotsFired)) * 100f;
            }
        }

        public void IncreaseScore()
        {
            m_TheScore += scoreValue;
        }

        public void Quit()
        {
            m_TimerIsRunning = false;
            FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MainMenu");
        }
    }
}