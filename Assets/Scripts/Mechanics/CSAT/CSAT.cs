using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using NaughtyAttributes;

/*
 * La mecánica consiste en que cuando coincida el patrón de imágenes propuesto, en la segunda imagen se deba pulsar el botón de disparo.
 * Ésto hará que Moon lance una piedra para romper la roca.
 * Cuando se hallan lanzado x piedras, la roca se partirá, superando la mecánica.
 * El patrón se basa en que aparecerán imágenes aleatorias en la pantalla, una cada vez y durante unos segundos. Si aparece la primeraImagenPatron
 * y acto seguido la segundaImagenPatron se debe pulsar el botón de disparo para lanzar la piedra.
 */
public class CSAT : MonoBehaviour {

    //CONSTANTES
    private const float MAX_FREQ = 0.99F;
    private const string FIRE_BUTTON = "Fire1";
    private const string INSTRUCTON_BUTTON = "Square";

    public enum CSATFSM {
        LOADING,
        INSTRUCTION,
        PLAYING,
        FINISHED
    }
    public CSATFSM estado = CSATFSM.LOADING;

    //VARIABLES A MEDIR
    private float tiempoDeRespuesta;
    private int aciertos;
    private int erroresOmision;
    private int erroresComision;

    [Header("Variables modificables por psicólogo")]
    //VARIABLES MODIFICABLES POR PSICÓLOGO
    public int nivel = 1;
    public int nivelDeCarga = 3;
    public int vidaRoca = 3;

    [Tooltip("El target será siempre la imagen 1 seguida por la imagen 2")]
    public int cantidadImagenesAMostrar = 5;
    public int distractores = 0;
    public Sprite[] imagenesAMostrar;
    public int penalizacionDelError = 0;
    public bool instruccion;
    public float tiempoEntreIconos = 3F;
    public float frecuenciaAparicionTarget = 0.7F;
    public int duracionPruebaSegundos = 20;
    public float cooldownAnimacion = 0.5F;
    public bool tresTarget = false;

    //VARIABLES DEL EDITOR
    [BoxGroup("GUI")] public Canvas psicohud;
    [BoxGroup("GUI")] public GameObject instruction;
    [BoxGroup("GUI")] public TMP_Text textoGanador;
    [BoxGroup("GUI")] public Image imagenACambiar;
    [BoxGroup("GUI")] public Image imagenGanador;
    [BoxGroup("GUI")] public Image barraFuerza;
    [BoxGroup("GUI")] public Image barraRoca;
    [BoxGroup("GUI")] public Text textoRoca;
    [BoxGroup("GUI")] public Text textFuerza;
    [BoxGroup("GUI")] public Text tiempo;

    [BoxGroup("Elementos del juego")] public GameObject player;
    [BoxGroup("Elementos del juego")] public GameObject piedra;
    [BoxGroup("Elementos del juego")] public GameObject moon;
    [BoxGroup("Elementos del juego")] public GameObject lanzadorPiedra;
    [BoxGroup("Elementos del juego")] public GameObject roca;
    [BoxGroup("Elementos del juego")] public GameObject roturaRoca;

    [BoxGroup("Distractores")] public GameObject distractorGenerator1;
    [BoxGroup("Distractores")] public GameObject distractor1;
    [BoxGroup("Distractores")] public GameObject distractorGenerator2;
    [BoxGroup("Distractores")] public GameObject distractor2;

    [BoxGroup("Audio/Video")] private AudioSource source;
    [BoxGroup("Audio/Video")] public AudioClip rightSound;
    [BoxGroup("Audio/Video")] public AudioClip wrongClip;
    [BoxGroup("Audio/Video")] public VideoClip secondClip;
    [BoxGroup("Audio/Video")] public VideoClip firstClip;
    [BoxGroup("Audio/Video")] public GameObject squareButton;
    [BoxGroup("Audio/Video")] public GameObject videoPlayer;

    public Image black;
    public int introCount;
    public bool test;

    //VARIABLES INTERNAS
    private int primeraImagenPatron = 1;
    private int segundaImagenPatron = 2;
    private int piedrasLanzadas = 0;
    private int imagenAnterior = 0;
    private int imagenActual = 0;
    private int cargaActual = 0;
    private float timeLeft = 0;
    private bool flagConteo = true;
    private bool comienza = false;
    private bool seHaDisparado = false;
    private bool cdPiedra = false;
    //private bool distractorOn = false;
    private bool loadingScene = false;
    private int cantidadImagenesGeneradas = 0;
    public bool tutorial;



    // Use this for initialization
    void Start() {
        black.CrossFadeAlpha(0, 0.5F, true);
        source = player.GetComponent<AudioSource>();
        imagenGanador.canvasRenderer.SetAlpha(0.0F);
        textoGanador.CrossFadeAlpha(0, 0, true);
        timeLeft = (float)duracionPruebaSegundos;
        Debug.Log("<color=aqua>[CSAT.cs]</color> Inicia la prueba [comienza :" + comienza + "]");
        setDifficulties(PlayerPrefs.GetInt("nivelCSAT"));
    }

    //Obtiene la dificultad de un json asociado
    public void setDifficulties(int nivel) {
        CSAT_dificultad dif = GetComponent<CSAT_dificultad>();
        this.nivel = nivel;
        nivelDeCarga = dif.getDificultad(nivel).Carga;
        vidaRoca = dif.getDificultad(nivel).Vida;
        cantidadImagenesAMostrar = dif.getDificultad(nivel).TotalElm;
        instruccion = dif.getDificultad(nivel).Instruccion;
        penalizacionDelError = dif.getDificultad(nivel).Pena;
        distractores = dif.getDificultad(nivel).Distractores;
    }

    public void loadDifficulty() {
        setDifficulties(nivel);
    }

    public void finishInstruction() {
        estado = CSATFSM.PLAYING;
    }

    void CheckOver(VideoPlayer vp) {
        introCount++;
        //vp.Stop();
    }

    private void StartPlaying() {
        estado = CSATFSM.PLAYING;
    }

    // Update is called once per frame
    void Update() {
        
        //UI managing
        barraFuerza.fillAmount = (float)cargaActual / nivelDeCarga;
        textFuerza.text = cargaActual + "/" + nivelDeCarga;
        barraRoca.fillAmount = 1 - ((float)piedrasLanzadas / (float)vidaRoca);
        textoRoca.text = vidaRoca - piedrasLanzadas + "";

        switch (estado) {
            case CSATFSM.LOADING:
                if(nivel == 29 ) {
                    instruction.GetComponent<InstructionScreen>().setInstructionVideo(secondClip);
                    instruction.GetComponent<InstructionScreen>().startInstruction();
                } else if(nivel == 1){
                    instruction.GetComponent<InstructionScreen>().setInstructionVideo(firstClip);
                    instruction.GetComponent<InstructionScreen>().startInstruction();
                } else {
                    instruction.GetComponent<InstructionScreen>().startNoTutorial();
                }
                estado = CSATFSM.INSTRUCTION;
                break;
            case CSATFSM.INSTRUCTION:
                break;
            case CSATFSM.PLAYING:

                if (flagConteo) {
                    StartCoroutine(contando());
                    StartCoroutine(activarDistractores());
                    flagConteo = false;
                }

                //Tiempo de la prueba
                if (timeLeft > 0) {
                    timeLeft = timeLeft - Time.deltaTime;
                    tiempo.text = "" + Mathf.Round(timeLeft) + "";
                } else {
                    comienza = false;
                    Debug.Log("<color=aqua>[CSAT.cs.tiempoDeLaPrueba()]</color> Acaba la prueba [comienza :" + comienza + "]");
                }

                //Detección de destrucción de piedra
                if (cargaActual >= nivelDeCarga) {
                    lanzarPiedra();
                    cargaActual = 0;
                }

                //Detectar el disparo
                if (comienza && piedrasLanzadas < vidaRoca) {
                    if (imagenAnterior == primeraImagenPatron && imagenActual == segundaImagenPatron && !seHaDisparado) {
                        if (Input.GetButtonDown(FIRE_BUTTON)) {
                            // ACIERTO
                            seHaDisparado = true;
                            aciertos++;
                            Debug.Log("<color=aqua>[CSAT.cs]</color> Se ha acertado");
                            source.clip = rightSound;
                            source.Play();
                            cargaActual++;
                        }
                    } else if ((imagenAnterior != primeraImagenPatron || imagenActual != segundaImagenPatron || seHaDisparado) && Input.GetButtonDown("Fire1")) {
                        erroresComision++;
                        source.clip = wrongClip;
                        source.Play();
                        if (penalizacionDelError > 0) cargaActual -= penalizacionDelError;
                        if (cargaActual < 0) cargaActual = 0;
                        Debug.Log("<color=aqua>[CSAT.cs]</color> Se ha producido un error por comisión");
                    }
                } else if (comienza && piedrasLanzadas >= vidaRoca) {
                    StartCoroutine(ganar());
                    estado = CSATFSM.FINISHED;
                    Debug.Log("<color=aqua>[CSAT.cs]</color> Winner");
                    
                } else {
                    Debug.Log("Se ha perdido.");
                    //sePierde()
                }
                break;
            case CSATFSM.FINISHED:
                //Debug.Log("Finished");
                int gamemode = PlayerPrefs.GetInt("Gamemode");
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

    }

    //Activación de distractores
    private IEnumerator activarDistractores() {
        if (distractores > 0) {
            bool distraer = true;
            while (distraer) {
                Debug.Log("<color=aqua>[CSAT.cs]</color> Generación de conejito.");
                Instantiate(distractor1, distractorGenerator1.transform.position, distractorGenerator1.transform.rotation);
                yield return new WaitForSeconds(Random.Range(10, 40));
                if (distractores > 1) {
                    Debug.Log("<color=aqua>[CSAT.cs]</color> Generación de pajaro.");
                    Instantiate(distractor2, distractorGenerator2.transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(10, 40));
                }
                if (!comienza) distraer = false;
            }
        }
    }

    //Empieza a cambiar las imágenes
    private IEnumerator contando() {
        comienza = true;
        while (comienza) {

            if (!cdPiedra) {
                imagenAnterior = imagenActual;

                float probabilidadTarget = Mathf.Lerp(frecuenciaAparicionTarget, MAX_FREQ, Time.time / duracionPruebaSegundos);
                float probabilidadNoTarget = Mathf.Lerp(1 - frecuenciaAparicionTarget, 1 - MAX_FREQ, Time.time / duracionPruebaSegundos);

                imagenActual = GetRandomValue(
                    new RandomSelection(1, 2, probabilidadTarget),
                    new RandomSelection(3, cantidadImagenesAMostrar, probabilidadNoTarget)
                );

                imagenACambiar.sprite = imagenesAMostrar[imagenActual];
                cantidadImagenesGeneradas++;
                seHaDisparado = false;
                yield return new WaitForSeconds(tiempoEntreIconos);

                //Error por omisión
                if (imagenAnterior == primeraImagenPatron && imagenActual == segundaImagenPatron && !seHaDisparado) {
                    Debug.Log("<color=aqua>[CSAT.cs]</color>Se ha producido un error por omisión");
                    erroresOmision++;
                    source.clip = wrongClip;
                    source.Play();
                    seHaDisparado = true;
                    if (penalizacionDelError > 0 && cargaActual > 0) cargaActual -= penalizacionDelError;
                }
                imagenACambiar.sprite = imagenesAMostrar[0];
                yield return new WaitForSeconds(0.5F);
            } else {
                imagenActual = 0;
                yield return new WaitForSeconds(cooldownAnimacion);
                cdPiedra = false;
            }
        }
        yield return null;
    }

    //Animación para poder romper la roca
    private void lanzarPiedra() {
        piedrasLanzadas++;
        float tama = (float)1 / (float)vidaRoca;
        Debug.Log(tama + " piedralanzada=" + piedrasLanzadas + " vidaroca=" + vidaRoca);
        if (piedrasLanzadas < vidaRoca) {
            roca.transform.localScale -= new Vector3(tama, tama, tama);
        }
        Instantiate(piedra, lanzadorPiedra.transform.position, lanzadorPiedra.transform.rotation);
        cdPiedra = true;
    }

    private IEnumerator ganar() {
        yield return new WaitForSeconds(1.0F);
        GameObject explosion = Instantiate<GameObject>(roturaRoca, Vector3.zero, roca.transform.rotation, roca.transform);
        roca.GetComponent<MeshFilter>().mesh = null;
        imagenGanador.CrossFadeAlpha(1, 0.5F, true);
        textoGanador.CrossFadeAlpha(1, 0.5F, true);
        imagenGanador.transform.GetChild(1).gameObject.SetActive(true);
        imagenGanador.transform.GetChild(2).GetComponent<TMP_Text>().text = "Aciertos: " + aciertos + " \nErrores Comision: " + erroresComision + " \nErrores Omisión: " + erroresOmision + " \nEstímulos:" + cantidadImagenesGeneradas;
        yield return new WaitForSeconds(3.0F);
        Destroy(roca);
        comienza = false;
    }

    public void iniciarCsat() {
        psicohud.gameObject.SetActive(false);
        estado = CSATFSM.INSTRUCTION;
    }

    //Estructura para generar el random
    struct RandomSelection {
        private int minValue;
        private int maxValue;
        public float probability;

        public RandomSelection(int minValue, int maxValue, float probability) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.probability = probability;
        }

        public int GetValue() { return Random.Range(minValue, maxValue + 1); }
    }


    private int GetRandomValue(params RandomSelection[] selections) {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections) {
            currentProb += selection.probability;
            if (rand <= currentProb)
                return selection.GetValue();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
    }
}
