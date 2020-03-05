using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is contained by GeneradorDe
public class GNG_GenerateTargets : MonoBehaviour {

    public List<GameObject> TargetList;           /// Todos los targets que deberán aparecer de manera aleatoria
    public Transform positionSpawn;               /// Posición en la que van a Spawnear
    public GNG_GameController gng;                            /// Referencia del script GoNoGo
    public Terrain parent;

    private float timeSpawn;                      /// Tiempo en milisegundos entre cada aparición de elementos
    private int numRandom;                        /// Posición random en la lista
    private float moreProbabTarget;
    private float lessProbabTarget;
    private int elemRandomSecondRound = 2;

    private GNG_DifficultyJSON data;         /// Referencia del script GoNoGo_SetData


    void Start() {
        timeSpawn = 2;//data.getDificultad(gng.nivelActual).TiempoAparicionEstimulos / 1000; //MAAAAAAAAAAAAAL
        moreProbabTarget = 2;// data.getDificultad(gng.nivelActual).FrecuenciaAparRonda2;
        lessProbabTarget = 1 - moreProbabTarget;
        //elemRandomSecondRound = Random.Range(0, data.getDificultad(gng.nivelActual).NElementos);
    }

    public IEnumerator GenerateFirstRound(int min, int max) {
        Debug.Log("generate coroutine");
        while (gng.get_stateFirstRound() || gng.get_stateSecondRound()) {

            if (gng.get_stateFirstRound() == true) {
                numRandom = Random.Range(min, max);

                GameObject targetPrefab = TargetList[numRandom];


                Instantiate(targetPrefab, positionSpawn.position, targetPrefab.transform.rotation, positionSpawn.transform);

            } else {
                numRandom = GetRandomValue(
                    new RandomSelection(min, max, lessProbabTarget),
                    new RandomSelection(elemRandomSecondRound, elemRandomSecondRound, moreProbabTarget)
                     );

                print(numRandom);
                GameObject targetPrefab = TargetList[numRandom + 5];

                Instantiate(targetPrefab, positionSpawn.position, targetPrefab.transform.rotation, positionSpawn.transform);
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
