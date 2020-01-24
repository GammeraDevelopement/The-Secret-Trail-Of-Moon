using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class PiedraCollision : MonoBehaviour {

	public GameObject explosion;
    public GameObject objetoADestruir;
    public bool explotable = false;

	private bool flag = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col){
		if (col.collider.tag == "Destroyer") {
			if (flag) {
				StartCoroutine (morir ());
				flag = false;
			}
		}
	}

	private IEnumerator morir(){
        if (explotable) {
            Instantiate<GameObject>(explosion, this.transform);
            gameObject.GetComponent<MeshFilter>().mesh = null;
        }
		this.GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (1.0F);
        int childs = transform.childCount;
        for (int i = childs - 1; i > 0; i--) {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        Destroy(objetoADestruir);
    }
}
