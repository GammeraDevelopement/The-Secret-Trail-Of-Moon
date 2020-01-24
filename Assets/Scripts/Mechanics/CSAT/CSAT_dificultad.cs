using UnityEngine;
using System.Collections.Generic;
using System;

public class CSAT_dificultad : MonoBehaviour
{

    public TextAsset json;
    private string encodedString;
    private Root root;

    [Serializable]
    public class Dificultades
    {
        public int Nivel;
        public int Carga;
        public int Vida;
        public int ElmSecuencia;
        public int TotalElm;
        public int Distractores;
        public int Pena;
        public bool Instruccion;
        public int Velocidad;
        public float Frecuencia;
        public int Duracion;
    }

    [Serializable]
    public class Root
    {
        public Dificultades[] dificultades;
    }

    // Start is called before the first frame update
    void Awake()
    {
        encodedString = json.text;
        parseJson(encodedString);
    }

    public void parseJson(string encodedString)
    {
        root = JsonUtility.FromJson<Root>(encodedString);
        Debug.Log(root.dificultades[0]);
    }

    public Dificultades getDificultad(int nivel)
    {
        return root.dificultades[nivel - 1];
    }

}
