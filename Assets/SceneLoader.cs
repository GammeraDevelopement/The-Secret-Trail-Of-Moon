using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private bool loadScene = false;

    [SerializeField]
    private int scene = 0;
    [SerializeField]
    public Text loadingText;

    public void setCSATLevel(int level) { PlayerPrefs.SetInt("nivelCSAT", level); }
    public void setSokobanLevel(int level) { PlayerPrefs.SetInt("nivelSokoban", level); }
    public void setEnigmaLevel(int level) { PlayerPrefs.SetInt("nivelEnigma", level); }
    public void setCubosLevel(int level) { PlayerPrefs.SetInt("nivelCubos", level); }
    public void setGonogoLevel(int level) { PlayerPrefs.SetInt("nivelGonogo", level); }

    private void Start() {

    }

    void Update() {

        /*if (loadScene == false) {


            loadScene = true;
            loadingText.text = "Loading...";
            StartCoroutine(LoadNewScene());

        }

        if (loadScene == true) {
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }*/

    }

    public void LoadSceneInOrder() {

        string sceneString = PlayerPrefs.GetString("SceneString");
        string[] currentScene = sceneString.Split(';');
        string[] escena = currentScene[0].Split(':');

        switch (escena[0]) {
            case "smasher":
                scene = 1;
                setCSATLevel(int.Parse(escena[1]));
                break;
            case "tekateki":
                scene = 2;
                setSokobanLevel(int.Parse(escena[1]));
                break;
            case "kuburi":
                scene = 3;
                setCubosLevel(int.Parse(escena[1]));
                break;
            case "enigma":
                scene = 4;
                setEnigmaLevel(int.Parse(escena[1]));
                break;
            case "kitsune":
                scene = 5;
                setGonogoLevel(int.Parse(escena[1]));
                break;
            case "chess":
                switch (escena[1]) {
                    case "Torre":
                        scene = 6;
                        break;
                    case "Alfil":
                        scene = 7;
                        break;
                    case "Caballo":
                        scene = 9;
                        break;
                    case "Peón":
                        scene = 10;
                        break;
                    case "Reina":
                        scene = 11;
                        break;
                    case "Rey":
                        scene = 8;
                        break;
                    case "Jaque Mate 1":
                        scene = 13;
                        break;
                    case "Jaque Mate 2":
                        scene = 17;
                        break;
                    case "Jaque Mate 3":
                        scene = 18;
                        break;
                    case "Repaso 1":
                        scene = 15;
                        break;
                    case "Repaso 2":
                        scene = 16;
                        break;
                    case "Tutorial":
                        scene = 14;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        Debug.Log("Loading Scene number:" + scene + " with level number:" + escena[1]);
        StartCoroutine(LoadNewScene());

        sceneString = "";
        for (int i = 0; i < currentScene.Length; i++) {
            if (i > 1) {
                sceneString += currentScene[i] + ";" ;
            }
        }
        PlayerPrefs.SetString("SceneString", sceneString);
        Debug.Log("sceneString: " + PlayerPrefs.GetString("SceneString"));

    }


    IEnumerator LoadNewScene() {

        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone) {
            yield return null;
        }

    }

}