using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public int posicion = 1;
    public bool finish = false;

    public Transform destino;

    private NavMeshAgent agente;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Llamar a waitForSeconds
        
        //if activar true 
        Move(agente);

        if ((destino.position.x == agente.transform.position.x) && (destino.position.z == agente.transform.position.z))
        {
            if (GameObject.Find("Objetivo" + posicion)!=null)
            {
                NuevaPosicion();
                posicion++;
            }
            else
            {
                finish = true;
            }
        }

    }

    public void NuevaPosicion()
    {
        destino = GameObject.Find("Objetivo" + posicion).transform;
    }

    public void Move(NavMeshAgent agente)
    {
        agente.destination = destino.transform.position;
    }

    public bool GetFinish() {

            return finish;
    }
}
