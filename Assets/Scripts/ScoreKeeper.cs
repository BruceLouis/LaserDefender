using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {

	public static float score;
	
	private Text text;
	
	// Use this for initialization
	void Start () {	
		Reset();
		text = GetComponent<Text>();
		text.text = ("Score: " + score);	
	}
	
	// Update is called once per frame
	void Update () {	
		text.text = ("Score: " + score);
	}
	
	public void ScorePoints (float points){
		score += points;
	}
	
	public static void Reset() {
		score = 0f;
	}
}
