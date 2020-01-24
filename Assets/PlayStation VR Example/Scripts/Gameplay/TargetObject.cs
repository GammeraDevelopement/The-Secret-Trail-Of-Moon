using UnityEngine;

namespace PlayStationVRExample
{
    public class TargetObject : MonoBehaviour
    {
        public GameObject explosionEffect;
        public GameObject solidTarget;
        public GameObject brokenTarget;
        public AudioClip audioClipSpawned;
        public AudioClip audioClipHit;
        public float destructionForce = 5f;
        public float debisDestroyDelay = 6f;
        AudioSource m_AudioPlayer;

        // Used for initialization
        void Start()
        {
            brokenTarget.SetActive(false);
            transform.LookAt(Camera.main.transform.position);
            m_AudioPlayer = GetComponent<AudioSource>();
            m_AudioPlayer.PlayOneShot(audioClipSpawned);
        }

        public void DestroyTarget()
        {
            // Increase the score, send a call to try and immediately spawn an object
            FindObjectOfType<ScoreManager>().IncreaseScore();
            FindObjectOfType<TargetsManager>().SpawnTargetImmediately();

            // Particle effect for the target being broken
            Instantiate(explosionEffect, transform.position, transform.rotation);

            // Swap between the solid and broken (physics) versions of the target
            solidTarget.SetActive(false);
            brokenTarget.SetActive(true);

            // Sound of being destroyed
            m_AudioPlayer.PlayOneShot(audioClipHit);

            // Add force to all the broken (physics) parts of the target
            foreach (Transform child in brokenTarget.transform)
            {
                child.GetComponent<Rigidbody>().AddForce(-transform.forward * destructionForce, ForceMode.Impulse);
            }

            // Remove this from the parent, so it's not counted as an 'active' target
            transform.parent = null;
        }
    }
}