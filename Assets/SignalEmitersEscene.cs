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

        if (PlayerPrefs.GetInt("escena1") == 1)
        {
            director = GetComponent<PlayableDirector>();
            director.time = 25;
        }
        else if (PlayerPrefs.GetInt("escena2") == 1)
        {
            director = GetComponent<PlayableDirector>();
            director.time = 35;
        }
        else if (PlayerPrefs.GetInt("escena3") == 1)
        {
            director = GetComponent<PlayableDirector>();
            director.time = 20;
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
                PlayerPrefs.SetInt("nivelCSAT", 2);
                SceneManager.LoadSceneAsync("CSAT Cueva");
            }
            else if (PlayerPrefs.GetInt("escena1") == 1)
            {
                PlayerPrefs.SetInt("escena1", 0);
                PlayerPrefs.SetInt("Savegame", 2);
                SceneManager.LoadSceneAsync("Escena2");        //Cambiar a EXP cuando terminado (21)
            }
        } // Escena1

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Escena2"))
        {

            PlayerPrefs.SetInt("Savegame", 2);

            if (PlayerPrefs.GetInt("escena2") == 0)
            {
                PlayerPrefs.SetInt("escena2", 1);
                PlayerPrefs.SetInt("nivelCubos", 2);
                SceneManager.LoadSceneAsync("Kuburi Escena2");
            }
            else if (PlayerPrefs.GetInt("escena2") == 1)
            {
                PlayerPrefs.SetInt("escena2", 0);
                SceneManager.LoadSceneAsync("Escena3");        //Cambiar a EXP cuadno terminado (21)
            }
        }//Escena2

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Escena3"))
        {

            PlayerPrefs.SetInt("Savegame", 3);

            if (PlayerPrefs.GetInt("escena3") == 0)
            {
                PlayerPrefs.SetInt("escena3", 1);
                PlayerPrefs.SetInt("nivelEnigma", 2);
                SceneManager.LoadSceneAsync("Enigma Working Memory");
            }
            else if (PlayerPrefs.GetInt("escena3") == 1)
            {
                PlayerPrefs.SetInt("escena3", 0);
                SceneManager.LoadSceneAsync("Intro");        //Cambiar a EXP cuadno terminado (21)   Y ESCENA 4
            }
        }



        



    }

}
