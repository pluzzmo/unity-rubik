using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class TRubikCubik : MonoBehaviour {
	private GameObject[] Sides;
	public GameObject HolderObject;
	public static Color SelectedColor = new Color(0.2f, 0.2f, 0.2f);
	
	Color[] FaceColors;
	public void Init(GameObject holderObject, GameObject[] sides) {
		FaceColors = new Color[sides.Length];
		HolderObject = holderObject;
		Sides = sides;
		int idx = 0;
		foreach (GameObject go in Sides) {
			FaceColors[idx] = go.GetComponent<MeshRenderer>().material.color;
			go.transform.parent = holderObject.transform;
			idx += 1;
		}
	}
	
	void Start () {
		//Debug.Log("TFakeClass");
	}
	
	void Update () {
	}
	
	private void SetColor(Color color) {
		foreach (GameObject go in Sides) {
			//go.GetComponent<MeshRenderer>().material.color += SelectedColor;
			Color color1 = SelectedColor;
			color1.a = 0.9f;
			go.GetComponent<MeshRenderer>().material.color = color1;
		}
	}
	
	public void Select() {
		SetColor(SelectedColor);
	}

	public void Select2() {
		foreach (GameObject go in Sides) {
			go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		}
	}
	
	public void UnSelect() {
		int idx = 0;
		foreach (GameObject go in Sides) {
			go.GetComponent<MeshRenderer>().material.color = FaceColors[idx];
			idx += 1;
		}
	}
};

public class TCubikStorage {
	public List<TRubikCubik> Elements = new List<TRubikCubik>();
	
}

public class TRotationPlane : TCubikStorage {
	
};

public class TRubik2 : TCubikStorage {
	public List<GameObject> GOBS = new List<GameObject>();
	public List<TRotationPlane> XPlanes = new List<TRotationPlane>();
	public List<TRotationPlane> YPlanes = new List<TRotationPlane>();
	public List<TRotationPlane> ZPlanes = new List<TRotationPlane>();
	
	
	public TRotationPlane FindPlane(ref TRubikCubik cubik, ref List<TRotationPlane> planes) {
		for (int i = 0; i < planes.Count; ++i) {
			TRotationPlane plane = planes[i];
			foreach (TRubikCubik rc in plane.Elements) {
				if (rc == cubik)
					return plane;
			}
		}
		
		return null;
	}
	
	public void SelectRotationPlane(ref List<TRotationPlane> planes, ref TRubikCubik cubik, bool select) {
		TRotationPlane plane = FindPlane(ref cubik, ref planes);
		if (plane == null) {
			Debug.Log("Can't find plane");
			return;
		}
		foreach (TRubikCubik rc in plane.Elements) {
			if (select)
				rc.Select();
			else
				rc.UnSelect();
		}
	}
	
	public void SelectAvailablePlanes(ref TRubikCubik cubik, bool select) {
		SelectRotationPlane(ref XPlanes, ref cubik, select);
		SelectRotationPlane(ref YPlanes, ref cubik, select);
		SelectRotationPlane(ref ZPlanes, ref cubik, select);
	}
};

public class TRubikCreator2 : MonoBehaviour {
	const int Dimension = 3;
	static Color[] Colors2 = {
		Color.red,
		Color.yellow,
		new Color(0.1f,0.1f,0.1f),
		Color.white,
		Color.blue,
		Color.green
	};
	
	static Vector3 PrefabNormal = new Vector3(0, 1, 0);
	public static int CalcColorIndex(int idx, int subidx) {
		return idx * 2 + subidx;
	}
	
	public static TRubikCubik CreateSmallCubik(Vector3 cubikCenter, Vector3 totalCenter, GameObject prefab, float elementSize, ref string name) {
		Vector3 vector2CubikCenter = cubikCenter - totalCenter;
		List<GameObject> res = new List<GameObject>();
		
		float[] normalsValues = {-1.0f, 1.0f};
		for (int i = 0; i < 3; ++i) {
			int colorSubIndex = 0;
			foreach( float normalValue in normalsValues) {
				
				Color color = Colors2[CalcColorIndex(i, colorSubIndex)];

				Vector3 normal2Check = Vector3.zero;
				normal2Check[i] = normalValue * elementSize / 2;
				
				Vector3 normalFromPos = Vector3.zero;
				normalFromPos[i] = vector2CubikCenter[i];
				
				if (i == 2) {
					Debug.DrawLine(cubikCenter, cubikCenter + normal2Check, Color.red);
					Debug.DrawLine(cubikCenter, cubikCenter + normalFromPos, Color.green);
				}				
				float angle = Vector3.Angle(normal2Check, normalFromPos);
				// create only proper sides
				// calculate sides by element position
				float dots = Vector3.Dot(normalFromPos, normal2Check);
				if (angle < 90 && dots > 0)  {
					Vector3 sidePos = cubikCenter + normal2Check;
					
					Quaternion quat = Quaternion.FromToRotation(PrefabNormal, normal2Check);
					//Quaternion quat = Quaternion.identity;
					GameObject go = (GameObject)Instantiate(prefab, sidePos, quat);
					go.name = name;
					go.GetComponent<MeshRenderer>().material.color = color;
					res.Add(go);
				}
				++colorSubIndex;
			}
		}

		GameObject[] res2 = new GameObject[res.Count];
		int l = 0;
		foreach(GameObject go in res) {
			res2[l] = go;
			++l;
		};

		GameObject holderObject = new GameObject();
		holderObject.transform.position = cubikCenter;
		TRubikCubik rc = holderObject.AddComponent<TRubikCubik>();
		rc.Init(holderObject, res2);
		
		return rc;
	}
		
	public static TRubik2 CreateRubik(Vector3 center, float elementSize, GameObject prefab) {
		int centerIndex = (Dimension - 1) / 2;
		float startShift = elementSize * (((float)Dimension - 1) / 2);
		Vector3 startPos = center - new Vector3(startShift, startShift, startShift);
		TRubik2 res = new TRubik2();
		
		int nameIndex = 0;
		for (int i = 0; i < Dimension; ++i) {
			//int j = 0;
			for (int j = 0; j < Dimension; ++j) 
			{
				//int k = 0;
				for (int k = 0; k < Dimension; ++k) 
				{
					string name = nameIndex.ToString();
					nameIndex += 1;
					// skip center cube
					if (i == centerIndex && j == centerIndex && k == centerIndex) {
						// add fake element
						res.GOBS.Add(new GameObject());
						continue;
					}
					Vector3 smallCubCenter = startPos + new Vector3(elementSize * i, elementSize * j, elementSize * k);
					TRubikCubik smallCubik = CreateSmallCubik(smallCubCenter, center, prefab, elementSize, ref name);
					res.Elements.Add(smallCubik);
					res.GOBS.Add(smallCubik.HolderObject);
					//ProcessSides(smallCubCenter,
					//break;
					
				}
			}
		}
		
		// rotation planes
		for (int i = 0; i < Dimension; ++i) {
			//int j = 0;
			TRotationPlane xPlane = new TRotationPlane();
			TRotationPlane yPlane = new TRotationPlane();
			TRotationPlane zPlane = new TRotationPlane();
			for (int j = 0; j < Dimension; ++j) 
			{
				//int k = 0;
				for (int k = 0; k < Dimension; ++k) 
				{
					if (i == centerIndex && j == centerIndex && k == centerIndex)
						continue;
					
					TRubikCubik cubik = res.GOBS[i * Dimension * Dimension + j * Dimension + k].GetComponent<TRubikCubik>();
					xPlane.Elements.Add(cubik);
					cubik = res.GOBS[j * Dimension * Dimension + i * Dimension + k].GetComponent<TRubikCubik>();
					yPlane.Elements.Add(cubik);
					cubik = res.GOBS[k * Dimension * Dimension + i * Dimension + j].GetComponent<TRubikCubik>();
					zPlane.Elements.Add(cubik);
				}
			}
			res.XPlanes.Add(xPlane);
			res.YPlanes.Add(yPlane);
			res.ZPlanes.Add(zPlane);
			
		}
		
		return res;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
