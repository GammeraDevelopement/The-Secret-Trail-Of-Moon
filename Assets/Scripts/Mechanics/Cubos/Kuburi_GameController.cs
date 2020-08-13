using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityStandardAssets.Characters.FirstPerson;
using System;
using System.Collections;

public class Kuburi_GameController : MonoBehaviour
{
    enum CubeFSM {
        LOADING,
        INSTRUCTION,
        WAITING,
        FINISHED
    }
    CubeFSM state = CubeFSM.LOADING;

    public int level;
    [HeaderAttribute("Empieza desde abajo izquierda y va subiendo.")]
    public string[] matrizPropuesta;
    public Sprite[] matrizPared;
    public GameObject[] blocks;
    public GameObject boards;
    public int[] levelSize;
    public int[] cubesAmount;

    public Image imageKey;
    public Light light;
    public Transform faceDetectors;
    public Transform cubes;
    public Transform cubePlace;
    public AudioClip finishedSound;
    
    public FirstPersonController firstPersonController;
    public Kuburi_ObjectInteraction cogerCubos;
    public bool tutorial = true;
    public Image black;
    public GameObject instruction;
    public GameObject winBoard;

    private GameObject[] detectores;
    private string[] matrizSolucion;
    private int childCount;
    private bool loadingScene = false;
    public Timer temporizador;

    private int amountOfDetectors = 0;
    public bool doyouwantcode = false;
    public string code;
    private bool check = true;

    private void Start() {
        level = PlayerPrefs.GetInt("nivelCubos");
        
        //Initialize matrices
        childCount = faceDetectors.childCount;
        switch (levelSize[level]) {
            case 4:
                amountOfDetectors = 16;
                boards.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 5:
                amountOfDetectors = 25;
                boards.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 6:
                amountOfDetectors = 36;
                boards.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 7:
                amountOfDetectors = 49;
                boards.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 8:
                amountOfDetectors = 64;
                boards.transform.GetChild(4).gameObject.SetActive(true);
                break;
            default:
                amountOfDetectors = 0;
                break;
        }

        if (blocks[level] != null) blocks[level].SetActive(true);

        matrizSolucion = new string[amountOfDetectors];
        detectores = new GameObject[amountOfDetectors];

        //Instanciar los cubitos
        for (int i = 0; i < cubesAmount[level]; i++) {
            cubes.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 0; i < amountOfDetectors; i++) {
            faceDetectors.GetChild(i).gameObject.SetActive(true);
            detectores[i] = faceDetectors.GetChild(i).gameObject;
            cubePlace.GetChild(i).gameObject.SetActive(true);
            matrizSolucion[i] = "+";
        }
      
    }

    private void StartPlaying() {
        
        temporizador.startTimer();
        state = CubeFSM.WAITING;
        
    }

    private void Update() {
        switch (state) {
            case CubeFSM.LOADING:
                if (level > 1) {
                    instruction.GetComponent<InstructionScreen>().startNoTutorial();
                } else {
                    instruction.GetComponent<InstructionScreen>().startInstruction();
                }
                state = CubeFSM.INSTRUCTION;
                break;

            case CubeFSM.INSTRUCTION:
                black.CrossFadeAlpha(0, 0.5F, true);
                break;

            case CubeFSM.WAITING:

                imageKey.sprite = matrizPared[level - 1];
                bool complete = true;
                for (int i = 0; i < amountOfDetectors; i++) {
                    matrizSolucion[i] = detectores[i].GetComponent<Kuburi_FaceDetector>().getCara();
                    if (matrizSolucion[i] == "") {
                        complete = false;
                    }
                }

                if (complete) {
                    if (checkWinner()) {
                        Debug.Log("Has ganado");
                        light.gameObject.SetActive(true);
                        light.color = Color.green;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().clip = finishedSound;
                        GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
                        winBoard.SetActive(true);
                        state = CubeFSM.FINISHED;
                    } 
                }

                //For key image implementation only, uncomment if you want to create a key code
                if (doyouwantcode) {
                    checkCodes();
                }
                
                
                break;

            case CubeFSM.FINISHED:
                Debug.Log("Tiempo :" + temporizador.GetMilliseconds() + " Seconds:"+temporizador.GetSeconds());
                winBoard.transform.GetChild(1).GetComponent<TMP_Text>().text = temporizador.GetMinutes() + temporizador.GetSeconds() + "";
                temporizador.stopTimer();
                int minute = temporizador.GetMinutes();
                int second = temporizador.GetSeconds();
                PlayerPrefs.SetInt("MinutosKuburi", minute);
                PlayerPrefs.SetInt("SegundosKuburi", second);
                int gamemode = PlayerPrefs.GetInt("Gamemode");
                switch (gamemode)
                {
                    case 0:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            AsyncOperation async = SceneManager.LoadSceneAsync(0);
                        }
                        break;
                    case 1:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            gameObject.GetComponent<SceneLoader>().LoadSceneInOrder();
                        }
                        break;
                    case 2:
                        break;
                    default:
                        break;
                }
                
                break;
            default:
                break;
        }
    }


    private void checkCodes() {
        /*****************************************************************************/
        /* THIS IS FOR CHECKING KEY CODES AND CREATE NEW IMAGES                      */
        /*****************************************************************************/
        for (int i = 0; i < amountOfDetectors; i++) {
            if (matrizSolucion[i] != null) {
                if (string.IsNullOrEmpty(matrizSolucion[i]) && check) {
                    check = false;
                    Debug.Log("nulo");
                    break;
                }
            }
        }
        if (check) {
            string piece = "";
            for (int i = 0; i < amountOfDetectors; i++) {
                piece += matrizSolucion[i];
            }
            code += piece;
            check = false;
        }
        check = true;
        
    }

    private bool checkWinner() {
        string[] substring = matrizPropuesta[level-1].Split(',');
        for (int i = 0; i < amountOfDetectors; i++) {
            if (!substring[i].Equals(matrizSolucion[i])) {
                return false;
            }
        }
        return true;
    }

}
