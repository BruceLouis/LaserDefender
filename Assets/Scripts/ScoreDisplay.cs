﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {
	
	private Text text;
	
	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		text.text = ("Score: " + ScoreKeeper.score);	
		ScoreKeeper.Reset();
	}
}
