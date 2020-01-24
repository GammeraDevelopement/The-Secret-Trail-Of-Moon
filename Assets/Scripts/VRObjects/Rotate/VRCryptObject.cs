using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa un cripto VR en la escena
    /// </summary>
    public class VRCryptObject: VRRotateObject
    {
        [System.Serializable]
        public struct CryptRound
        {
            public List<VRLoopObject> Pieces;
            public List<int> Solution;
        }

        public List<CryptRound> CryptRounds;
        private CryptRound currentRound;
        private int indexRound = 0, indexPiece = 0;

        //Indica si podemos movernos por el cripto o si hemos terminado
        private bool canMove, isEnd;
        //Corutina de movimiento del selector
        private IEnumerator MoveSelectorCoroutine;

        public override void Update()
        {
            base.Update();

            if (!this.canMove || this.isEnd)
                return;
        }

        /// <summary>
        /// Metodo para comprobar el puzzle
        /// </summary>
        public void CheckPuzzle()
        {
            bool check = true;
            for(int i = 0; i < this.currentRound.Pieces.Count; i++)
            {
                if (this.currentRound.Pieces[i].NumActivated != this.currentRound.Solution[i])
                {
                    check = false;
                    break;
                }
            }

            if(check)
            {
                ExecuteEvents.Execute(this.currentRound.Pieces[0].transform.parent.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

                if(this.indexRound < this.CryptRounds.Count - 1)
                {                    
                    this.indexRound++;
                    this.indexPiece = 0;
                    this.currentRound = this.CryptRounds[this.indexRound];

                    this.currentRound.Pieces[this.indexPiece].SelectObject(true);
                } 
                else
                {
                    this.canMove = false;
                    this.isEnd = true;

                    StopCoroutine(this.MoveSelectorCoroutine);
                }
            }
            else
            {
                
            }
        }

        protected override IEnumerator ActivateObjectEventCo(bool active)
        {
            if (this.isEnd)
                yield break;

            if (active)
            {
                this.GetComponent<BoxCollider>().enabled = false;

                this.currentRound = this.CryptRounds[this.indexRound];
                this.indexPiece = 0;

                this.currentRound.Pieces[this.indexPiece].SelectObject(true);
                this.canMove = true;

                this.MoveSelectorCoroutine = this.MoveSelector();
                StartCoroutine(this.MoveSelectorCoroutine);
            }
            else
            {
                this.GetComponent<BoxCollider>().enabled = true;

                this.currentRound.Pieces[this.indexPiece].SelectObject(false);
                this.canMove = false;

                StopCoroutine(this.MoveSelectorCoroutine);
            }

            yield return new WaitForSeconds(0);
        }

        /// <summary>
        /// Metodo para mover una pieza del cripto
        /// </summary>
        /// <returns></returns>
        private IEnumerator MovePiece()
        {
            this.canMove = false;
            yield return StartCoroutine(this.currentRound.Pieces[this.indexPiece].ActivateEvent());
            this.canMove = true;
        }

        /// <summary>
        /// Metodo para mover el selector del cripto
        /// </summary>
        /// <returns></returns>
        private IEnumerator MoveSelector()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);

                if (Input.GetAxis("Horizontal") > 0)
                {
                    this.currentRound.Pieces[this.indexPiece].SelectObject(false);
                    this.indexPiece = (this.indexPiece < this.currentRound.Pieces.Count - 1) ? this.indexPiece + 1 : 0;
                    this.currentRound.Pieces[this.indexPiece].SelectObject(true);
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    this.currentRound.Pieces[this.indexPiece].SelectObject(false);
                    this.indexPiece = (this.indexPiece > 0) ? this.indexPiece - 1 : this.currentRound.Pieces.Count - 1;
                    this.currentRound.Pieces[this.indexPiece].SelectObject(true);
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    yield return StartCoroutine(this.MovePiece());
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    yield return StartCoroutine(this.MovePiece());
                }
            }
        }
    }
}