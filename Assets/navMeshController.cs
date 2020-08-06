using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    public int contador = 0;
    public int parte;
    public bool finish = false;

    public Transform destino;

    public List<GameObject> positionsGuion1 = new List<GameObject>();
    public List<GameObject> positionsGuion2 = new List<GameObject>();

    private NavMeshAgent agente;

    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        parteDelGuion(parte);
    }

    /*public void NuevaPosicion()
    {
        destino = GameObject.Find("Objetivo" + numero).transform;
    }*/

    public void Move(NavMeshAgent agente)
    {
        if (parte == 1)
        {
            agente.destination = positionsGuion1[contador].transform.position; //destino.transform.position;
        }
        else if (parte == 2)
        {
            agente.destination = positionsGuion2[contador].transform.position;
        }
    }

    public void parteDelGuion(int parte) {

        if (parte == 1) {

            if (positionsGuion1.Count == 0 || contador >= positionsGuion1.Count)
            {

                Debug.Log("Lista vacia/finalizada");
                finish = true;
            }
            else
            {
                Move(agente);

                if ((destino.position.x == agente.transform.position.x) && (destino.position.z == agente.transform.position.z))
                {

                    contador++;
                }
            }
        }
        else if (parte == 2) {
            
            if (positionsGuion2.Count == 0 || contador >= positionsGuion2.Count)
            {

                Debug.Log("Lista vacia/finalizada");
                finish = true;
            }
            else
            {
                
                Move(agente);

                if ((destino.position.x == agente.transform.position.x) && (destino.position.z == agente.transform.position.z))
                {
                   
                    contador++;
                }
            }
        }
    }

    public bool GetFinish() {

            return finish;
    }
}
