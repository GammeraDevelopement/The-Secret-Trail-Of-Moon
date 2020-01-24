using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectVR
{
    /// <summary>
    /// Clase que representa la interfaz del objeto
    /// </summary>
    public class VRUIObject : MonoBehaviour
    {
        //Instancia
        public static VRUIObject Instance = null;

        private CanvasGroup myCanvas;
        private Text myText;

        private IEnumerator UICoroutine;

        void Start()
        {
            Instance = this;

            this.myCanvas = this.GetComponent<CanvasGroup>();
            this.myText = this.GetComponentInChildren<Text>();
        }

        /// <summary>
        /// Metodo para mostrar la interfaz del objeto
        /// </summary>
        /// <param name="description"></param>
        public void ActiveUI(string description, float timeDelay)
        {
            this.myText.text = CorgiTools.ReplaceButtonTutorial(description);

            if(this.UICoroutine != null)
                StopCoroutine(this.UICoroutine);
            this.UICoroutine = this.ActiveUICo(timeDelay);
            StartCoroutine(this.UICoroutine);
        }

        private IEnumerator ActiveUICo(float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);
            this.myCanvas.alpha = 1;
        }

        /// <summary>
        /// Metodo para ocultar la interfaz del objeto
        /// </summary>
        public void DisableUI()
        {
            if (this.UICoroutine != null)
                StopCoroutine(this.UICoroutine);
            this.myCanvas.alpha = 0;
        }
    }
}