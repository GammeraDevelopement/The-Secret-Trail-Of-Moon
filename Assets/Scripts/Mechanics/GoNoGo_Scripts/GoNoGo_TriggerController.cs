using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is contained by FPSGoNoGo
public class GoNoGo_TriggerController : MonoBehaviour
{
    /// <summary>
    /// Está fuera del trigger. Lo usaremos para saber en qué momento está en rango y cuando no.
    /// </summary>
    [SerializeField] bool outT = false;

    /// <summary>
    /// Distancia a la cual se podría dar dentro del rango entre el jugador y el target.
    /// </summary>
    [SerializeField] float distanceGoodSkill = 25;

    /// <summary>
    /// Nuesstro componente transform.
    /// </summary>
    Transform tr;

    /// <summary>
    /// Referencia del script GoNoGo
    /// </summary>
    [SerializeField] GNG_GameController gng;
    [SerializeField] GonoGoData_Save dataSave;

    /// <summary>
    /// Pasamos en milisegundos el tiempo de omisión tras haberse pasado la web con la araña:
    /// </summary>
    float skipTime = 1000;

    bool controlJoystick = false;
    public bool controlElementCount = false;

    string inputWeb = "DPAD Y-Axis"; //LT

    string inputTronco = "Square"; //A. Abajo

    string inputRoca = "Circle"; //X. Izquierda

    string inputArbustos = "Cross";    //Derecha

    string inputArbol = "Triangle"; //Y. Arriba

    Animator anim;

    bool animPress = false;

    string animRoca = "RocaApartarse";
    string animTronco = "TroncoAgacharse";
    string animArbol = "ArbolRalentizar";

    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        skipTime = 1;
        
        //Suscribimos la función al delegado
        gng.SuscribeDelegate(CheckControls);


    }

    private void Update()
    {
        if (gng.get_loading() == false && gng.get_introduction() == false) { 
            if ((Input.GetAxis(inputRoca) > 0 || Input.GetKey(KeyCode.X)) && animPress == false)
            {
                anim.SetBool(animRoca, true);
                print("Apartarse");
            
                StartCoroutine(Control(animPress, 1, animRoca));
            }
            else if ((Input.GetAxis(inputTronco) > 0 || Input.GetKey(KeyCode.T)) && animPress == false)
            {
                anim.SetBool(animTronco, true);
                print("Agacharse");

                StartCoroutine(Control(animPress, 1, animTronco));
            }
            else if ((Input.GetAxis(inputArbol) > 0 || Input.GetKey(KeyCode.Z)) && animPress == false)
            {
                anim.SetBool(animArbol, true);
                print("Ralentiza paso");

                StartCoroutine(Control(animPress, 1, animArbol));
            }
        }
    }

    private void CheckControls()
    {
        if(outT == false && controlJoystick == false)
        {
            print("GC --> Se ha pulsado después del límite del caso de omisión (antes de que el trigger colisionase o por error)");
            StartCoroutine(ControlTheControllers());
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (controlElementCount == false && (other.gameObject.tag == "WebAndSpider" || other.gameObject.tag == "Web" || other.gameObject.tag == "Rock"|| other.gameObject.tag == "TwoRocks" || other.gameObject.tag == "TroncoEntero" || other.gameObject.tag == "TroncoPartido" || other.gameObject.tag == "BushBu" || other.gameObject.tag == "ArbolGordito" || other.gameObject.tag == "ArbolFino"))
        {
            gng.set_elemRunCount();
            StartCoroutine(ControlTheElementsCount());
            outT = true;

            if (other.gameObject.tag == "ArbolGordito" || other.gameObject.tag == "ArbolFino")
            {
                other.gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetTrigger("Falling");
            }
        }



    }

    void OnTriggerStay(Collider other)
    {
        
        #region Spider+Web
        //Caso en el que está a rango y ha golpeado perfecto:
        if (other.gameObject.tag == "WebAndSpider" && (other.transform.position.x - tr.position.x) <= distanceGoodSkill && Input.GetAxis(inputWeb) > 0 && controlJoystick == false)
        {
            print("GoNoGoC --> Está dentro del trigger y se ha golpeado correctamente");
            
            gng.set_plus_aciertoCount();

            other.gameObject.transform.parent.GetComponent<Animator>().SetBool("SpiderJumping", true);

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        //Caso en el que está a rango pero se ha adelantado:
        else if (other.gameObject.tag == "WebAndSpider" && (other.transform.position.x - tr.position.x) > distanceGoodSkill && Input.GetAxis(inputWeb) > 0 && controlJoystick == false)
        {
            print("GoNoGoC --> Está dentro del trigger y se ha adelantado");

            gng.set_plus_errorCount();

            other.gameObject.GetComponent<Animator>().SetBool("SpiderJumping", true);

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        #endregion Spider+Web

        #region Web
        //Cuando el objeto no tiene la araña
        if (other.gameObject.tag == "Web" && Input.GetAxis(inputWeb) > 0 && controlJoystick == false)
        {
            print("GoNoGoC --> No tiene arañita!");
            gng.set_plus_errorCount();
            Destroy(other.transform.parent.gameObject);
            outT = false;

            StartCoroutine(ControlTheControllers());
        }
        #endregion Web

        #region Rock
        //Caso en el que está a rango y ha golpeado perfecto:
        if (other.gameObject.tag == "Rock" && (other.transform.position.x - tr.position.x) <= distanceGoodSkill && (Input.GetAxis(inputRoca) > 0 || Input.GetKey(KeyCode.X)) && controlJoystick == false)
        {
            gng.set_plus_aciertoCount();
            print("on point");
            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        //Caso en el que está a rango pero se ha adelantado:
        else if (other.gameObject.tag == "Rock" && (other.transform.position.x - tr.position.x) > distanceGoodSkill && (Input.GetAxis(inputRoca) > 0 || Input.GetKey(KeyCode.X)) && controlJoystick == false)
        {
            gng.set_plus_errorCount();
            print("muy rápido");
            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        #endregion Rock

        #region TwoRocks
        if (other.gameObject.tag == "TwoRocks" && Input.GetAxis(inputRoca) > 0)
        {
            gng.set_plus_errorCount();
            Destroy(other);

            outT = false;
            StartCoroutine(ControlTheControllers());
        }
        #endregion TwoRocks

        #region TroncoEntero
        //Caso en el que está a rango y ha golpeado perfecto:
        if (other.gameObject.tag == "TroncoEntero" && (other.transform.position.x - tr.position.x) <= distanceGoodSkill && Input.GetAxis(inputTronco) > 0 && controlJoystick == false)
        {
            gng.set_plus_aciertoCount();

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }

        else if (other.gameObject.tag == "TroncoEntero" && (other.transform.position.x - tr.position.x) > distanceGoodSkill && Input.GetAxis(inputTronco) > 0 && controlJoystick == false)
        {
            gng.set_plus_errorCount();

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        #endregion TroncoEntero

        #region TroncoPartido
        if (other.gameObject.tag == "TroncoPartido" && Input.GetAxis(inputTronco) > 0 && controlJoystick == false)
        {
            print("GoNoGoC --> No tiene arañita!");
            gng.set_plus_errorCount();
            Destroy(other);
            outT = false;

            StartCoroutine(ControlTheControllers());
        }

        #endregion TroncoPartido

        #region Bush
        //Caso en el que está a rango y ha golpeado perfecto:
        if (other.gameObject.tag == "BushBu" && (other.transform.position.x - tr.position.x) <= distanceGoodSkill && Input.GetAxis(inputArbustos) > 0 && controlJoystick == false)
        {
            gng.set_plus_aciertoCount();

            other.gameObject.GetComponent<Animator>().SetTrigger("Bushes");

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        //Caso en el que está a rango pero se ha adelantado:
        else if (other.gameObject.tag == "BushBu" && (other.transform.position.x - tr.position.x) > distanceGoodSkill && Input.GetAxis(inputArbustos) > 0 && controlJoystick == false)
        {
            gng.set_plus_errorCount();

            other.gameObject.GetComponent<Animator>().SetTrigger("Bushes");

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        #endregion Bush

        #region ThinTree
        //Caso en el que está a rango y ha golpeado perfecto:
        if (other.gameObject.tag == "ArbolFino" && (other.transform.position.x - tr.position.x) <= distanceGoodSkill && Input.GetAxis(inputArbol) > 0 && controlJoystick == false)
        {
            gng.set_plus_aciertoCount();

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        //Caso en el que está a rango pero se ha adelantado:
        else if (other.gameObject.tag == "ArbolFino" && (other.transform.position.x - tr.position.x) > distanceGoodSkill && Input.GetAxis(inputArbol) > 0 && controlJoystick == false)
        {
            gng.set_plus_errorCount();           

            outT = false;
            Destroy(other);

            StartCoroutine(ControlTheControllers());
        }
        #endregion ThinTree

        #region BigTree
        if (other.gameObject.tag == "ArbolGordito" && Input.GetAxis(inputTronco) > 0 && controlJoystick == false)
        {
            gng.set_plus_errorCount();
            Destroy(other);
            outT = false;

            StartCoroutine(ControlTheControllers());
        }
        #endregion BigTree
    }

    void OnTriggerExit(Collider other)
    {
        //Preparamos el caso en el que se daría después de pasar el objeto. Si sigue en true es que no se le ha golpeado a la araña en el tiempo correspondiente.
        if (other.gameObject.tag == "WebAndSpider" && outT == true )
        {
            print("Salimos del Trigger y no se ha golpeado aún");
            StartCoroutine(SkipTime(other, inputWeb));
           
        }
        else if(other.gameObject.tag == "Web" && outT == true)
        {
            print("Hemos salido del Trigger y no se ha golpeado. Vamos a comprobar si no le da más tarde, a pesar de no tener araña");           
            StartCoroutine(SkipTime_Contrarios(other, inputWeb));
        }
        else if (other.gameObject.tag == "Rock" && outT == true)
        {
            StartCoroutine(SkipTime(other, inputRoca));
        }
        else if (other.gameObject.tag == "TwoRocks" && outT == true)
        {
            StartCoroutine(SkipTime_Contrarios(other, inputRoca));
        }
        else if (other.gameObject.tag == "TroncoEntero" && outT == true)
        {
            StartCoroutine(SkipTime(other, inputTronco));
            
        }
        else if (other.gameObject.tag == "TroncoPartido" && outT == true)
        {
            StartCoroutine(SkipTime_Contrarios(other, inputTronco));
            
        }
        else if (other.gameObject.tag == "BushBu" && outT == true)
        {
            StartCoroutine(SkipTime(other, inputArbustos));

        }
        else if (other.gameObject.tag == "ArbolFino" && outT == true)
        {
            StartCoroutine(SkipTime(other, inputArbol));

        }
        else if (other.gameObject.tag == "ArbolGordito" && outT == true)
        {
            StartCoroutine(SkipTime_Contrarios(other, inputArbol));

        }
    }

    #region SkipTime
    IEnumerator SkipTime(Collider _other, string input)   //Le pasamos otro parámetro con el input correspondiente.
    {
        float time = 0;
        for (; time <= skipTime && outT == true; time += Time.deltaTime)
        {
            if (Input.GetAxis(input) > 0 && controlJoystick == false)
            { //Si se golpea se saldrá del bucle       
                print("Nos salimos del bucle. Lo ha hecho un poco más tarde, pero no llega a ser caso de omisión");
                gng.set_plus_aciertoCount();
                Destroy(_other);
                outT = false;

                if (input == inputArbol && _other.gameObject.GetComponent<Animator>() != null)
                    _other.gameObject.GetComponent<Animator>().SetBool("SpiderJumping", true);

                StartCoroutine(ControlTheControllers());
            }

            yield return null;
        }

        //Caso de omisión
        if (time >= skipTime)
        {
            print("GoNoGoC --> Caso de omisión. Si se le da posteriormente es que ha reaccionado más tarde.");
            gng.set_plus_errorCount();
            //dataSave.set_errorXOmision();
            outT = false;
        }
        yield return null;
    }

    IEnumerator SkipTime_Contrarios(Collider _other, string input)//Le pasamos otro parámetro con el input correspondiente.
    {
        float time = 0;
        for (; time <= skipTime && outT == true; time += Time.deltaTime)
        {
            if (Input.GetAxis(input) > 0  && controlJoystick == false)
            { //Si se golpea se saldrá del bucle       
                print("Maaal");
                gng.set_plus_errorCount();
                Destroy(_other);
                outT = false;

                StartCoroutine(ControlTheControllers());
            }

            yield return null;
        }

        //Caso de omisión
        if (time >= skipTime)
        {
            gng.set_plus_aciertoCount();
            outT = false;
        }
        yield return null;
    }
    #endregion SkipTime

    IEnumerator ControlTheControllers()
    {
        controlJoystick = true;
        yield return new WaitForSeconds(1f);
        controlJoystick = false;
    }

    IEnumerator ControlTheElementsCount()
    {
        controlElementCount = true;
        yield return new WaitForSeconds(2.5f);
        controlElementCount = false;
    }

    IEnumerator Control(bool boolean, float time, string animName)
    {
        boolean = true;
        yield return new WaitForSeconds(time);
        boolean = false;

        anim.SetBool(animName, false);
    }

}
