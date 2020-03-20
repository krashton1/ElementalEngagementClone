using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public AnimationCurve scrollSpeed;
	public float smoothTime = 1.2f;
	float smoothVelocity;
	float currentSpeed;

	float zoom = 0.5f;
	public float mouseScrollSensitivity = 1;

	public float zoomTime = 1.2f;
	float zoomVelocity;
	float currentZoom;


	public Vector3 position;
	Vector3 movementDirection;

	public AnimationCurve angle;

	void Start() {
		position = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		zoom -= Input.GetAxis("Mouse ScrollWheel") * mouseScrollSensitivity;
		if (Camera.main.orthographic){
			zoom = Mathf.Clamp(zoom, 0.5f, 1.3f);
			Camera.main.orthographicSize = 20 * zoom;
		}
		else{
			zoom = Mathf.Clamp(zoom, 0.1f, 1.5f);
		}
		currentZoom = Mathf.SmoothDamp (currentZoom, zoom, ref zoomVelocity, zoomTime);

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;
		float targetSpeed = 120 * scrollSpeed.Evaluate(currentZoom) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothVelocity, smoothTime);
		if (inputDir != Vector2.zero){
			movementDirection = Quaternion.AngleAxis(45f, Vector3.up) * new Vector3(inputDir.x, 0, inputDir.y);
		}

		position += movementDirection * currentSpeed * Time.deltaTime;

		transform.rotation = Quaternion.Euler(50 * angle.Evaluate(currentZoom), 45, transform.rotation.z);

 		transform.position = new Vector3(position.x, 0, position.z) + new Vector3(-1, 3,-1).normalized * currentZoom * 50;
		
	}

	
}
