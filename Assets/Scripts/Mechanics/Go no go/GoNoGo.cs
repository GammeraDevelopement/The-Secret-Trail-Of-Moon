using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GoNoGo : MonoBehaviour {

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

    public delegate void ControllerDelegate();
    ControllerDelegate CDelegate;

    public GenerateElements_Controller geC;

    private bool corrutinaActive = false;
    private byte elemCounter;

    /// <summary>
    /// Referencia del script GoNoGo_SetData
    /// </summary>
    [Header("DataManager")]
    public GoNoGo_SetData data;
    public GonoGoData_Save dataSave;
    public DataManager dataManager;

    [Header("GUI")]
    public Canvas canvasStart;
    public Canvas canvasGame;
    public TextMeshProUGUI textState;
    public Image black;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip acierto;
    public AudioClip error;
    public int nivelActual = 1;

    private byte elemMaxSpawn;
    private int errorCount;
    private int aciertoCount;
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
    public Transform one; //fox
    public RectTransform bar;
    #endregion progressBar

    void Start() {
        timeReaction = data.getDificultad(nivelActual).TiempoReaccionMax / 1000;
        sizeZ = speed * timeReaction;

        elemMaxSpawn = (byte)(data.getDificultad(nivelActual).NElementos);
        canvasGame.gameObject.SetActive(false);


        distanceBar = bar.sizeDelta.x;


        barStartPosition = (-distanceBar / 2);
        barStartPositionFox = barStartPosition + (distanceBar / data.getDificultad(nivelActual).NErroresMaxXRonda);

        distanceToRun = distanceBar / data.getDificultad(nivelActual).NElemTotal;
        distanceToRunFox = (distanceBar + barStartPositionFox / 2) / data.getDificultad(nivelActual).NElemTotal;

        print(barStartPosition);
        print(barStartPositionFox);

        movi.localPosition = new Vector3(barStartPosition, movi.localPosition.y, movi.localPosition.z);
        //Fox position is the beggining of the bar + max errors
        one.localPosition = new Vector3(barStartPositionFox, one.localPosition.y, one.localPosition.z);

        estado = GonogoFSM.LOADING;
    }

    private void StartPlaying() {

        textState.text = "PRIMERA RONDA";
        textState.gameObject.SetActive(true);

        //Animación de texto que aparece aquí y se va ocultando.
        /*dataManager.Save();
        print("Se ha guardado");*/
        estado = GonogoFSM.FIRSTROUND;
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
                Terrenos();
                if (corrutinaActive == false) {
                    StartCoroutine(geC.GenerateFirstRound(0, elemMaxSpawn));
                    corrutinaActive = true;

                }
                if ((elemRun) == data.getDificultad(nivelActual).NElemXRonda) {
                    elemCounter = 0;
                    errorCount = 0; //Se cuentan por ronda. 
                    elemRun = 0;

                    textState.text = "SEGUNDA RONDA";
                    textState.gameObject.SetActive(true);
                    //Animación de texto que aparece aquí y se va ocultando.
                    estado = GonogoFSM.SECONDROUND;

                    /*
                    dataManager.Save();
                    print("Se ha guardado");*/
                }
                break;
            case GonogoFSM.SECONDROUND:
                Terrenos();

                if (corrutinaActive == true) {
                    print("Hemos cambiado a la segunda ronda");
                    StartCoroutine(geC.GenerateFirstRound(0, elemMaxSpawn));    //Se vuelve a invocar porque le dimos false cuando spawneó todos los elementos de la primera ronda
                    corrutinaActive = false;
                }
                if (elemRun == data.getDificultad(nivelActual).NElemXRonda) {
                    textState.text = "¡HAS GANADO!";
                    textState.gameObject.SetActive(true);

                    //Animación de texto que aparece aquí y se va ocultando.
                    estado = GonogoFSM.FINISHED_WIN;
                    /*
                    dataManager.Save();
                    print("Se ha guardado");*/
                }

                break;
            case GonogoFSM.FINISHED_WIN:
                break;
            case GonogoFSM.FINISHED_LOSE:
                //Recargamos la escena
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                dataManager.Save();
                print("Se ha guardado");
                break;
            default:
                break;
        }

        if (estado == GonogoFSM.FIRSTROUND) {
            Terrenos();
            if (corrutinaActive == false) {
                StartCoroutine(geC.GenerateFirstRound(0, elemMaxSpawn));
                corrutinaActive = true;

            }
            if ((elemRun) == data.getDificultad(nivelActual).NElemXRonda) {
                elemCounter = 0;
                errorCount = 0; //Se cuentan por ronda. 
                elemRun = 0;

                textState.text = "SEGUNDA RONDA";
                textState.gameObject.SetActive(true);
                //Animación de texto que aparece aquí y se va ocultando.
                estado = GonogoFSM.SECONDROUND;
                dataManager.Save();
                print("Se ha guardado");
            }

        } else if (estado == GonogoFSM.SECONDROUND) {
            Terrenos();

            if (corrutinaActive == true) {
                print("Hemos cambiado a la segunda ronda");
                StartCoroutine(geC.GenerateFirstRound(0, elemMaxSpawn));    //Se vuelve a invocar porque le dimos false cuando spawneó todos los elementos de la primera ronda
                corrutinaActive = false;
            }
            if (elemRun == data.getDificultad(nivelActual).NElemXRonda) {
                textState.text = "¡HAS GANADO!";
                textState.gameObject.SetActive(true);
                //Animación de texto que aparece aquí y se va ocultando.
                estado = GonogoFSM.FINISHED_WIN;
                dataManager.Save();
                print("Se ha guardado");
            }
        } else if (estado == GonogoFSM.FINISHED_LOSE) {
            //Recargamos la escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            dataManager.Save();
            print("Se ha guardado");
        }

        #endregion Estados

        if (errorCount == data.getDificultad(nivelActual).NErroresMaxXRonda) {
            textState.text = "HAS PERDIDO";
            textState.gameObject.SetActive(true);
            estado = GonogoFSM.FINISHED_LOSE;
        }
        if (aciertoCount >= data.getDificultad(nivelActual).NAciertosSeguidosComodin && comodines != data.getDificultad(nivelActual).NComodines) {
            comodines++;
            aciertoCount -= 3;
        }
        if (CDelegate != null && (Input.GetAxis("RightStickY") >= 1 && Input.GetAxis("LeftStickY_Test") >= 1)) {//Mientras está jugando, se estará comprobando todo el rato si se está golpeando.
            CDelegate();
        }   //Aquí estamos llamando al delegado para saber si estamos dentro del rango del Trigger del FPS
        if (Input.GetKeyDown(KeyCode.V)) {
            dataManager.Save();
            print("Se ha guardado");
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            if (dataManager.Load())
                print("Se ha cargado");

            else
                print("no se ha podido cargar");
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

    void movementAnimalsUI(Transform animal, float _distanceToRun) {
        animal.localPosition += new Vector3(_distanceToRun, 0, 0);
    }



    public void SuscribeDelegate(ControllerDelegate function) {
        CDelegate += function;
    }
    public void DeSuscribeDelegate(ControllerDelegate function) {
        CDelegate -= function;
    }

    #region Getters
    public bool get_introduction() {
        return estado == GonogoFSM.INSTRUCTION;
    }
    public bool get_loading() {
        return estado == GonogoFSM.LOADING;
    }
    public bool get_stateFirstRound() {
        if (elemCounter == data.getDificultad(nivelActual).NElemXRonda) return false;

        else return estado == GonogoFSM.FIRSTROUND;
    }
    public bool get_stateSecondRound() {
        if (elemCounter == data.getDificultad(nivelActual).NElemXRonda) return false;

        else return estado == GonogoFSM.SECONDROUND;
    }
    #endregion Getters

    #region Setters
    public void set_plus_elemCounter() {
        elemCounter++;
    }

    public void set_plus_errorCount() {
        if (comodines > 0)   //Como si hubieses acertado.
        {
            audioSource.clip = error;
            audioSource.Play();

            comodines--;
            //Preguntar si quieren que se le cuente este error para los datos que ellas recogen.
            aciertoCount = 0;
            movementAnimalsUI(movi, distanceToRun);
        } else {
            audioSource.clip = error;
            audioSource.Play();

            errorCount++;
            aciertoCount = 0;
        }

        if (estado == GonogoFSM.SECONDROUND)
            dataSave.set_errorXContinuacion();
    }
    public void set_plus_aciertoCount() {
        audioSource.clip = acierto;
        audioSource.Play();

        aciertoCount++;
        aciertoOutput++;
        movementAnimalsUI(one, distanceToRunFox);
        if (estado == GonogoFSM.FIRSTROUND)
            dataSave.set_aciertosRonda1();
        else if (estado == GonogoFSM.SECONDROUND)
            dataSave.set_aciertosRonda2();

    }
    public void set_elemRunCount()  //Elements crossed
    {
        elemRun++;

        movementAnimalsUI(movi, distanceToRun);
    }
    #endregion Setters


}
