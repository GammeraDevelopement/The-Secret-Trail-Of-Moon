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
        
    }

    public void NextScene()     //Carga la siguiente escena
    {

        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            PlayerPrefs.SetInt("escena1", 1);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            PlayerPrefs.SetInt("escena1", 0);
            SceneManager.LoadSceneAsync(21);
        }

    }

    public void StartAt()   //Para que empiece la segunda parte de la escena
    {
        
        Debug.Log("StartAt");
        if (PlayerPrefs.GetInt("escena1") == 1)
        {
            director = GetComponent<PlayableDirector>();
            director.time = 25;
        }
    }
}
