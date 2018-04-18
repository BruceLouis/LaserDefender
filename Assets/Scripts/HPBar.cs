using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBar : MonoBehaviour {

	private Slider slider;
	private float maxHP;
	private float sliderValue;
	private bool settingMaxHP;	

	// Use this for initialization
	void Start () {
		slider = GameObject.FindObjectOfType<Slider>();
		settingMaxHP = true;
	}
	
	public void SetHP(float currentHP){
		if (settingMaxHP){
			maxHP = currentHP;
			settingMaxHP = false;
		}
		sliderValue = currentHP/maxHP;
		slider.value = sliderValue;
	}
}
