using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClikURL : MonoBehaviour {

	public string url;

	void OnMouseDown()
	{
		Application.OpenURL(url);
	}
}
