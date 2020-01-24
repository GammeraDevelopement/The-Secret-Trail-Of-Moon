using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enigma : MonoBehaviour
{

    private static Enigma Instance;

    [Header("Variables del psicólogo")]
    public int nivel;
    public int numeroRuedas;
    public int numeroElementos;
    public float tiempo;
    public bool senalizacionError;
    public int penalizacion;
    public int mostrarTiempo;

    [Header("Variables del editor")]
    public bool iniciarConPSICOHUD = false;
    public GameObject[] rueda;
    public GameObject[] notchs;
    public GameObject[] objetivos;
    public Image[] ajedrez;
    public Image[] codigoPared;
    public GameObject puerta;
    public Text tiempoText;
    
    private Quaternion rotacionTarget;
    private bool flagQuaternion = false;
    private Sprite[] arrayCodigo;              //Array con el código a descifrar
    private Sprite[] imagenesCodigo;  //Imágenes del código de la pared
    private string codigoActual = "";
    private Animator leverAnim;
    private bool cd = false;
    private float rotacion1;          //Imágenes de la rueda
    private float timeLeft = 0;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        Sprite[] imagenesAux = new Sprite[codigoPared.Length];
        for (int i = 0; i < codigoPared.Length; i++) {
            imagenesAux[i] = codigoPared[i].gameObject.GetComponentInChildren<Image>().sprite;
        }
        imagenesAux = shuffleList<Sprite>(new List<Sprite>(imagenesAux)).ToArray();
        for (int i = 0; i < codigoPared.Length; i++) {
            codigoPared[i].sprite = imagenesAux[i];
        }
        if (!iniciarConPSICOHUD) Begin();

    }

    //Se inicia cuando se le de al botón de iniciar en el PSICOHUD
    public void Begin() {
        rotacionTarget = rueda[0].transform.GetChild(0).transform.rotation;
        leverAnim = GetComponent<Animator>();
        setDifficulties(nivel);
        imagenesCodigo = new Sprite[numeroRuedas];
        timeLeft = tiempo;
        for (int i = 0; i < numeroRuedas; i++) {
            rueda[i].SetActive(true);
            objetivos[i].SetActive(true);

            //Set image targets
            for (int j = 0; j < numeroElementos; j++) {
                GameObject wheel = rueda[i].transform.GetChild(0).gameObject;
                GameObject canvas = wheel.transform.GetChild(0).gameObject;
                GameObject childTarget = canvas.transform.GetChild(j).gameObject;
                childTarget.GetComponent<Image>().sprite = codigoPared[j].sprite;
            }

            //Set code sprites
            for (int j = 0; j < numeroRuedas; j++) {
                int random = Random.Range(0, numeroRuedas - 1);
                imagenesCodigo[j] = codigoPared[random].sprite;
                GameObject canvas = objetivos[j].transform.GetChild(0).gameObject;
                GameObject child = canvas.transform.GetChild(0).gameObject;
                child.GetComponent<Image>().sprite = ajedrez[random].sprite;
            }

        }

    }

    // Update is called once per frame
    void Update() {
        if (timeLeft >= 0) {
            timeLeft = timeLeft + Time.deltaTime;
            tiempoText.text = "" + Mathf.Round(timeLeft) + "";
        } else {
            Debug.Log("<color=green>[DifficultyShow.cs.changeValuesByUI()]</color> Time finished.");
        }

    }

    public void setDifficulties(int nivel) {
        DificultadEnigma dif = GetComponent<DificultadEnigma>();
        this.nivel = nivel;
        numeroRuedas = dif.getDificultad(nivel).NRuedas;
        numeroElementos = dif.getDificultad(nivel).NElementos;
        tiempo = dif.getDificultad(nivel).Tiempo;
        //senalizacionError = dif.getDificultad(nivel).Senalizacion;
        //penalizacion = dif.getDificultad(nivel).Penalizacion;
        //mostrarTiempo = dif.getDificultad(nivel).MostrarTiempo;
    }

    public void loadDifficulty() {
        setDifficulties(nivel);
    }

    private bool checkCode() {
        arrayCodigo = new Sprite[numeroRuedas];
        for (int i = 0; i < numeroRuedas; i++) {
            arrayCodigo[i] = notchs[i].GetComponent<NotchEnigma>().getTarget().GetComponent<Image>().sprite;
        }

        for (int i = 0; i < numeroRuedas; i++) {
            if (arrayCodigo[i].Equals(imagenesCodigo[i])) {

            } else {
                return false;
            }
        }
        return true;
    }

    public void pullLever() {
        leverAnim.SetBool("Pulled", true);
        if (!cd) {
            StartCoroutine(cooldown(1F));
            cd = true;
        }
        
    }

    //Vuelve a la posición inicial
    private void penalizacionCero() {
        volverPosOriginal();
    }

    //Vuelve a la posición inicial y se cambia el orden de los símbolos de la rueda
    private void penalizacionUno() {
        volverPosOriginal();
        imagenesCodigo = shuffleList<Sprite>(new List<Sprite>(imagenesCodigo)).ToArray();
    }

    //Vuelve a la posicion inicial y se cambia el orden de la rueda y del código de la pared
    private void penalizacionDos() {
        volverPosOriginal();
        imagenesCodigo = shuffleList<Sprite>(new List<Sprite>(imagenesCodigo)).ToArray();
    }

    private void volverPosOriginal() {
        for (int i = 0; i < rueda.Length; i++) {
            rueda[i].transform.GetChild(0).transform.rotation = rotacionTarget;
        }
    }

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

    private IEnumerator cooldown(float tiempo) {
        yield return new WaitForSeconds(tiempo);
        if (!checkCode()) {
            leverAnim.SetBool("Pulled", false);

            if(penalizacion == 1) {
                penalizacionCero();
            } else if (penalizacion == 2) {
                penalizacionUno();
            } else {
                penalizacionDos();
            }

            Debug.Log("Wrong leveeeeeer!");
        } else {
            puerta.GetComponent<Animator>().Play("open");
            Debug.Log("You win.");
        }
        cd = false;

    }
}
