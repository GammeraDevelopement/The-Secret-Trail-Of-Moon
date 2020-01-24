using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PlayStationVRExample
{
    public class SceneSwitcher : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public float fadeSpeed = 2f;

        // Canvas starts fully visible, then fades down after a couple of seconds
        IEnumerator Start()
        {
            canvasGroup.alpha = 1;

            yield return new WaitForSeconds(2f);

            if (FindObjectOfType<VRPostReprojection>())
                FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(false);

            StartCoroutine(FadeDownRoutine());
        }

        // Public call to then call the coroutine (using index)
        public void SwitchToScene(int sceneIndex)
        {
            StartCoroutine("SwitchToSceneRoutine", sceneIndex);
        }

        // Public call to then call the coroutine (using name)
        public void SwitchToScene(string sceneName)
        {
            StartCoroutine("SwitchToSceneRoutine", sceneName);
        }

        IEnumerator FadeDownRoutine()
        {
            if (FindObjectOfType<VRPostReprojection>())
                FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(false);

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
                yield return null;
            }
        }

        // Switch to another scene after a short delay
        IEnumerator SwitchToSceneRoutine(int sceneIndex)
        {
            yield return new WaitForSeconds(0.5f);

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }

            if (FindObjectOfType<VRPostReprojection>())
                FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(true);

            SceneManager.LoadSceneAsync(sceneIndex);
        }

        // Switch to another scene after a short delay
        IEnumerator SwitchToSceneRoutine(string sceneName)
        {
            yield return new WaitForSeconds(0.5f);

            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime * fadeSpeed;
                yield return null;
            }

            if (FindObjectOfType<VRPostReprojection>())
                FindObjectOfType<VRPostReprojection>().busySpinner.SetActive(true);

            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}