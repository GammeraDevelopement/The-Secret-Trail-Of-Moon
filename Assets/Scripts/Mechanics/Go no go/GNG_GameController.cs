using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GNG_GameController : MonoBehaviour {

    public enum GonogoFSM {
        LOADING,
        INSTRUCTION,
        FIRSTROUND,
        SECONDROUND,
        FINISHED_WIN,
        FINISHED_LOSE
    }
    public GonogoFSM estado = GonogoFSM.LOADING;
    public InstructionScreen instruction;

    public float speed = 10F;
    public Terrain[] terrenos;
    public Transform deathPlace;
    public Transform birthPlace;

    [Header("Variables del psicólogo")]
    public int nivelActual = 1;
    public int nMaxElemRonda = 20;
    public int nElementos = 2;
    public int erroresMaxRonda = 10;
    public int comodinesRonda;
    public int aciertosParaComodin;
    public int targetsPassed = 0;

    public GNG_GenerateTargets geC;

    private bool corrutinaActive = false;
    private byte elemCounter;

    [Header("DataManager")]
    public GNG_DifficultyJSON data;
    public GonoGoData_Save dataSave;
    public DataManager dataManager;

    [Header("GUI")]
    public Canvas canvasStart;
    public Canvas canvasGame;
    public GameObject initialPopup;
    public TMP_Text textState;
    public Image black;
    public GameObject winBoard;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip acierto;
    public AudioClip error;
    
    private int errorCount { get; set; }
    private int errorOmisionCount { get; set; }
    private int aciertoCount { get; set; }
    private int aciertoOutput = 0;
    private int comodines = 0;
    private float timeReaction;

    public float sizeZ;

    private bool startGame = false;

    /// <summary>
    /// Variable que lleva la cuenta de cuántos elementos has atravesado
    /// </summary>
    private int elemRun;

    #region progressBar
    private float distanceBar;
    private float distanceToRun;
    private float distanceToRunFox;
    private float barStartPosition;
    private float barStartPositionFox;

    [Header("Progress Bar")]
    public Transform movi; //raccon
    public TMP_Text textoComodines;
    public Transform one; //fox
    public RectTransform bar;
    private bool loadingScene;
    private int gamemode;
    private AudioClip finishedSound;
    #endregion progressBar

    void Start() {
        /*timeReaction = data.getDificultad(nivelActual).TiempoReaccionMax / 1000;
        sizeZ = speed * timeReaction;*/

        nivelActual = PlayerPrefs.GetInt("nivelGonogo");

        //Inicialización de elementos mediante JSON
        nElementos = data.getDificultad(nivelActual).NElementos;
        nMaxElemRonda = data.getDificultad(nivelActual).NElemXRonda;
        erroresMaxRonda = data.getDificultad(nivelActual).NErroresMaxXRonda;
        comodinesRonda = data.getDificultad(nivelActual).NComodines;
        aciertosParaComodin = data.getDificultad(nivelActual).NAciertosSeguidosComodin;


        distanceBar = bar.sizeDelta.x;
        barStartPosition = (-distanceBar / 2);
        barStartPositionFox = barStartPosition + (distanceBar / data.getDificultad(nivelActual).NErroresMaxXRonda);

        distanceToRun = distanceBar / data.getDificultad(nivelActual).NElemTotal;
        distanceToRunFox = (distanceBar + barStartPositionFox / 2) / data.getDificultad(nivelActual).NElemTotal;

        movi.localPosition = new Vector3(barStartPosition, movi.localPosition.y, movi.localPosition.z);
        //Fox position is the beggining of the bar + max errors
        one.localPosition = new Vector3(barStartPositionFox, one.localPosition.y, one.localPosition.z);

        comodines = comodinesRonda;

        estado = GonogoFSM.LOADING;
    }

    private void StartPlaying() {

        textState.text = "PRIMERA RONDA";
        textState.gameObject.SetActive(true);
        textState.CrossFadeAlpha(0, 2.5F, true);

        
        estado = GonogoFSM.FIRSTROUND;
        StartCoroutine(geC.GenerateRound());
        initialPopup.SetActive(true);
    }

    // Update is called once per frame
    void Update() {

        #region Estados

        switch (estado) {
            case GonogoFSM.LOADING:
                black.CrossFadeAlpha(0, 0.5F, true);
                estado = GonogoFSM.INSTRUCTION;
                break;
            case GonogoFSM.INSTRUCTION:
                break;

            case GonogoFSM.FIRSTROUND:
                Terrenos(); //Movimiento del terreno

                if (targetsPassed == nMaxElemRonda) {
                    elemCounter = 0;
                    errorCount = 0; //Se cuentan por ronda. 
                    elemRun = 0;
                    targetsPassed = 0;


                    textState.text = "SEGUNDA RONDA";
                    textState.gameObject.SetActive(true);
                    textState.CrossFadeAlpha(1, 0, true);
                    textState.CrossFadeAlpha(0, 2.5F, true);

                    //TODO: Animación de texto que aparece aquí y se va ocultando.

                    estado = GonogoFSM.SECONDROUND;
                    StartCoroutine(geC.GenerateRound());

                }
                break;
            case GonogoFSM.SECONDROUND:
                Terrenos();

                if (targetsPassed == nMaxElemRonda) {
                    textState.text = "¡HAS GANADO!";
                    textState.gameObject.SetActive(true);

                    //TODO: Animación de texto que aparece aquí y se va ocultando.

                    estado = GonogoFSM.FINISHED_WIN;
                }

                break;
            case GonogoFSM.FINISHED_WIN:

                GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().clip = finishedSound;
                GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().Play();
                winBoard.SetActive(true);

                winBoard.transform.GetChild(1).GetComponent<TMP_Text>().text =  "";
                
                gamemode = PlayerPrefs.GetInt("Gamemode");
                switch (gamemode)
                {
                    case 0:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
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
            case GonogoFSM.FINISHED_LOSE:

                winBoard.transform.GetChild(1).GetComponent<TMP_Text>().text = aciertoCount + " " + errorOmisionCount + " " + errorCount;

                gamemode = PlayerPrefs.GetInt("Gamemode");
                switch (gamemode)
                {
                    case 0:
                        if (Input.GetButtonDown("Square") && !loadingScene)
                        {
                            black.CrossFadeAlpha(1, 0.5F, true);
                            loadingScene = true;
                            AsyncOperation async = SceneManager.LoadSceneAsync("Intro");
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

        #endregion Estados

        textoComodines.text = comodines.ToString();
        if (errorCount == data.getDificultad(nivelActual).NErroresMaxXRonda) {
            textState.text = "HAS PERDIDO";
            textState.gameObject.SetActive(true);
            estado = GonogoFSM.FINISHED_LOSE;
        }
        if (aciertoCount >= data.getDificultad(nivelActual).NAciertosSeguidosComodin && comodines != data.getDificultad(nivelActual).NComodines) {
            comodines++;
            aciertoCount -= 3;
        }

    }

    void Terrenos() {
        float step = speed * Time.deltaTime;
        #region Terrenos
        for (int i = 0; i < terrenos.Length; i++) {
            terrenos[i].transform.position = Vector3.MoveTowards(terrenos[i].transform.position, deathPlace.position, step);
        }
        for (int i = 0; i < terrenos.Length; i++) {
            if (terrenos[i].transform.position == deathPlace.position) terrenos[i].transform.position = birthPlace.transform.position;
        }
        #endregion Terrenos
    }

    public void movementAnimalsUI(Transform animal, float _distanceToRun) {
        animal.localPosition += new Vector3(_distanceToRun, 0, 0);
    }

    #region Getters
    public bool get_introduction() {
        return estado == GonogoFSM.INSTRUCTION;
    }
    public bool get_loading() {
        return estado == GonogoFSM.LOADING;
    }
    public bool get_stateFirstRound() {
        if (estado!=GonogoFSM.FIRSTROUND) return false;
        else return estado == GonogoFSM.FIRSTROUND;
    }
    public bool get_stateSecondRound() {
        if (estado != GonogoFSM.SECONDROUND) return false;
        else return estado == GonogoFSM.SECONDROUND;
    }
    #endregion Getters

    #region Setters
    public void set_plus_elemCounter() {
        elemCounter++;
    }

    public void set_plus_errorCount(bool omision) {
        if (comodines > 0)   //Como si hubieses acertado.
        {
            audioSource.clip = error;
            audioSource.Play();

            comodines--;
            

        } else {
            audioSource.clip = error;
            audioSource.Play();
            if (omision) {
                errorOmisionCount++;
            } else {
                errorCount++;
            }
            movementAnimalsUI(movi, distanceToRun);
            aciertoCount = 0;
        }
    }
    public void set_plus_aciertoCount() {
        audioSource.clip = acierto;
        audioSource.Play();

        aciertoCount++;
        aciertoOutput++;
        movementAnimalsUI(one, distanceToRunFox);
        movementAnimalsUI(movi, distanceToRun);
    }
    public void set_elemRunCount()  //Elements crossed
    {
        elemRun++;

        movementAnimalsUI(movi, distanceToRun);
    }
    #endregion Setters


}
