using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObectInteraction : MonoBehaviour {
    private const string FIRE_BUTTON = "Fire1";
    private const string X_AXIS = "DpadRight";
    private const string WHEEL_NAME = "Rueda";

    public float interactionDistance = 3f;
    public float speed = 10;
    public int nElementos;

    public GameObject[] selecciones;

    private GameObject[] ruedas;
    private Quaternion targetRotation;
    private bool cd = false;
    private bool x_isAxisInUse;
    private int[] movimientos;

    // Start is called before the first frame update
    void Start() {
        ruedas = new GameObject[selecciones.Length];
        for (int i = 0; i < ruedas.Length; i++) {
            ruedas[i] = GameObject.Find("RuedaEnigma" + (i + 1));
        }
        movimientos = new int[6];
        for (int i = 0; i < movimientos.Length; i++) {
            movimientos[i] = 1;
        }
        targetRotation = ruedas[0].transform.rotation;
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(transform.position, transform.forward * interactionDistance);

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, 1 << 8)) {

            for (int i = 0; i < ruedas.Length; i++) {

                if (hit.collider.gameObject.name == WHEEL_NAME + (i + 1)) {
                    selecciones[i].SetActive(true);

                    if ((Input.GetAxisRaw(X_AXIS) == 1 || Input.GetKeyDown("e"))  && !x_isAxisInUse) {
                        if (movimientos[i] < nElementos) {
                            hit.collider.transform.GetChild(0).transform.Rotate(Vector3.down, 45);
                            movimientos[i]++;
                            x_isAxisInUse = true;
                        }

                    } else if ((Input.GetAxisRaw(X_AXIS) == -1 || Input.GetKeyDown("q")) && !x_isAxisInUse) {
                        if (movimientos[i] > 1) {
                            hit.collider.transform.GetChild(0).transform.Rotate(Vector3.up, 45);
                            movimientos[i]--;
                            x_isAxisInUse = true;
                        }
                    }
                    if (Input.GetAxisRaw(X_AXIS) == 0) {
                        x_isAxisInUse = false;
                    }
                } else {
                    selecciones[i].SetActive(false);
                }

            }
        }

    }
}

