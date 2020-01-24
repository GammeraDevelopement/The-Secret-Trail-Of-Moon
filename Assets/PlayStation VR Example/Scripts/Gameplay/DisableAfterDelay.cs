using UnityEngine;
using System.Collections;

namespace PlayStationVRExample
{
    public class DisableAfterDelay : MonoBehaviour
    {
        public float delay = 5f;

        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}