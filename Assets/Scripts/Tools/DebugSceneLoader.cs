using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugSceneLoader : MonoBehaviour {

    public string sceneName;
    public Image black;
    private Dropdown dd;
    public GameObject gm;
    public GameObject cont; // continue
    public Button btnAceptar;

    private void Start() {
        Dropdown drop = gameObject.GetComponent<Dropdown>();
        if(drop != null)
            dd = drop;
        black.CrossFadeAlpha(0, 1, true);
    }

    public void loadScene() {
        StartCoroutine(LoadNewScene());
        setScene("Escena1");      // Si algo falla descomentar esto
    }

    public void newGame() {

        if (PlayerPrefs.GetInt("Savegame") == 0)
        {

            gm.GetComponent<GamemodeInitialization>().ChangeGamemode(2);
            //setScene("Escena1");
            loadScene();
            //GamemodeInicitialization = 2
        }
        else {

            cont.SetActive(true);
            PlayerPrefs.SetInt("Gamemode", 2);
            btnAceptar.Select();
        }
    }

    public void setScene(string scene) {

       /* if (PlayerPrefs.GetInt("Gamemode") == 2)
        {
            if (PlayerPrefs.GetInt("Savegame") != 0)
            {
                sceneName = "Escena" + PlayerPrefs.GetInt("Savegame");
            }
            else
            {
                sceneName = "Escena1";
            }
        }
        else
        { */
            sceneName = scene;              // y dejar solo esta linea de codigo
        //}
        
    }


    IEnumerator LoadNewScene() {
        AsyncOperation async;
        black.CrossFadeAlpha(1, 0.5F, true);
        yield return new WaitForSeconds(3);

         if(dd != null) {
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
