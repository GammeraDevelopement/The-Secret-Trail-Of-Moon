using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NearWallsCamera : MonoBehaviour
{
    public TMP_Text text;
    public float hitDistance = 5.5f;
    public Transform posInicial;
    private Vector3 posSegunda;
    private Vector3 inicio;
    public float damp = 0.5f;

    private void Start()
    {
        posSegunda = new Vector3(posInicial.localPosition.x, posInicial.localPosition.y + damp, posInicial.localPosition.z);
        inicio = posInicial.localPosition;
        //  posInicial = text.gameObject.transform.localPosition;  //Coge la posicion inicial del texto
        Debug.Log(posInicial.localPosition);
    }

    void Update()
    {
        //text = GameObject.Find("").GetComponent<TMP_Text>();
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        int layerMask = 1 << 2;
        layerMask = ~layerMask;




        if (Physics.Raycast(ray, out hitInfo, hitDistance, layerMask))
        { //Si la vista esta cerca de una pared o suelo

            Debug.DrawLine(ray.origin, hitInfo.point, Color.white);
            text.gameObject.transform.localPosition = posSegunda;
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 5, Color.green);
            text.gameObject.transform.localPosition = inicio;
        }

    }
}
