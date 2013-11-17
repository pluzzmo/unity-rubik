using UnityEngine;
using System.Collections;

public class TMainController : MonoBehaviour {
	
	//public ScriptableObject[] RubikElements;
	public Color[] Colors;
	public GameObject[] RubikElements;
	public GameObject Prefab;
	//public TRubikCreator.TRubik Rubik;
	public TRubik2 Rubik;
	// Use this for initialization
	void Start () {
		//RubikElements = TRubikCreator.CreateRubik(new Vector3(0,0,0), 3, ref Prefab, ref Colors);
		//RubikElements = TRubikCreator.CreateRubik2(new Vector3(0,0,0), 3, ref Prefab);//, ref Colors);
		//RubikElements = new TRubikElement[9];
		//TRubikElement re = (TRubikElement)ScriptableObject.CreateInstance("TRubikElement");
		//re.Init(Prefab, new Vector3(0, 0, 0));
		//RubikElements[0] = re;
		Vector3 center = new Vector3(0,0,0);
		float elementSize = 1.0f;
		//Rubik = TRubikCreator.CreateRubik(center, elementSize, Prefab);
		Rubik = TRubikCreator2.CreateRubik(center, elementSize, Prefab);
	}
	
	private TRubikCubik SelectedCubic;
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 20)) {
				GameObject selectedObj = hit.collider.gameObject;
				/*
				TRubikElement2 elem = selectedObj.transform.parent.GetComponent<TRubikElement2>();
				elem.Select();
				//TRubikCreator.TRubik.TRubikElement rubikElement = selectedObj.transform.parent.GetComponent("TRubikElement") as TRubikCreator.TRubik.TRubikElement;
				//rubikElement.Select();
				Debug.Log(hit.ToString());
				*/
				TRubikCubik  cubik = selectedObj.transform.parent.GetComponent<TRubikCubik>();
				if (SelectedCubic != null)
					Rubik.SelectAvailablePlanes(ref SelectedCubic, false);
				SelectedCubic = cubik;
				Rubik.SelectAvailablePlanes(ref cubik, true);
			}
		}
	}
}
