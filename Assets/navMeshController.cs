using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navMeshController : MonoBehaviour
{
    public int posicion = 1;

    public Transform destino;
    //public Vector3 hola;
    //public Transform destino;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       //Llamar a waitForSeconds
        NavMeshAgent agente = GetComponent<NavMeshAgent>();

        Move(agente);

        if ((destino.position.x == agente.transform.position.x) && (destino.position.z == agente.transform.position.z))
        {
            if (posicion <= 3)
            {
                NuevaPosicion();
            }
            else {

                posicion = 1;
            }
        }
        
    }

    public void NuevaPosicion() {

        destino = GameObject.Find("Objetivo" + posicion).transform;
        posicion ++;
    }

    public void Move(NavMeshAgent agente) {
        agente.destination = destino.transform.position;
    }
}
