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
    

    public void NextScene()     //Carga la siguiente escena
    {
        /*if (SceneManager.GetActiveScene().Equals(22))
        {

        }*/

        if (PlayerPrefs.GetInt("escena1") == 0)
        {
            PlayerPrefs.SetInt("escena1", 1);
            SceneManager.LoadSceneAsync(1);
        }
        else if (PlayerPrefs.GetInt("escena1") == 1)
        {
            PlayerPrefs.SetInt("escena1", 0);
            SceneManager.LoadSceneAsync(23);        //Cambiar a EXP cuando terminado (21)
        }

        if (PlayerPrefs.GetInt("escena2") == 0)
        {
            PlayerPrefs.SetInt("escena2", 1);
            SceneManager.LoadSceneAsync(3);
        }
        else if (PlayerPrefs.GetInt("escena2") == 1)
        {
            PlayerPrefs.SetInt("escena2", 0);
            SceneManager.LoadSceneAsync(24);        //Cambiar a EXP cuadno terminado (21)
        }



    }

}
