using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SubtitleClip : PlayableAsset
{
    public string subtitleText;

    public int capitulo;
    public int escena;
    public int id = 0;
    public GameObject messageJSONManager;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        MessageJSONManager mm = messageJSONManager.GetComponent<MessageJSONManager>();

        var playable = ScriptPlayable<SubtitleBehaviour>.Create(graph);

        SubtitleBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleText = mm.GetMensajes(capitulo, escena, id).Cadena;
        subtitleBehaviour.subtitleText = subtitleText;    

        return playable;
    }

}
