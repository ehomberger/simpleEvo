using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		//transform.RotateTowards(Camera.main.transform.position, Vector3.left);
		//Transform cameraTransform = Camera.main.transform;
		//transform.rotation = new Quaternion(0, cameraTransform.rotation.y, 0, 0);
		var lookPos = Camera.main.transform.position - transform.position;
		lookPos.y = 0;
		var rotation = Quaternion.LookRotation(lookPos);
		transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 20F * Time.deltaTime);//Time.deltaTime);
	}
}
