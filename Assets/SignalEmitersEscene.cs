using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
    }
    
    //SUSTITUIR POR CASE CUANDO SE AVANCE

    public void NextScene()     //Carga la siguiente escena
    {

        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            PlayerPrefs.SetInt("escena1", 1);
            SceneManager.LoadSceneAsync(1);
        }
        else if (PlayerPrefs.GetInt("escena1") == 1)
        {
            PlayerPrefs.SetInt("escena1", 0);
            SceneManager.LoadSceneAsync(21);
        }

    }

}
