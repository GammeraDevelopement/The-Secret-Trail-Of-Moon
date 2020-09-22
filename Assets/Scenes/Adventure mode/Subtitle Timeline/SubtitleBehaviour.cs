using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleBehaviour : PlayableBehaviour
{
    public string subtitleText;
/*
    public int capitulo;
    public int escena;
    public int id = 0;
    public MessageJSONManager messageJSONManager;*/

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TextMeshProUGUI text = playerData as TextMeshProUGUI;
        //subtitleText = messageJSONManager.GetMensajes(capitulo, escena, id).Cadena;
        text.text = subtitleText;
        text.color = new Color(1, 1, 1, info.weight);  // fading
    }
}
