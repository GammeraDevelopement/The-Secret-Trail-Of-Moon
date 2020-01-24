using UnityEngine;
using System.Collections;

//Script que se pone en el jugador y en la camara para mover la camara con el ratón
//La copia del jugador llevará esCamara = false y sólo controlará el eje Y de la cámara(eje x del mundo)
//La copia de la camara tendrá esCamara = true y sólo controlará el eje x de la cámara (eje y del mundo)
public class RotarConCamara : MonoBehaviour {

	public bool esCamara;
	public bool ocultarCursor;
	public float sensibilidadX;
	public float sensibilidadY;
	public float minimoY;
	public float maximoY;
	public float minimoX;
	public float maximoX;

	private float rotateX;
	private float rotateY;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rotateX += Input.GetAxis("Mouse X") * sensibilidadX;
		rotateX = ClampAngle(rotateX,minimoX,maximoX);
		rotateY += Input.GetAxis("Mouse Y") * sensibilidadY;
		rotateY = ClampAngle(rotateY,minimoY,maximoY);
		Quaternion qX = Quaternion.AngleAxis(rotateX,Vector3.down);
		Quaternion qY = Quaternion.AngleAxis(rotateY,Vector3.right);
		if(esCamara){
			transform.localRotation = Quaternion.Inverse(qY);
		}else{
			transform.localRotation = Quaternion.Inverse(qX);
		}

		if(ocultarCursor){
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}
