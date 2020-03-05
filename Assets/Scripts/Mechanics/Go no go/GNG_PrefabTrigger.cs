using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;

/// <summary>
/// Esta clase comprueba que el jugador se encuentra en el rango frente al estímulo objetivo y a su vez detecta el input para
/// conocer si ha acertado o no.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class GNG_PrefabTrigger : MonoBehaviour {

    private const string SQUARE = "Square";
    private const string CROSS = "Cross";
    private const string CIRCLE = "Circle";
    private const string TRIANGLE = "Triangle";
    private const string R1 = "R1";

    public enum InputGNG {
        WEB, ROCK, TREE, BUSH, STUMP
    }

    [HorizontalLine(color: EColor.Blue)]
    public InputGNG tipoInput = InputGNG.WEB;
    public bool instruccionInversa = false;

    private GNG_GameController gng;
    private BoxCollider box;
    private bool noInput = true;
    
    void Start() {
        gng = GameObject.FindGameObjectWithTag("GameController").GetComponent<GNG_GameController>();

        box = gameObject.GetComponent<BoxCollider>();

        //Resize the box trigger regarding the size calculated on gng_gameController
        box.size = new Vector3(box.size.x, box.size.y, gng.sizeZ);
        box.center = new Vector3(box.center.x, box.center.y, -gng.sizeZ / 2);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            switch (tipoInput) {
                case InputGNG.WEB:
                    if (Input.GetButton(CROSS)) {
                        Debug.Log(Enum.GetName(typeof(InputGNG), tipoInput));
                        Debug.Log("Botón Correcto");
                        Debug.Log("Acierto");
                        box.gameObject.SetActive(false);
                        noInput = false;
                    } else if (Input.GetButton(SQUARE) || Input.GetButton(CIRCLE) || Input.GetButton(TRIANGLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto");
                        Debug.Log(Input.inputString);
                        noInput = false;
                    }
                    break;
                case InputGNG.ROCK:
                    if (Input.GetButton(SQUARE)) {
                        Debug.Log(Enum.GetName(typeof(InputGNG), tipoInput));
                        Debug.Log("Botón Correcto");
                        Debug.Log("Acierto");
                        box.gameObject.SetActive(false);
                        noInput = false;
                    } else if (Input.GetButton(CROSS) || Input.GetButton(CIRCLE) || Input.GetButton(TRIANGLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto");
                        Debug.Log(Input.inputString);
                        noInput = false;
                    }
                    break;
                case InputGNG.TREE:

                    break;
                case InputGNG.BUSH:
                    break;
                case InputGNG.STUMP:
                    break;
                default:
                    break;
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            if (!instruccionInversa) {
                if (noInput) {
                    Debug.Log("Error de omisión");
                } else {
                    //Posible error de comisión
                }
            } else {
                if (noInput) {
                    Debug.Log("Acierto");
                } else {

                }
            }
        }
    }

}
