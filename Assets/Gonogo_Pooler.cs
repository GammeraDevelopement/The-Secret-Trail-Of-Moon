using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gonogo_Pooler : MonoBehaviour
{
    public GameObject objPrefab;                                                //El objeto que vamos a estar reutilizando

    public int poolSize;                                                        //Cuantos objetos se necesitaran

    private Queue<GameObject> objPool;                                          //La "alberca" donde estarán los objetos

    void Start() {
        objPool = new Queue<GameObject>();                                      //Inicializamos la cola

        for (int i = 0; i < poolSize; i++)                                      //Vamos a llenar la alberca en base al tamaño
        {  
            GameObject newObj = Instantiate(objPrefab);                         //Instanciamos el objeto y lo guardamos en una varible temporal  
            objPool.Enqueue(newObj);                                            //Lo añadimos a la cola con Enqueue
            newObj.SetActive(false);                                            //Lo desactivamos ya que en ese momento no se requiere
        }
    }

    public GameObject GetObjFromPool(Vector3 newPosition, Quaternion newRotation) {
        GameObject newObj = objPool.Dequeue();                                  //Se obtiene el 1er objeto disponible en la cola
        newObj.SetActive(true);                                                 //Activamos el objeto, se activa su comportamiento
        newObj.transform.SetPositionAndRotation(newPosition, newRotation);      //Le damos la posición y rotación, en donde se necesita que este
        return newObj;
    }

    public void ReturnObjToPool(GameObject go) {
        go.SetActive(false);                                                    //Lo desactivamos
        objPool.Enqueue(go);                                                    //Lo volvemos a añadir a la cola para reutilizarlo
    }

}
