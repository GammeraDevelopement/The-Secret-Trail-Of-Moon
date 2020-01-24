using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is contained by GeneradorDe
public class GenerateElements_Controller : MonoBehaviour
{
    /// <summary>
    /// Tiempo en milisegundos entre cada aparición de elementos
    /// </summary>
    float timeSpawn;

    /// <summary>
    /// Todos los targets que deberán aparecer de manera aleatoria
    /// </summary>
    [SerializeField] List<GameObject> TargetList;

    /// <summary>
    /// Posición random en la lista 
    /// </summary>
    int numRandom;

    /// <summary>
    /// Posición en la que van a Spawnear 
    /// </summary>
    [SerializeField] Transform positionSpawn;

    /// <summary>
    /// Como cogemos una referencia de posición que se va a ir moviendo, asignamos su posición en el start en esta variable.
    /// </summary>
    Vector3 position;

    /// <summary>
    /// Referencia del script GoNoGo
    /// </summary>
    [SerializeField] GoNoGo gng;

    float moreProbabTarget;
    float lessProbabTarget;

    /// <summary>
    /// Referencia del script GoNoGo_SetData
    /// </summary>
    [SerializeField] GoNoGo_SetData data;

    int elemRandomSecondRound;

    void Start()
    {
        timeSpawn = data.getDificultad(gng.nivelActual).TiempoAparicionEstimulos / 1000;
        moreProbabTarget = data.getDificultad(gng.nivelActual).FrecuenciaAparRonda2;
        lessProbabTarget = 1 - moreProbabTarget;

        position = positionSpawn.position;

        elemRandomSecondRound = Random.Range(0, data.getDificultad(gng.nivelActual).NElementos);
    }

    public IEnumerator GenerateFirstRound(byte min, byte max)
    {
        while (gng.get_stateFirstRound() || gng.get_stateSecondRound())
        {
            
            if(gng.get_stateFirstRound() == true)
            {
                numRandom = Random.Range(min, max);
                
                GameObject targetPrefab = TargetList[numRandom];

                if (numRandom == 1)
                    Instantiate(targetPrefab, position, Quaternion.identity);

                else
                    Instantiate(targetPrefab, position, Quaternion.Euler(0, 90, 0));

            }
            else
            {
                //do
                //{
                    numRandom = GetRandomValue(
                        new RandomSelection(min, max, lessProbabTarget),
                        new RandomSelection(elemRandomSecondRound,elemRandomSecondRound, moreProbabTarget)
                         );
                //} while ((data.getDificultad(gng.nivelActual).NElementos) == 2 && (numRandom == 2 || numRandom == 5));

                print(numRandom);

                GameObject targetPrefab = TargetList[numRandom + 5];

                if (numRandom == 1 || numRandom == 4)
                    Instantiate(targetPrefab, position, Quaternion.identity);

                else
                    Instantiate(targetPrefab, position, Quaternion.Euler(0, 90, 0));

            }

            gng.set_plus_elemCounter();

            yield return new WaitForSeconds(timeSpawn);
        }

    }

    /*
    public IEnumerator GenerateSecondRound()
    {
        while (gng.get_stateSecondRound())
        {
            yield return new WaitForSeconds(timeSpawn);
            //Si tenemos más elementos de los permitidos en el nivel, volvemos a buscar un número random.
            do
            {
                numRandom = GetRandomValue(
                    new RandomSelection(0, 2, lessProbabTarget),
                    new RandomSelection(3, 5, moreProbabTarget)
                     );
            }while((data.getDificultad(gng.nivelActual).NElementos) == 2 && (numRandom == 2 || numRandom == 5));

            GameObject targetPrefab = TargetList[numRandom];
            gng.targetPrefabs.Add(targetPrefab);

            if (numRandom == 1 || numRandom == 4)
                Instantiate(targetPrefab, position, Quaternion.identity);

            else
                Instantiate(targetPrefab, position, Quaternion.Euler(0, 90, 0));


            gng.set_plus_elemCounter();
            
        }

    }
    */

    #region RandomStructure
    //Estructura para generar el random
    struct RandomSelection
    {
        private int minValue;
        private int maxValue;
        public float probability;

        public RandomSelection(int minValue, int maxValue, float probability)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.probability = probability;
        }

        public int GetValue() { return Random.Range(minValue, maxValue + 1); }
    }


    private int GetRandomValue(params RandomSelection[] selections)
    {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections)
        {
            currentProb += selection.probability;
            if (rand <= currentProb)
                return selection.GetValue();
        }

        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return -1;
    }
    #endregion RandomStructure
}
