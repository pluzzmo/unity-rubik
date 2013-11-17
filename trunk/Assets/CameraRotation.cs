using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
	
	public float Speed;
	// Use this for initialization
	void Start () {
		//this.transform.position = new Vector3(0, -1, 5);
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 oldTransform = this.transform.rotation;
		float p = Speed * Time.deltaTime;
		this.transform.Rotate(new Vector3(p/4, p, p/2));
	}
}
