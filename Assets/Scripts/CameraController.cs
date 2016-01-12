using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float mouseSensitivity  = 0.01f;
	private Vector3 lastPosition;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			lastPosition = Input.mousePosition;
		}
		
		if (Input.GetMouseButton(0))
		{
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
			lastPosition = Input.mousePosition;
		}

		if (Input.GetMouseButtonDown(1))
		{
			lastPosition = Input.mousePosition;
		}
		
		if (Input.GetMouseButton(1))
		{
			Vector3 delta = Input.mousePosition - lastPosition;
			transform.Rotate(-delta.y * mouseSensitivity * 5, delta.x * mouseSensitivity * 5, 0);
			lastPosition = Input.mousePosition;
		}
	}
}
