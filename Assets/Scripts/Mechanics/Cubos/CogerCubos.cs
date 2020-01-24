using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CogerCubos : MonoBehaviour {

    private const string INTERACTION_BUTTON = "Cross";
    private const string X_AXIS = "DpadRight";
    private const string Y_AXIS = "DpadUp";
    private const string ROTATE_FACE_RIGHT = "R1";
    private const string ROTATE_FACE_LEFT = "L1";
    private const string COLLIDER_TAG = "Interactable";

    public float min;
    public float interactionDistance = 40;
    public GameObject cubePlace;
    public float smooth = 1000;


    private bool hasCube = false;
    private bool flag = true;
    private bool x_isAxisInUse = false;
    private bool y_isAxisInUse = false;
    private bool cooldown = false;
    private Transform pickedCube;
    private Quaternion targetRotation;  //Rotación real del cubo

    private void Start() {
        targetRotation = transform.root.rotation;
    }


    void Update() {

        if (cubePlace.transform.childCount > 0) {
            hasCube = true;
            if (flag) {
                pickedCube = cubePlace.transform.GetChild(0);
                targetRotation = pickedCube.transform.rotation;
                flag = false;
            }
        } else {
            hasCube = false;
            flag = true;
            pickedCube = null;
        }

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red, Time.deltaTime);
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance,9<<10)) {
            if (hit.collider.tag == COLLIDER_TAG) {
                if (Input.GetButtonDown(INTERACTION_BUTTON)) {
                    hit.collider.SendMessage("Action");
                }
            }
        }

        if (!hasCube) {

        } else {

            //testing controls

            if (Input.GetAxisRaw(X_AXIS) == 1 && !x_isAxisInUse) {
                pickedCube.transform.Rotate(transform.up, 90, Space.World);
                x_isAxisInUse = true;
            } else if (Input.GetAxisRaw(X_AXIS) == -1 && !x_isAxisInUse) {
                pickedCube.transform.Rotate(transform.up, -90, Space.World);
                x_isAxisInUse = true;
            }
            if (Input.GetAxisRaw(X_AXIS) == 0) {
                x_isAxisInUse = false;
            }

            if (Input.GetAxisRaw(Y_AXIS) == 1 && !y_isAxisInUse) {
                pickedCube.transform.Rotate(transform.right, -90, Space.World);
                y_isAxisInUse = true;
            } else if (Input.GetAxisRaw(Y_AXIS) == -1 && !y_isAxisInUse) {
                pickedCube.transform.Rotate(transform.right, 90, Space.World);
                y_isAxisInUse = true;
            }
            if (Input.GetAxisRaw(Y_AXIS) == 0) {
                y_isAxisInUse = false;
            }

            if (Input.GetButtonDown(ROTATE_FACE_RIGHT)) {
                pickedCube.transform.Rotate(transform.forward, 90, Space.World);
            } else if (Input.GetButtonDown(ROTATE_FACE_LEFT)) {
                pickedCube.transform.Rotate(transform.forward, -90, Space.World);
            }

            if (Input.GetButtonDown("Circle") && hasCube) {
                pickedCube.SendMessage("ActivateGravity");
            }

        }


    }

    private IEnumerator rotationCooldown(Vector3 axis, float angle, float duration = 1.0f) {
        Quaternion from = pickedCube.transform.rotation;
        Quaternion to = Quaternion.AngleAxis(angle, axis);
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration) {

            pickedCube.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        pickedCube.transform.rotation = to;
        cooldown = false;
    }
}
