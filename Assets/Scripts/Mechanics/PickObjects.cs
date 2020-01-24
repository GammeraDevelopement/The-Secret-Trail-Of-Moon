using UnityEngine;
using System.Collections;

public class PickObjects : MonoBehaviour {

    public Transform holdPlace;
    public bool leavePlace;

    public AudioSource source;

    void Action() {
        if (!leavePlace) {
            TakeObject();
        } else {
            LeaveObject();
        }
    }

    void TakeObject() {
        if (holdPlace.childCount == 0) {
            transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            transform.rotation = holdPlace.rotation;
            transform.position = holdPlace.position;
            transform.SetParent(holdPlace);
            disableCollider(transform.GetComponent<Collider>(), false);
        }
    }

    void LeaveObject() {
        if (transform.childCount > 0) { //Coger cubo del sitio
            transform.GetChild(0).position = holdPlace.position;
            transform.GetChild(0).rotation = holdPlace.rotation;
            disableCollider(transform.GetChild(0).GetComponent<Collider>(), false);
            transform.GetChild(0).SetParent(holdPlace);
            source.Play();
        } else { //Dejar cubo en el sitio
            if (holdPlace.childCount > 0) {
                if (source != null) {
                    source.Play();
                }
                holdPlace.GetChild(0).position = transform.position;
                disableCollider(holdPlace.GetChild(0).GetComponent<Collider>(), true);
                holdPlace.GetChild(0).rotation = holdPlace.GetChild(0).localRotation;
                holdPlace.GetChild(0).SetParent(transform, true);
            }

        }

    }

    void ActivateGravity() {
        Debug.Log("gravity activated");

            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            transform.SetParent(null, true);
            disableCollider(transform.GetComponent<Collider>(), true);

    }

    void disableCollider(Collider col, bool enable) {
        col.enabled = enable;
    }

    private void OnCollisionEnter(Collision collision) {

    }
}