using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelJSONManager : MonoBehaviour
{
    public TextAsset json;

    private string encodedString;
    private ArrayList list = new ArrayList();
    private Root root;

    [Serializable]
    public class ExpMax
    {
        public int Experiencia;
    }

    [Serializable]
    public class ExpCSAT
    {
        public int Experiencia;
    }

    [Serializable]
    public class ExpEnigma
    {
        public int Experiencia;
    }

    [Serializable]
    public class ExpCubos
    {
        public int Experiencia;
    }

    [Serializable]
    public class ExpTekaTeki
    {
        public int Experiencia;
    }

    [Serializable]
    public class ExpGoNoGo
    {
        public int Experiencia;
    }

    [Serializable]
    public class Root
    {
        public ExpMax[] ExpMax;
        public ExpCSAT[] ExpCSAT;
        public ExpEnigma[] ExpEnigma;
        public ExpCubos[] ExpCubos;
        public ExpTekaTeki[] ExpTekaTeki;
        public ExpGoNoGo[] ExpGoNoGo;
    }

    private void Awake()
    {
        encodedString = json.text;
        parseJson(encodedString);
    }

    public void parseJson(string encodedString)
    {
        root = JsonUtility.FromJson<Root>(encodedString);
    }

    public int getExperienciaMax(int nivel)
    {
        return root.ExpMax[nivel - 1].Experiencia;
    }

    public int getExperienciaCSAT(int nivel)
    {
        return root.ExpCSAT[nivel - 1].Experiencia;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
