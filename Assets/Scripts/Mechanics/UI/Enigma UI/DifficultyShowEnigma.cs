using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyShowEnigma : MonoBehaviour
{
    public GameObject gameController;

    public InputField nivel;
    public Slider numeroRuedas;
    public Slider numeroElementos;
    public InputField tiempo;
    public Toggle senalizacionError;
    public Slider penalizacion;
    public Slider mostrarTiempo;
    public Text demora;
    public Toggle toggle1;
    public Toggle toggle2;
    public Toggle toggle3;

    private Enigma enigma;
    private int initialLevel = 1;
    private int codeImage = 1;

    // Start is called before the first frame update
    void Start() {
        enigma = gameController.GetComponent<Enigma>();
        Time.timeScale = 0;
    }

    void Update() {
        if (mostrarTiempo.value == 0) {
            demora.text = "No";
        } else if (mostrarTiempo.value == 1) {
            demora.text = "Si";
        } else if (mostrarTiempo.value == 2) {
            demora.text = "Demora";
        }
    }

    // Update is called once per frame
    public void changeValuesByUI() {
        enigma.numeroRuedas = (int)numeroRuedas.value;
        enigma.numeroElementos = (int)numeroElementos.value;
        enigma.tiempo = float.Parse(tiempo.text);
        enigma.senalizacionError = senalizacionError.isOn;
        enigma.penalizacion = (int)penalizacion.value;
        enigma.mostrarTiempo = (int)mostrarTiempo.value;
        if (toggle1.isOn) {
            codeImage = 1;
        } else if (toggle2.isOn) {
            codeImage = 2;
        } else if (toggle3.isOn) {
            codeImage = 3;
        }
    }

    public void updateValues() {
        Debug.Log("<color=aqua>[DifficultyShow.cs.changeValuesByUI()]</color> Los valores de dificultad de la prueba han cambiado.");
        numeroRuedas.value = enigma.numeroRuedas;
        numeroElementos.value = enigma.numeroElementos;
        tiempo.text = enigma.tiempo + "";
        senalizacionError.isOn = enigma.senalizacionError;
        penalizacion.value = enigma.penalizacion;
        mostrarTiempo.value = enigma.mostrarTiempo;
    }

    public void addValue(int valor) {
        
        //initialLevel = int.Parse(nivel.text);
        initialLevel += valor;
        if (initialLevel < 1) {
            initialLevel = 1;
        } else if (initialLevel > 20) {
            initialLevel = 20;
        }
        enigma.setDifficulties(initialLevel);
        nivel.text = "" + initialLevel;
        updateValues();
    }

    public void Iniciar() {
        Time.timeScale = 1;
        gameController.GetComponent<Enigma>().Begin();
    }
}
