using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is contained by GeneradorDe
public class GenerateElements_Controller : MonoBehaviour {

    public List<GameObject> TargetList;           /// Todos los targets que deberán aparecer de manera aleatoria
    public Transform positionSpawn;               /// Posición en la que van a Spawnear
    public GoNoGo gng;                            /// Referencia del script GoNoGo
    public Terrain parent;

    private float timeSpawn;                      /// Tiempo en milisegundos entre cada aparición de elementos
    private int numRandom;                        /// Posición random en la lista  
    private Vector3 position;                     /// Como cogemos una referencia de posición que se va a ir moviendo, asignamos su posición en el start en esta variable.
    private float moreProbabTarget;
    private float lessProbabTarget;
    private int elemRandomSecondRound;

    private GoNoGo_SetData data;         /// Referencia del script GoNoGo_SetData


    void Start() {
        timeSpawn = data.getDificultad(gng.nivelActual).TiempoAparicionEstimulos / 1000;
        moreProbabTarget = data.getDificultad(gng.nivelActual).FrecuenciaAparRonda2;
        lessProbabTarget = 1 - moreProbabTarget;

        position = positionSpawn.position;

        elemRandomSecondRound = Random.Range(0, data.getDificultad(gng.nivelActual).NElementos);
    }

    public IEnumerator GenerateFirstRound(int min, int max) {
        while (gng.get_stateFirstRound() || gng.get_stateSecondRound()) {
            if (gng.get_stateFirstRound() == true) {
                numRandom = Random.Range(min, max);

                GameObject targetPrefab = TargetList[numRandom];

                if (numRandom == 1) {
                    Instantiate(targetPrefab, position, Quaternion.identity, parent.transform);
                }

            } else {
                numRandom = GetRandomValue(
                    new RandomSelection(min, max, lessProbabTarget),
                    new RandomSelection(elemRandomSecondRound, elemRandomSecondRound, moreProbabTarget)
                     );

                print(numRandom);
                GameObject targetPrefab = TargetList[numRandom + 5];

                if (numRandom == 1 || numRandom == 4) {
                    Instantiate(targetPrefab, position, Quaternion.identity,parent.transform);
                } else {
                    Instantiate(targetPrefab, position, Quaternion.Euler(0, 90, 0),parent.transform);
                }
            }

            gng.set_plus_elemCounter();

            yield return new WaitForSeconds(timeSpawn);
        }

    }


    #region RandomStructure
    //Estructura para generar el random
    struct RandomSelection {
        private int minValue;
        private int maxValue;
        public float probability;

        public RandomSelection(int minValue, int maxValue, float probability) {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.probability = probability;
        }

        public int GetValue() { return Random.Range(minValue, maxValue + 1); }
    }


    private int GetRandomValue(params RandomSelection[] selections) {
        float rand = Random.value;
        float currentProb = 0;
        foreach (var selection in selections) {
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
