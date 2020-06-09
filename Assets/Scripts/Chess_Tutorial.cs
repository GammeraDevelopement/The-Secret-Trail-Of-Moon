using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chess_Tutorial : MonoBehaviour {
    private const string CONFIRM = "Cross";
    private const string CANCEL = "Circle";


    public enum ChessTutorialFSM {
        LOADING,
        INSTRUCTION,
        NEXT_LEVEL,
        PLAYING,
        FINISHED
    }
    public ChessTutorialFSM state;

    //Variables del editor
    public GameObject[] ejercicios;
    public Transform piecePlace;
    public AudioSource source;
    public AudioClip right;
    public AudioClip wrong;

    //Variables de GUI
    public TMP_Text[] textos; //debe haber 4
    public string[] nombresConPosiciones; //debe haber 12 (4 * 3)
    public Image black;
    public GameObject winCartel;

    //Variables de programacións
    private bool[] aciertos;
    private GameObject player;
    private GameObject clone;
    private int posiciones = 0;
    private int jugada = 0;
    private bool loadingScene = false;

    // Start is called before the first frame update
    void Start() {
        aciertos = new bool[4];
        player = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void StartPlaying() {
        state = ChessTutorialFSM.NEXT_LEVEL;
    }

    // Update is called once per frame
    void Update() {
        switch (state) {
            case ChessTutorialFSM.LOADING:   //----------------------------------------------------------------------------------------------- LOADING
                black.CrossFadeAlpha(0, 0.5F, true);
                break;
            case ChessTutorialFSM.INSTRUCTION:    //------------------------------------------------------------------------------------------ INSTRUCTION
                //instruccion
                break;
            case ChessTutorialFSM.NEXT_LEVEL:   //-------------------------------------------------------------------------------------------- NEXT LEVEL

                //Reset variables
                for (int i = 0; i < aciertos.Length; i++) {
                    aciertos[i] = false;
                }
                Destroy(clone);
                //Instancia la siguiente jugada
                clone = Instantiate(ejercicios[jugada], piecePlace.transform.position, piecePlace.transform.rotation);
                jugada++;
                //Agrega los 4 siguientes textos al panel de posiciones
                if (posiciones >= 11) posiciones = 0;
                for (int i = 0; i < 4; i++) {
                    textos[i].text = nombresConPosiciones[i + posiciones];
                }
                posiciones += 4;

                state = ChessTutorialFSM.PLAYING;
                break;


            case ChessTutorialFSM.PLAYING:     //--------------------------------------------------------------------------------------------- PLAYING

                if (Input.GetButtonDown(CONFIRM)) {

                    Ray ray = new Ray(player.transform.position, player.transform.forward);
                    if (Physics.Raycast(ray, out RaycastHit hit, 100, ~(1 << 11))) {
                        Debug.Log(hit.collider.gameObject.name);

                        //Comprueba si el nombre de la pieza seleccionada se encuentra entre los cuatro nombres correctos. 
                        //Si está, cambia el array de aciertos en la posición concreta a verdadero. Si no, reproduce un sonido de error.
                        bool existe = false;
                        for (int i = 0; i < aciertos.Length; i++) {
                            if (hit.collider.name == textos[i].text) {

                                Material mate = hit.collider.gameObject.GetComponent<Renderer>().material;
                                mate.SetColor("_EmissionColor", Color.green);

                                source.clip = right;
                                source.Play();

                                aciertos[i] = true;
                                existe = true;
                            }
                        }
                        if (!existe && hit.collider.tag != "back") {
                            source.clip = wrong;
                            source.Play();
                        }

                        //  {--Win--}
                        bool win = true;
                        for (int i = 0; i < aciertos.Length; i++) {
                            if (!aciertos[i]) win = false;
                        }
                        if (win) {
                            if (jugada >= 3) {
                                winCartel.SetActive(true);
                                state = ChessTutorialFSM.FINISHED;
                            } else {
                                state = ChessTutorialFSM.NEXT_LEVEL;
                            }
                        }
                    }
                }
                break;
            case ChessTutorialFSM.FINISHED:    //------------------------------------------------------------------------------------------ FINISHED
                Debug.Log("Win");
                if (Input.GetButtonDown("Square") && !loadingScene) {
                    black.CrossFadeAlpha(1, 0.5F, true);
                    loadingScene = true;
                    AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
                }
                break;
            default:
                break;
        }
    }
}
