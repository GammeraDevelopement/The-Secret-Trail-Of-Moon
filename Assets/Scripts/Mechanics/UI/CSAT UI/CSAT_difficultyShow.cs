using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CSAT_difficultyShow : MonoBehaviour
{
    public TMP_InputField nivel;
    public Slider sliderCarga;
    public Slider sliderVida;
    public TMP_InputField penalizacion;
    public Slider sliderFrecuencia;
    public Slider sliderTiempoEntre;
    public TMP_InputField duracion;

    private CSAT csat;
    private int initialLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        csat = GameObject.Find("GameController").GetComponent<CSAT>();
    }

    // Update is called once per frame
    public void changeValuesByUI()
    {
        csat.nivelDeCarga = (int)sliderCarga.value;
        csat.vidaRoca = (int)sliderVida.value;
        csat.penalizacionDelError = int.Parse(penalizacion.text);
        csat.duracionPruebaSegundos = int.Parse(duracion.text);
        csat.tiempoEntreIconos = sliderTiempoEntre.value;
        csat.frecuenciaAparicionTarget = sliderFrecuencia.value;
    }

    public void updateValues()
    {
        Debug.Log("<color=aqua>[DifficultyShow.cs.changeValuesByUI()]</color> Los valores de dificultad de la prueba han cambiado. \n NivelCarga: "+ csat.nivelDeCarga+"\n VidaRoca:"+ csat.vidaRoca
            +"\n Penalizacion:"+ csat.penalizacionDelError);
        sliderCarga.value = csat.nivelDeCarga;
        sliderVida.value = csat.vidaRoca;
        penalizacion.text = csat.penalizacionDelError + "";
        duracion.text = csat.duracionPruebaSegundos + "";
        sliderTiempoEntre.value = csat.tiempoEntreIconos;
        sliderFrecuencia.value = csat.frecuenciaAparicionTarget;
    }

    public void addValue(int valor)
    {
        Debug.Log("valor actual:" + initialLevel + " value change" + valor);
        csat.setDifficulties(initialLevel);
        initialLevel = int.Parse(nivel.text);
        initialLevel += valor;
        if (initialLevel < 1) initialLevel = 1;
        nivel.text = "" + initialLevel;
        updateValues();
    }
}
