using UnityEngine;
using System.Collections;

public class TRubikElementOld : ScriptableObject {
	
	GameObject[] Elements;
	public GameObject SideElement;
	float ElementSize = 1.0f;
	
	
	public void Init(GameObject prefab, Vector3 center) {
		
	}
	
	public void Init2(GameObject prefab, Vector3 position) {

		float[] sides = new float[2];
		sides[0] = -ElementSize / 2;
		sides[1] =  ElementSize / 2;
		
		Elements = new GameObject[6];
		int counter = 0;
		for(int i = 0; i < 3; ++i) {
			foreach(float side in sides) {
				Vector3 delta = new Vector3(0, 0, 0);
				delta[i] += side;
				Vector3 rotate = new Vector3();
				
				if (i == 0) {
					rotate[2 - i] = -side * 180;
				} else if (i == 2) {
					rotate[2 - i] = side * 180;
				} else if ( i == 1) {
					rotate[2 - i] = side * 180;
					if (side == sides[0])
						rotate[2] = 180;
				}
				
				Quaternion q = Quaternion.Euler(rotate);
				GameObject go = (GameObject)Instantiate(prefab, position + delta, q);
				Elements[counter] = go;
				counter += 1;
			}
		};
		/*
		Elements = new GameObject[6];
		Vector3 delta = new Vector3(0, -ElementSize, 0);
		GameObject go = (GameObject)Instantiate(prefab, position + delta, q);
		Elements[0] = go;
		//go.parent = this;
		*/
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
