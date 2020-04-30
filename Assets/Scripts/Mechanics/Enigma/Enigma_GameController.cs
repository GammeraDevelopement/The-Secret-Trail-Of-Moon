using PlayStationVRExample;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enigma_GameController : MonoBehaviour {

    private const string TURNAROUND_ANIMATION = "animated";
    private const string CONFIRM_BUTTON = "Cross";
    private const string JOKER_BUTTON = "Triangle";

    enum EnimgaFSM {
        LOADING,
        INSTRUCTION,
        MEMORIZING,
        NEWSERIE,
        PLAYING,
        FINISHED
    }

    EnimgaFSM estado = EnimgaFSM.LOADING;

    [Header("Variables del psicólogo")]
    public int nivel = 1;
    public int numeroRuedas = 1;
    public int numeroElementos; //Numero estímulos
    public float tiempo; //Tiempo de memorización
    public float tiempoDisponible; //Tiempo total 
    public int comodines;
    public Slider slideNivel;

    [Header("Variables del editor")]
    public Enigma_ObectInteraction obectInteraction;
    public GameObject wheelStand;
    public GameObject codes;
    public VRUIController vrui;
    public Enigma_ObectInteraction oi;
    public Image[] chessImages;
    public Image[] wrongWheelImage;
    [Tooltip("Mínimo 6 imágenes.")]
    public Sprite[] codeImages;
    public GameObject instruction;

    [Header("Variables del HUD")]
    public Text timeleftText;
    public Text totalTimeLeftText;
    public Text seriesCorrectasText;
    public Text comodinesText;
    public GameObject winBoard;
    public Image black;

    [Header("Variables del script")]
    private GameObject[] rueda;
    private GameObject[] particulas;
    private Image[] imagesObjects;
    private Image[] chessCodeImage;
    private Sprite[] codigoPared;
    private GameObject player;
    private int[] solucion;
    private int[] solucionPropuesta;
    private bool[] answer;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip serie;
    public AudioClip joker;
    public AudioClip turnaround;

    private Animator wheelAnimator;
    private float timeLeft;
    private float totalTimeLeft;
    private int comodinesActuales;
    private int seriesCorrectas = 0;
    private int errores = 0;
    private int pistas = 0;
    private bool loadingScene = false;

    void Start() {
        Debug.Log("Estado loadDificulty:" + estado);
        this.nivel = (int)slideNivel.value;
        setDifficulties(PlayerPrefs.GetInt("nivelEnigma"));
        Begin();
        obectInteraction.nElementos = numeroElementos;
        Debug.Log("Estado actual:" + estado);
    }

    public void loadDificulty() {
        
    }

    private void Begin() {
        Debug.Log("Estado Begin:" + estado);
        //vrui.enabled = true;

        oi.enabled = true;
        solucion = new int[numeroRuedas];
        solucionPropuesta = new int[numeroRuedas];
        answer = new bool[numeroRuedas];
        rueda = new GameObject[wheelStand.transform.childCount];
        particulas = new GameObject[rueda.Length];
        imagesObjects = new Image[rueda.Length];
        chessCodeImage = new Image[numeroRuedas];
        player = GameObject.FindGameObjectWithTag("Player");

        codigoPared = new Sprite[codeImages.Length];

        wheelAnimator = wheelStand.transform.parent.parent.GetComponent<Animator>();
        wheelAnimator.SetBool(TURNAROUND_ANIMATION, true);

        // Get wheels
        int i = 0;
        foreach (Transform child in wheelStand.transform) {
            rueda[i] = child.gameObject;
            i++;
        }

        // Get images
        i = imagesObjects.Length - 1;
        foreach (Transform child in codes.transform) {
            imagesObjects[i] = child.gameObject.GetComponent<Image>();
            i--;
        }

        // Get sprites for the wall
        for (i = 0; i < codeImages.Length; i++) {
            codigoPared[i] = codeImages[i];
        }

        for (i = 0; i < numeroElementos; i++) {
            chessImages[i].gameObject.SetActive(true);
        }

        //Get particles for effect
        for (i = 0; i < rueda.Length; i++) {
            particulas[i] = rueda[i].transform.GetChild(3).gameObject;
        }

        //Se obtiene la clave de la traducción y se aleatoriza
        codigoPared = shuffleList(new List<Sprite>(codigoPared)).ToArray(); 
        for (int j = 0; j < numeroElementos; j++) {
            imagesObjects[j].gameObject.SetActive(true);
            imagesObjects[j].sprite = codigoPared[j];
        }
        for (i = 0; i < numeroRuedas; i++) {
            rueda[i].SetActive(true);

            // Get chess images
            int childrenOfTheWheel = rueda[i].transform.childCount - 1;
            chessCodeImage[i] = rueda[i].transform.GetChild(childrenOfTheWheel).GetChild(0).GetComponent<Image>();

            //Selecciona las imágenes que aparecerán en la rueda
            for (int j = 0; j < numeroElementos; j++) {
                GameObject wheel = rueda[i].transform.GetChild(0).gameObject;
                GameObject canvas = wheel.transform.GetChild(0).gameObject;
                GameObject childTarget = canvas.transform.GetChild(j).gameObject;
                childTarget.GetComponent<Image>().sprite = codigoPared[(numeroElementos -1)- j];
            }
        }

        solucion = new int[numeroRuedas];
        timeLeft = tiempo;
        totalTimeLeft = tiempoDisponible;
        comodinesActuales = comodines;
        Debug.Log("Estado actual:" + estado);
    }

    private void StartPlaying() {
        estado = EnimgaFSM.MEMORIZING;
    }

    void Update() {
        seriesCorrectasText.text = seriesCorrectas + "";
        comodinesText.text = comodinesActuales + "";
        totalTimeLeftText.text = "" + Mathf.Round(totalTimeLeft) + "";

        switch (estado) {

            case EnimgaFSM.LOADING:
                if (nivel > 1) {
                    instruction.gameObject.GetComponent<InstructionScreen>().startNoTutorial();
                } else {
                    instruction.gameObject.GetComponent<InstructionScreen>().startInstruction();
                }
                black.CrossFadeAlpha(0, 0.5F, true);
                estado = EnimgaFSM.INSTRUCTION;
                break;

            case EnimgaFSM.INSTRUCTION:
                break;

            case EnimgaFSM.MEMORIZING:
                if (timeLeft >= 0) {
                    timeLeft = timeLeft - Time.deltaTime;
                    timeleftText.text = "" + Mathf.Round(timeLeft) + "";
                } else {
                    wheelAnimator.SetBool(TURNAROUND_ANIMATION, false);
                    source.clip = turnaround;
                    source.Play();
                    estado = EnimgaFSM.NEWSERIE;
                }
                
                break;

            case EnimgaFSM.NEWSERIE:
                for (int i = 0; i < numeroRuedas; i++) {
                    solucion[i] = Random.Range(1, numeroElementos + 1);
                    chessCodeImage[i].sprite = chessImages[solucion[i]-1].sprite;
                }

                GameObject[] ruedasACambiarDePosicion = new GameObject[numeroElementos];
                //Randomizar las posiciones de las imágenes de la naturaleza en las ruedas
                for (int i = 0; i < numeroRuedas; i++) {
                    
                    for (int j = 0; j < numeroElementos; j++) {
                        ruedasACambiarDePosicion[j] = rueda[i].transform.GetChild(0).GetChild(0).GetChild(j).gameObject;
                    }

                    if (nivel > 13) {
                        Vector3 aux;
                        Quaternion aux2;
                        System.Random rand = new System.Random();
                        int max = rand.Next(numeroElementos - 1);
                        aux = ruedasACambiarDePosicion[0].transform.position;
                        ruedasACambiarDePosicion[0].transform.position = ruedasACambiarDePosicion[max].transform.position;
                        ruedasACambiarDePosicion[max].transform.position = aux;
                        aux2 = ruedasACambiarDePosicion[0].transform.rotation;
                        ruedasACambiarDePosicion[0].transform.rotation = ruedasACambiarDePosicion[max].transform.rotation;
                        ruedasACambiarDePosicion[max].transform.rotation = aux2;
                    }
                }

                estado = EnimgaFSM.PLAYING;
                break;

            case EnimgaFSM.PLAYING:
                if (totalTimeLeft >= 0) {
                    totalTimeLeft = totalTimeLeft - Time.deltaTime;
                    totalTimeLeftText.text = "" + Mathf.Round(totalTimeLeft) + "";
                } else {
                    wheelAnimator.SetBool(TURNAROUND_ANIMATION, true);
                    source.clip = serie;
                    source.Play();
                    winBoard.SetActive(true);
                    winBoard.transform.GetChild(1).GetComponent<TMP_Text>().text = "Aciertos:" + seriesCorrectas + " Errores:" + errores;
                    estado = EnimgaFSM.FINISHED;
                }
                //Si pulsa el botón x
                if (Input.GetButtonDown(CONFIRM_BUTTON)) {
                    bool ganar = true;
                    answer = checkCode();
                    for (int i = 0; i < numeroRuedas; i++) {
                        if (!answer[i]) {
                            ganar = false;
                            wrongWheelImage[i].transform.parent.gameObject.SetActive(true);
                            errores++;
                        } else {
                            wrongWheelImage[i].transform.parent.gameObject.SetActive(false);
                        }
                    }
                    if (ganar) {
                        seriesCorrectas++;
                        Debug.Log("Serie correcta");
                        source.clip = serie;
                        source.Play();
                        for (int i = 0; i < rueda.Length; i++) {
                            particulas[i].GetComponent<ParticleSystem>().Play();
                        }
                        estado = EnimgaFSM.NEWSERIE;
                    }
                }

                //Si pulsa el botón triángulo
                if (Input.GetButtonDown(JOKER_BUTTON)) {
                    wheelAnimator.SetBool(TURNAROUND_ANIMATION, true);
                    timeLeft = tiempo + 1;
                    if (comodinesActuales <= 0) {
                        pistas++;
                        comodinesText.color = Color.red;
                        comodinesActuales--;
                    } else {
                        comodinesActuales--;
                    }
                    pistas++;
                    source.clip = joker;
                    source.PlayDelayed(0.5F);
                    estado = EnimgaFSM.MEMORIZING;
                }
                break;

            case EnimgaFSM.FINISHED:
                if (Input.GetButtonDown("Square") && !loadingScene) {
                    black.CrossFadeAlpha(1, 0.5F, true);
                    loadingScene = true;
                    AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
                }
                break;
        }

    }

    private bool[] checkCode() {
        for (int i = 0; i < numeroRuedas; i++) {
            solucionPropuesta[i] = rueda[i].transform.GetChild(1).GetComponent<Enigma_Notch>().getTargetName();
        }

        for (int i = 0; i < numeroRuedas; i++) {
            if (solucionPropuesta[i] == solucion[i]) {
                answer[i] = true;
            } else {
                answer[i] = false;
            }
        }
        return answer;
    }

    public void setDifficulties(int nivel) {
        Enigma_Dificultad dif = GetComponent<Enigma_Dificultad>();
        this.nivel = nivel;
        numeroRuedas = dif.getDificultad(nivel).NRuedas;
        numeroElementos = dif.getDificultad(nivel).NElementos;
        tiempo = dif.getDificultad(nivel).Tiempo;
        tiempoDisponible = dif.getDificultad(nivel).TiempoDisponible;
        comodines = dif.getDificultad(nivel).Comodines;
    }

    //Randomiza los elementos a traducir del código
    private List<E> shuffleList<E>(List<E> inputList) {
        List<E> randomList = new List<E>();
        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0) {
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }
        return randomList; //return the new random list
    }

    public static System.Random rand = new System.Random();
    //Randomiza los elementos a traducir del código
    private List<E> shuffleList2<E>(List<E> inputList) {
        List<E> lst = new List<E>(inputList);
        for (int i = lst.Count - 1; i >= 1; i--) {
            int j = rand.Next(0, i + 1);
            E tmp = lst[j];
            lst[j] = lst[i];
            lst[i] = tmp;
        }
        return lst;
    }


    //Randomiza la posición de cada imagen dentro de la rueda para que no vayan en orden
    private Vector3[] randomizeTargetPosition(Vector3[] targets) {
        Debug.Log("is randomizing!!!");
        List<Vector3> listaTargets = new List<Vector3>();
        for (int i = 0; i < targets.Length ; i++) {
            listaTargets.Add(targets[i]);
        }
        listaTargets = shuffleList(listaTargets);

        targets = listaTargets.ToArray();
        return targets;
    }

}
