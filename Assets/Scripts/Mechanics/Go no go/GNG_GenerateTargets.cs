using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is contained by GeneradorDe
public class GNG_GenerateTargets : MonoBehaviour {

    public List<GameObject> TargetList;           /// Todos los targets que deberán aparecer de manera aleatoria
    public Transform positionSpawn;               /// Posición en la que van a Spawnear
    public string[] elements;

    [SerializeField]private GNG_GameController gng;                            /// Referencia del script GoNoGo
    [SerializeField]private Terrain parent;

    public float timeSpawn = 0;                      /// Tiempo en milisegundos entre cada aparición de elementos
    private int numRandom;                        /// Posición random en la lista
    private int firstTarget = 0;
    private int secondTarget = 0;

    private GNG_DifficultyJSON data;         /// Referencia del script GoNoGo_SetData


    void Start() {
        data = gng.GetComponent<GNG_DifficultyJSON>();
        if (timeSpawn == 0)
        {
            timeSpawn = data.getDificultad(gng.nivelActual).TiempoAparicionEstimulos / 1000;
        }

        firstTarget = 0;
        secondTarget = 0;

        if (gng.nivelActual > 16)
        {
            do
            {
                firstTarget = Random.Range(0, 5);
                secondTarget = Random.Range(0, 5);
            } while (firstTarget == secondTarget);
        }

        string[] split = elements[gng.nivelActual].Split(';');
        firstTarget = int.Parse(split[0]);
        secondTarget = int.Parse(split[1]);
    }

    public IEnumerator GenerateRound() {

        int count = 0;
        while (count < gng.nMaxElemRonda) {

            numRandom = Random.Range(0, 2);
            Debug.Log(numRandom);
            GameObject targetPrefab;
            if (numRandom == 0) {
                targetPrefab = TargetList[firstTarget];
            } else {
                if (gng.get_stateFirstRound() == true) {
                    targetPrefab = TargetList[secondTarget];
                } else {
                    targetPrefab = TargetList[secondTarget+5];
                }
            }

            Instantiate(targetPrefab, positionSpawn.position, targetPrefab.transform.rotation, positionSpawn.transform);

            gng.set_plus_elemCounter();
            count++;
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
