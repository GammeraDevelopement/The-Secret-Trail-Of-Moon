using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PantallaInformes : MonoBehaviour
{

    public GameObject outputEnigma;
    public GameObject outputCSAT;
    public GameObject outputKuburi;
    public GameObject outputTekaTeki;

    private string nj = "No Jugado";

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("AciertosCSAT")) {

            outputCSAT.transform.GetComponent<TMP_Text>().text = "Aciertos: " + PlayerPrefs.GetInt("AciertosCSAT") + " \nErrores Comision: " + PlayerPrefs.GetInt("ErroresComisionCSAT") + " \nErrores Omisión: " + PlayerPrefs.GetInt("ErroresOmisionCSAT");
            //imagenGanador.transform.GetChild(2).GetComponent<TMP_Text>().text
        }
        else
        {
            outputCSAT.transform.GetComponent<TMP_Text>().text = nj;
        }
        if (PlayerPrefs.HasKey("AciertosEnigma"))
        {

            outputEnigma.transform.GetComponent<TMP_Text>().text = "Aciertos: " + PlayerPrefs.GetInt("AciertosEnigma") + " \nErrores: " + PlayerPrefs.GetInt("ErroresEnigma");
            //imagenGanador.transform.GetChild(2).GetComponent<TMP_Text>().text
        }
        else
        {
            outputEnigma.transform.GetComponent<TMP_Text>().text = nj;
        }
        if (PlayerPrefs.HasKey("MovimientosTekaTeki"))
        {

            outputTekaTeki.transform.GetComponent<TMP_Text>().text = "Movimientos: " + PlayerPrefs.GetInt("MovimientosTekaTeki");
            //imagenGanador.transform.GetChild(2).GetComponent<TMP_Text>().text
        }
        else
        {
            outputTekaTeki.transform.GetComponent<TMP_Text>().text = nj;
        }
        if (PlayerPrefs.HasKey("MinutosKuburi"))
        {

            outputKuburi.transform.GetComponent<TMP_Text>().text = "Tiempo: " + PlayerPrefs.GetInt("MinutosKuburi") + " min " + PlayerPrefs.GetInt("SegundosKuburi") + "seg";
            //imagenGanador.transform.GetChild(2).GetComponent<TMP_Text>().text
        }
        else
        {
            outputKuburi.transform.GetComponent<TMP_Text>().text = nj;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
