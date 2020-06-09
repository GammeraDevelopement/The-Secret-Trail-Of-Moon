using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigma_Dificultad : MonoBehaviour
{
    public TextAsset json;

    private string encodedString;
    private Root root;

    [Serializable]
    public class Dificultades {
        public int Nivel;
        public int NRuedas;
        public int NElementos;
        public float Tiempo;
        public float TiempoDisponible;
        public int Comodines;
    }

    [Serializable]
    public class Root {
        public Dificultades[] dificultades;
    }

    // Start is called before the first frame update
    void Awake() {
        encodedString = json.text;
        parseJson(encodedString);
    }

    public void parseJson(string encodedString) {
        root = JsonUtility.FromJson<Root>(encodedString);
    }

    public Dificultades getDificultad(int nivel) {
        return root.dificultades[nivel];
    }
}
