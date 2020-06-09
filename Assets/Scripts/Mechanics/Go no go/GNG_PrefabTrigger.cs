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
    private const string ANIM_WEB = "SpiderJumping";
    private const string ANIM_ROCK = "Esquivar";
    private const string ANIM_TREE = "Agacharse";
    private const string ANIM_BUSH = "Apartar";
    private const string ANIM_STUMP = "Saltar";

    public enum InputGNG {
        WEB, ROCK, TREE, BUSH, STUMP, NO_INPUT
    }

    [HorizontalLine(color: EColor.Blue)]
    public InputGNG tipoInput = InputGNG.WEB;
    public bool instruccionInversa = false;

    private GNG_GameController gng;
    private Animator player;
    private Animator thisObjectAnim;
    private bool noInput = true;

    private bool counted = false;
    
    void Start() {
        gng = GameObject.FindGameObjectWithTag("GameController").GetComponent<GNG_GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        thisObjectAnim = GetComponent<Animator>();

        //Resize the box trigger regarding the size calculated on gng_gameController
        /*
        box.size = new Vector3(box.size.x, box.size.y, gng.sizeZ);
        box.center = new Vector3(box.center.x, box.center.y, -gng.sizeZ / 2);
        */
    }
   
    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            if (!counted) {
                gng.targetsPassed++;
                counted = true;
            }
            switch (tipoInput) {
                case InputGNG.WEB:
                    if (Input.GetButton(CROSS) || Input.GetKey(KeyCode.A)) {
                        Debug.Log("Botón Correcto");
                        Acierto();
                    } else if (Input.GetButton(SQUARE) || Input.GetButton(CIRCLE) || Input.GetButton(TRIANGLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto, es Cruz/A");
                        Error(false);
                    }
                    break;
                case InputGNG.ROCK:
                    if (Input.GetButton(SQUARE) || Input.GetKey(KeyCode.S)) {
                        Debug.Log("Botón Correcto");
                        Acierto();
                    } else if (Input.GetButton(CROSS) || Input.GetButton(CIRCLE) || Input.GetButton(TRIANGLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto, es Cuadrado/S");
                        Error(false);
                    }
                    break;
                case InputGNG.TREE:
                    if (Input.GetButton(CIRCLE) || Input.GetKey(KeyCode.D)) {
                        Debug.Log("Botón Correcto");
                        Acierto();
                    } else if (Input.GetButton(CROSS)  || Input.GetButton(SQUARE) || Input.GetButton(TRIANGLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto,es Círculo/D");
                        Error(false);
                    }
                    break;
                case InputGNG.BUSH:
                    if (Input.GetButton(TRIANGLE) || Input.GetKey(KeyCode.F)) {
                        Debug.Log("Botón Correcto");
                        Acierto();
                    } else if (Input.GetButton(CROSS)  || Input.GetButton(SQUARE) || Input.GetButton(CIRCLE) || Input.GetButton(R1)) {
                        Debug.Log("Botón Incorrecto, es Triángulo/F");
                        Error(false);
                    }
                    break;
                case InputGNG.STUMP:
                    if (Input.GetButton(R1) || Input.GetKey(KeyCode.G)) {
                        Debug.Log("Botón Correcto");
                        Acierto();
                    } else if (Input.GetButton(CROSS) || Input.GetButton(SQUARE) || Input.GetButton(CIRCLE) || Input.GetButton(TRIANGLE)) {
                        Debug.Log("Botón Incorrecto, es R1/G");
                        Error(false);
                    }
                    break;
                case InputGNG.NO_INPUT:
                    if (!Input.anyKey) {


                    } else {
                        Debug.Log("Botón Incorrecto");
                        Error(false);
                    }
                    break;
                default:
                    break;
            }
            
        }
    }

    private void Acierto() {
        gng.set_plus_aciertoCount();
        SpecificAnimation(tipoInput);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        noInput = false;
    }

    private void Error(bool omision) {
        gng.set_plus_errorCount(omision);
        gameObject.GetComponent<BoxCollider>().enabled = false;
        noInput = false;
    }

    private void SpecificAnimation(InputGNG input) {
        switch (input) {
            case InputGNG.WEB:
                thisObjectAnim.SetBool(ANIM_WEB, true);
                Debug.Log("Mata a la arañita");
                break;
            case InputGNG.ROCK:
                player.SetTrigger(ANIM_ROCK);
                Debug.Log("Esquiva la roca");
                break;
            case InputGNG.TREE:
                player.SetTrigger(ANIM_TREE);
                Debug.Log("Se agacha en el árbol");
                break;
            case InputGNG.BUSH:
                thisObjectAnim.SetBool(ANIM_BUSH, true);
                Debug.Log("Aparta la maleza");
                break;
            case InputGNG.STUMP:
                player.SetTrigger(ANIM_STUMP);
                Debug.Log("Salta el árbol");
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player") {
            if (!instruccionInversa) {
                if (noInput) {
                    Debug.Log("Error de omisión");
                    Error(true);
                }
            } else {
                if (noInput) {
                    Acierto();
                } else {

                }
            }
        }
    }

}
