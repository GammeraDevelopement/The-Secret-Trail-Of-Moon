using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectVR
{
    public class VRCinematicObject : MonoBehaviour
    {
        [System.Serializable]
        public struct CinematicVector
        {
            public Vector3 Vector;
            public float Time;
        }

        public List<CinematicVector> Distances;
        public List<CinematicVector> Rotations;        

        public void Move()
        {
            StartCoroutine(this.MoveCo());
        }

        public IEnumerator MoveCo()
        {
            foreach(CinematicVector dist in this.Distances)
                yield return StartCoroutine(CorgiTools.MoveFromTo(this.gameObject, this.transform.position, this.transform.position + dist.Vector, dist.Time));

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
        }

        public void MoveLocally()
        {
            StartCoroutine(this.MoveLocallyCo());
        }

        public IEnumerator MoveLocallyCo()
        {
            foreach (CinematicVector dist in this.Distances)
                yield return StartCoroutine(CorgiTools.MoveLocalFromTo(this.gameObject, this.transform.localPosition, this.transform.localPosition + dist.Vector, dist.Time));

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
        }

        public void Rotate()
        {
            StartCoroutine(this.RotateCo());
        }

        public IEnumerator RotateCo()
        {
            foreach (CinematicVector rot in this.Rotations)
                yield return StartCoroutine(CorgiTools.RotateLocalFromTo(this.gameObject, this.transform.localEulerAngles, rot.Vector, rot.Time));

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
        }

        public void FadeMaterial()
        {
            StartCoroutine(this.FadeMaterialCo());
        }

        public IEnumerator FadeMaterialCo()
        {
            Material mat = this.GetComponent<MeshRenderer>().material;
            yield return StartCoroutine(CorgiTools.FadeMaterial(mat, 0.25f, new Color(1, 1, 1, 0)));

            ExecuteEvents.Execute(this.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.cancelHandler);
        }        
    }
}