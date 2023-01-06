using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWrapper : MonoBehaviour {


	public string text;


	void Start () {
		TMP_Text textField = GetComponent<TMP_Text> ();
		textField.text = HindiCorrector.Correct(text);
	}


}
