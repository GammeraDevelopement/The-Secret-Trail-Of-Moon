using System;
using System.Collections.Generic;
using UnityEngine;

public class GoNoGo_SetData : MonoBehaviour
{
    public TextAsset json;
    private string encodedString;
    private Root root;

    [Serializable]
    public class Dificultades
    {
        public int Nivel;
        public int NRondasTotales;
        public int NElementos;
        public int NElemXRonda;
        public int NElemTotal;
        public float FrecuenciaAparRonda1;
        public float FrecuenciaAparRonda2;
        public int NErroresMaxXRonda;
        public int NComodines;
        public int NAciertosSeguidosComodin;
        public float TiempoReaccionMax;
        public float TiempoAparicionEstimulos;

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
