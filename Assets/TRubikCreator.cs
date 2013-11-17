using UnityEngine;
using System.Collections;

public class TRubikElement2 : MonoBehaviour {
	GameObject[] Sides;
	
	public TRubikElement2() {
		//Debug.Log("TFakeClass");
	}
	void Start () {
		Debug.Log("TFakeClass");
	}
	
	void Update () {
	}
	public void Init(int[] idxs, GameObject[] sides, Vector3 pos, GameObject parent) {
		Sides = new GameObject[idxs.Length];
		int i = 0;
		foreach(int v in idxs) {
			Sides[i] = sides[v];
			//Sides[i].transform.parent = this.transform.parent;
			Sides[i].transform.parent = parent.transform;
			i++;
		}
	}
	public void Select() {
		foreach (GameObject go in Sides) {
			go.GetComponent<MeshRenderer>().material.color = Color.black;
		}
	}
};

public class TRubikCreator : MonoBehaviour {
	private static int Dimension = 3;
	
	public class TRubik {
		public class TRubikElement1 : MonoBehaviour {
			
			
			public TRubikElement1() {
			}

			void Start () {
				Debug.Log("TFakeClass");
			}
			
			void Update () {
			}
			

			
			static public TRubikElement2 CreateRubikElement(int[] idxs, GameObject[] sides, Vector3 pos) {
				GameObject go = new GameObject();
				go.transform.position = pos;
				//TRubikElement element = go.AddComponent<TRubikElement>();
				TRubikElement2 rb = go.AddComponent<TRubikElement2>();
				rb.Init(idxs, sides, pos, go);
				//rb.Init(
				//TFakeClass fk = go.AddComponent<TFakeClass>();
				//Rigidbody rb = go.AddComponent<Rigidbody>();
				//element = new TRubikElement(idxs, sides, pos, go);
				//TRubikElement element2 = go.GetComponent<TRubikElement>() as TRubikElement;
				return rb;
				//return new TRubikElement2();
			}
			

		};
		
		TRubikElement2[] Elements;
		
		public TRubik(GameObject[] sides, Vector3[] elementsPositions) {
			
			Elements = new TRubikElement2[Dimension * Dimension * Dimension];
			
			int[][] indexes = {
				new int[] {0, 51, 29},
				new int[] {1, 32},
				new int[] {2, 42, 35},
				new int[] {3, 52},
				new int[] {4},
				new int[] {5, 43},
				new int[] {6, 53, 20},
				new int[] {7, 23},
				new int[] {8, 26, 44},
				new int[] {9, 45, 27},
				new int[] {10, 30},
				new int[] {11, 36, 33},
				new int[] {12, 46},
				new int[] {13},
				new int[] {14, 37},
				new int[] {15, 47, 18},
				new int[] {16, 21},
				new int[] {17, 38, 24},
				new int[] {19, 50},
				new int[] {22},
				new int[] {25, 41},
				new int[] {28, 48},
				new int[] {31},
				new int[] {34, 39},
				new int[] {49},
				new int[] {40},
			};
			int idx = 0;
			for (int i = 0; i < Dimension; ++i) {
				for (int j = 0; j < Dimension; ++j) {
					for (int k = 0; k < Dimension; ++k) {
						// skip center cube
						if (i == 0 && j == 0 && k ==0) {
							continue;
						}
						Vector3 pos = elementsPositions[i * Dimension * Dimension + j * Dimension + k];
						Elements[idx] = TRubikElement1.CreateRubikElement(indexes[idx], sides, pos);
						idx++;
					}
				}
			}
		}
	};
	
	public static TRubik CreateRubik(Vector3 center, float elementSize, GameObject prefab) 
	{
		
		Color[] colors = new Color[6];
		colors[0] = Color.red;
		colors[1] = Color.yellow;
		colors[2] = Color.green;
		colors[3] = Color.white;
		colors[4] = Color.blue;
		colors[5] = new Color(0.1f, 0.1f, 0.1f);
		
		
		GameObject[] res = new GameObject[Dimension * Dimension * 6];
		int startIndex = 0;
		int[] eulerIndexes = {2, 2, 0};
		float[] eulerValues = {270, 0, 90};
		
		for (int i = 0; i < Dimension; ++i) {
			Vector3 eulers = Vector3.zero;
			int eulerIndex = eulerIndexes[i];
			eulers[eulerIndex] = eulerValues[i];

			Quaternion quat = Quaternion.Euler(eulers);
			
			float faceHalfLength = ((Dimension) * elementSize ) / 2;
			Vector3 delta = Vector3.zero;
			delta[i] = faceHalfLength;
			Vector3 faceCenter = center + delta;

			Color color = colors[i * 2];
			int xIndex = (i + 1) % 3;
			int yIndex = (i + 2) % 3;

			GameObject[] side = CreateSide(faceCenter, elementSize, color, ref prefab, xIndex, yIndex, quat);
			
			foreach (GameObject g in side) {
				g.name = string.Format("face_{0}", startIndex);
				res[startIndex++] = g;
				
			}
			
			delta[i] = -delta[i];
			faceCenter = center + delta;
			color = colors[i * 2 + 1];
			eulers[eulerIndex] = eulers[eulerIndex] + 180;
			quat.eulerAngles = eulers;
			side = CreateSide(faceCenter, elementSize, color, ref prefab, xIndex, yIndex, quat);

			foreach (GameObject g in side) {
				g.name = string.Format("face_{0}", startIndex);
				res[startIndex++] = g;
			}
		}
		
		
		Vector3[] elementsPositions = new Vector3[Dimension * Dimension * Dimension];
		{
		float delta = (((float)Dimension - 1) / 2) * elementSize;
		Vector3 beginPos = center - new Vector3(delta, delta, delta);
		for (int i = 0; i < Dimension; ++i) {
			for (int j = 0; j < Dimension; ++j) {
				for (int k = 0; k < Dimension; ++k) {
					elementsPositions[i * Dimension * Dimension + j * Dimension + k] = beginPos	+ new Vector3(i * elementSize, j * elementSize, k * elementSize);
				}
			}
		}
		}
		return new TRubik(res, elementsPositions);
	}
	
	public static GameObject[] CreateSide(
		Vector3 faceCenter,
		float elementSize,
		Color color,
		ref GameObject prefabElement,
		int xIndex, int yIndex,
		Quaternion quat
	) {
		GameObject[] res = new GameObject[Dimension * Dimension];
		float halfSideSize = (elementSize * (Dimension - 1)) / 2;
		Vector3 firstPoint = faceCenter;
		firstPoint[xIndex] -= halfSideSize;
		firstPoint[yIndex] -= halfSideSize;
		
		for (int i = 0; i < Dimension; ++i)
			for (int j = 0; j < Dimension; ++j) {
				Vector3 delta = new Vector3(0, 0, 0);
				delta[(int)xIndex] = i * elementSize;
				delta[(int)yIndex] = j * elementSize;
				Vector3 pos = firstPoint + delta;
				GameObject go = (GameObject)Instantiate(prefabElement, pos, quat);
				go.GetComponent<MeshRenderer>().material.color = color;
				int idx = i * Dimension + j;
				res[idx] = go;
		}
		return res;
	}
}
