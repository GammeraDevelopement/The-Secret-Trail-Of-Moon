using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public int capitulo;
    public int escena;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip clip;

    [Header("GUI")]
    public Canvas canvas;
    public TMP_Text text;

    private MessageJSONManager messageJSONManager;
    private float duracion;
    private bool wait = false;

    // Start is called before the first frame update
    void Start()
    {
        messageJSONManager = gameObject.GetComponent<MessageJSONManager>();
        text.text = "";
        text.text = messageJSONManager.GetMensajes(capitulo, escena, 1).Cadena;
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait) {
            StartCoroutine(waiting());
            wait = true;
        }

    }

    private IEnumerator waiting() {
        yield return new WaitForSeconds(3);
    }
}
