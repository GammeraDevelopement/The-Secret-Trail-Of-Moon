using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageJSONManager : MonoBehaviour
{
    public TextAsset json;

    private string encodedString;
    private Root root;

    [Serializable]
    public class Capitulos {
        public Capitulo[] Capitulo;
    }

    [Serializable]
    public class Capitulo{
        public Escena[] Escena;
        public int id;
    }

    [Serializable]
    public class Escena {
        public Mensajes[] Mensajes;
        public int id;
    }

    [Serializable]
    public class Mensajes {
        public int id;
        public string Cadena;
        public string Audio;
        public float x;
        public float y;
        public float Duracion;
    }

    [Serializable]
    public class Root {
        public Capitulos[] Capitulos;
    }

    void Awake(){
        encodedString = json.text;
        parseJson(encodedString);
    }

    public void parseJson(string encodedString) {
        root = JsonUtility.FromJson<Root>(encodedString);
    }

    public Mensajes GetMensajes(int id_capitulo, int id_escena, int id_mensaje) {
        return root.Capitulos[0].Capitulo[id_capitulo-1].Escena[id_escena-1].Mensajes[id_mensaje-1];

    }
}