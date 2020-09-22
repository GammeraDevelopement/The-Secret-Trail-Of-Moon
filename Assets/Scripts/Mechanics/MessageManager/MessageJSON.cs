using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class MessageJSON : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        encodedString = json.text;
        parseJson(encodedString);

        return Playable.Create(graph);
    }

    public TextAsset json;

    private string encodedString;
    private ArrayList list = new ArrayList();
    private Root root;

    [Serializable]
    public class Capitulos
    {
        public Capitulo[] Capitulo;
    }

    [Serializable]
    public class Capitulo
    {
        public Escena[] Escena;
        public int id;
    }

    [Serializable]
    public class Escena
    {
        public Mensajes[] Mensajes;
        public int id;
    }

    [Serializable]
    public class Mensajes
    {
        public int id;
        public string Cadena;
        public string Audio;
        public float x;
        public float y;
        public float Duracion;
        public float Silencio;
    }

    [Serializable]
    public class Root
    {
        public Capitulos[] Capitulos;
    }

    public void parseJson(string encodedString)
    {
        root = JsonUtility.FromJson<Root>(encodedString);
    }

    public ArrayList getList(int id_capitulo, int id_escena)
    {
        int length = getLength(id_capitulo, id_escena);
        for (int i = 0; i < length; i++)
        {
            list.Add(GetMensajes(id_capitulo, id_escena, i));
        }
        return list;
    }

    public Mensajes GetMensajes(int id_capitulo, int id_escena, int id_mensaje)
    {
        return root.Capitulos[0].Capitulo[id_capitulo].Escena[id_escena].Mensajes[id_mensaje];
    }

    public int getLength(int capitulo, int escena)
    {
        return root.Capitulos[0].Capitulo[capitulo].Escena[escena].Mensajes.Length;
    }
}
