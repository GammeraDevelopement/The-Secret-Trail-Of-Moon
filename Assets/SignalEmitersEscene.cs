using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SignalEmitersEscene : MonoBehaviour
{

    [Header("Timelines")]
    private PlayableDirector director;
    public GameObject controlPanel;

    void Start()
    {
      
        if (PlayerPrefs.GetInt("escena1") == 1) {
            director = GetComponent<PlayableDirector>();
            director.time = 25;
        }
        if (PlayerPrefs.GetInt("escena2") == 1) {
            director = GetComponent<PlayableDirector>();
            director.time = 35;
        }
    }
    

    public void NextScene()     //Carga la siguiente escena
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Escena1"))
        {
            PlayerPrefs.SetInt("Savegame", 1);

            if (PlayerPrefs.GetInt("escena1") == 0)
            {
                PlayerPrefs.SetInt("escena1", 1);
                PlayerPrefs.SetInt("nivelCSAT", 1);
                SceneManager.LoadSceneAsync("Smasher Escena1");
            }
            else if (PlayerPrefs.GetInt("escena1") == 1)
            {
                PlayerPrefs.SetInt("escena1", 0);
                SceneManager.LoadSceneAsync("Escena2");        //Cambiar a EXP cuando terminado (21)
            }
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Escena2"))
        {

            PlayerPrefs.SetInt("Savegame", 2);

            if (PlayerPrefs.GetInt("escena2") == 0)
            {
                PlayerPrefs.SetInt("escena2", 1);
                PlayerPrefs.SetInt("nivelCubos", 1);
                SceneManager.LoadSceneAsync("Cubos");
            }
            else if (PlayerPrefs.GetInt("escena2") == 1)
            {
                PlayerPrefs.SetInt("escena2", 0);
                SceneManager.LoadSceneAsync("Escena3");        //Cambiar a EXP cuadno terminado (21)
            }
        }



        



    }

}
