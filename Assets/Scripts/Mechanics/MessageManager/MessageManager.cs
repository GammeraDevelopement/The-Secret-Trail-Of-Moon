using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(MessageJSONManager))]
public class MessageManager : MonoBehaviour
{
    public int capitulo;
    public int escena;
    public int id = 0;
    public float duracionTotal = 0;
    public float velocidad = 2;


    [Header("Audio")]
    public AudioSource source;
    private AudioClip clip;

    [Header("GUI")]
    public Canvas canvas;
    public TMP_Text text;

    private MessageJSONManager messageJSONManager;
    private float duracion;
    private bool wait = false;
    private ArrayList lista = new ArrayList();


    // Start is called before the first frame update
    void Start()
    {

        messageJSONManager = gameObject.GetComponent<MessageJSONManager>();
        text.text = "";

    }

    
    private IEnumerator waiting(float duracion, float silencio) {
        clip = Resources.Load(messageJSONManager.GetMensajes(capitulo, escena, id).Audio) as AudioClip;
        source.clip = clip;
        source.Play();
        text.text = messageJSONManager.GetMensajes(capitulo, escena, id).Cadena;
        yield return new WaitForSeconds(duracion);
        text.text = "";
        yield return new WaitForSeconds(silencio);
        id++;
        
        if (id <= messageJSONManager.getLength(capitulo, escena)-1) {
            wait = false;
        } 
        else {
            wait = true;
        }
            yield return null;
    }

    public void message() {
        StartCoroutine(waiting(messageJSONManager.GetMensajes(capitulo, escena, id).Duracion * velocidad,
                messageJSONManager.GetMensajes(capitulo, escena, id).Silencio * velocidad));
    }
}
