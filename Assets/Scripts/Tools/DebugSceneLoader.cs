using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugSceneLoader : MonoBehaviour {

    public string sceneName;
    public Image black;
    private Dropdown dd;

    private void Start() {
        Dropdown drop = gameObject.GetComponent<Dropdown>();
        if(drop != null)
            dd = drop;
        black.CrossFadeAlpha(0, 1, true);
    }

    public void loadScene() {
        StartCoroutine(LoadNewScene());
        
    }

    public void setScene(string scene) {
        sceneName = scene;
    }

    IEnumerator LoadNewScene() {
        AsyncOperation async;
        black.CrossFadeAlpha(1, 0.5F, true);
        yield return new WaitForSeconds(3);
        if (dd != null) {
            Debug.Log("Scene " + dd.value + " loaded.");
            async = SceneManager.LoadSceneAsync(dd.captionText.text);
        } else {
            async = SceneManager.LoadSceneAsync(sceneName);
        }
        
        while (!async.isDone) {
            yield return null;
        }

    }


}
