using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa el puzzle del automata
    /// </summary>
    public class AutomataEvent : MonoBehaviour
    {
        [System.Serializable]
        public struct AutomataRound
        {
            public List<string> SolutionBells;
            public List<string> AnimationTriggers;
        }

        public List<AutomataRound> Solution;
        public List<VRRotateObject> Bells;

        private int indexBell = 0, indexSolution = 0;
        private Animator _anim;

        private bool isPlaying;

        void Start()
        {
            this._anim = this.GetComponent<Animator>();
        }

        /// <summary>
        /// Metodo para activar el automata
        /// </summary>
        public void ActivateAutomata()
        {
            StartCoroutine(ActivateAutomataCo());
        }

        private IEnumerator ActivateAutomataCo()
        {
            yield return new WaitForSeconds(2);
            this._anim.SetTrigger("Activate");
        }

        public void ActivatePuzzle()
        {
            StartCoroutine(this.ActivatePuzzleCo());
        }

        public IEnumerator ActivatePuzzleCo()
        {
            this.isPlaying = true;

            yield return new WaitForSeconds(1);

            for (int i = 0; i < this.Solution[this.indexSolution].SolutionBells.Count; i++)
            {                
                this._anim.SetTrigger(this.Solution[this.indexSolution].AnimationTriggers[i]);
                yield return new WaitForSeconds(3);
            }            

            this.isPlaying = false;
        }

        /// <summary>
        /// Metodo para comprobar el puzzle
        /// </summary>
        public void CheckPuzzle(string nameBell)
        {
            if (this.isPlaying)
                return;

            if (nameBell == this.Solution[this.indexSolution].SolutionBells[indexBell])
            {
                if (this.indexBell < this.Solution[this.indexSolution].SolutionBells.Count)
                {
                    this.indexBell++;

                    if(this.indexBell >= this.Solution[this.indexSolution].SolutionBells.Count)
                    {
                        this.indexBell = 0;

                        if(this.indexSolution < this.Solution.Count - 1)
                        {
                            this.indexSolution++;
                            this.ActivatePuzzle();
                        }
                        else
                        {
                            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
                        }                           
                    }   
                }              
            }
            else
            {
                this.indexBell = 0;
                this.ActivatePuzzle();
            }
        }

        /// <summary>
        /// Metodo para activar las campanas
        /// </summary>
        /// <param name="active"></param>
        public void ActivateBellsObjects(bool active)
        {
            foreach (VRRotateObject bell in this.Bells)
            {
                bell.GetComponent<BoxCollider>().enabled = active;
            }                
        }
    }
}