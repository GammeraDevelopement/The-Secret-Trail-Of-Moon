using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sokoban_LoadSokoban : MonoBehaviour
{
    public Transform sokobanPlace;
    public Dropdown dropdown;
    public GameObject[] sokobanes;
    public GameObject canvas;

    public GameObject sokobanACargar;

    // Start is called before the first frame update
    void Start()
    {
        sokobanACargar = Resources.Load("Sokoban Levels/Sokoban" + PlayerPrefs.GetInt("nivelSokoban")) as GameObject;
    }

    public void loadSokoban() {
        for (int i = 0; i < sokobanes.Length; i++) {
            if (sokobanes[i].name == dropdown.captionText.text) {
                sokobanACargar = sokobanes[i];
            }
        }
        Instantiate(sokobanACargar, sokobanPlace.transform.position, sokobanPlace.transform.rotation);

        canvas.SetActive(false);

    }

    public void loadLevel() {
        Instantiate(sokobanACargar, sokobanPlace.transform.position, sokobanPlace.transform.rotation);
    }

    public int getLevel() {
        return PlayerPrefs.GetInt("nivelSokoban");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
