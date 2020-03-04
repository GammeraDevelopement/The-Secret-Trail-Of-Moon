using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public int capitulo;
    public int escena;
    public int id = 0;


    [Header("Audio")]
    public AudioSource source;
    public AudioClip clip;

    [Header("GUI")]
    public Canvas canvas;
    public TMP_Text text;

    private MessageJSONManager messageJSONManager;
    private float duracion;
    private bool wait = false;
    private ArrayList lista = new ArrayList();
    //private const int MAXID = 2;


    // Start is called before the first frame update
    void Start()
    {


        messageJSONManager = gameObject.GetComponent<MessageJSONManager>();
        text.text = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!wait) {
            StartCoroutine(waiting(messageJSONManager.GetMensajes(0,0,id).Duracion, messageJSONManager.GetMensajes(0, 0, id).Silencio));
            wait = true;
        }

    }
    
    private IEnumerator waiting(float duracion, float silencio) {
        text.text = messageJSONManager.GetMensajes(capitulo, escena, id).Cadena;
        yield return new WaitForSeconds(duracion);
        text.text = "";
        yield return new WaitForSeconds(silencio);
        id++;
        //if (messageJSONManager.GetMensajes(1, 1, id).id <= MAXID) {
        
        if (id <= messageJSONManager.getLength(capitulo, escena)-1) {
            wait = false;
        } 
        else {
            wait = true;
        }
            yield return null;
    }
}
