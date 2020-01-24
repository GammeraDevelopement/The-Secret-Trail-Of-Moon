using UnityEngine;
using System.Collections;

public class TurnColor : MonoBehaviour {

    void Action() {
        TurnColors();
    }

    void TurnColors() {
        Color col = new Color(Random.value, Random.value, Random.value);
        gameObject.GetComponent<Renderer>().material.color = col;
        
    }
}
