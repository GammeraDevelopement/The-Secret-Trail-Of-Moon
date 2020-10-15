using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeInitialization : MonoBehaviour
{

    public GameObject sceneLoader;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Gamemode", 0);
        PlayerPrefs.SetInt("Savegame", 0);
    }

    public void ChangeGamemode(int value) {

        PlayerPrefs.SetInt("Gamemode", value);
        /*
        if (PlayerPrefs.GetInt("Gamemode") == 2)
        {
            if (PlayerPrefs.GetInt("Savegame") != 0)
            {
                botonContinuar.SetActive(true);
            }
        }
        //Continue? si -> loadScene("Escena"+Savegame), no->loadScene("Escena1") */
    }
    
    /*
    public void PartidaNueva() {

        PlayerPrefs.SetInt("Savegame", 0);
    }*/
}
