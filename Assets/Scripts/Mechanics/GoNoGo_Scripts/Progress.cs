using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress : MonoBehaviour
{
    //Una función de avance para cada uno de los animalitos según el largo total del objeto de la barra (quien lleva este script)

    float distanceBar;
    float distanceToRun;
    [SerializeField] Transform raccon; //movi
    [SerializeField] Transform fox; //one

    [SerializeField] RectTransform bar;

    [SerializeField] GoNoGo gng;

    [SerializeField] GoNoGo_SetData data;

    void Start()
    {
        bar = GetComponent<RectTransform>();

        distanceBar = bar.sizeDelta.x;

        distanceToRun = distanceBar / data.getDificultad(gng.nivelActual).NElemTotal;

        raccon.position = new Vector3((-distanceBar / 2), raccon.position.y, fox.position.z);
        //Fox position is the beggining of the bar + max errors
        fox.position = new Vector3( (-distanceBar/2) + (distanceBar / data.getDificultad(gng.nivelActual).NErroresMaxXRonda), fox.position.y, fox.position.z);
    }

    void Movi()
    {
        raccon.position += new Vector3(distanceToRun, 0, 0);
    }

    void One()
    {
        fox.position += new Vector3(distanceToRun, 0, 0);
    }
}
