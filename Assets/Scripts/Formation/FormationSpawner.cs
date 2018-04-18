using UnityEngine;
using System.Collections;

public class FormationSpawner : MonoBehaviour {
	
	public GameObject formation1;
	public GameObject formation2;
	public GameObject formation3;
	public GameObject formation4Left;
	public GameObject formation4Right;
	public GameObject bossFormation;
	public int waves;
	public int wavesLeft;
	public int wavesRight;
	
	private GameObject firstWave;
	private GameObject secondWave;
	private GameObject thirdWave;
	private GameObject fourthWaveLeft;
	private GameObject fourthWaveRight;
	private GameObject boss;
	private int userInputWaves;
	private bool bossLevel;
	
	private LevelManager levelManager;
	
	// Use this for initialization
	void Start () {
		firstWave = Instantiate(formation1) as GameObject;
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		userInputWaves = waves;
		bossLevel = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (waves <= 0){
			if (firstWave){
				DestroyObject(firstWave);
				secondWave = Instantiate(formation2) as GameObject;
				ResetWaves();
			}
			else if (secondWave){
				DestroyObject(secondWave);
				thirdWave = Instantiate(formation3) as GameObject;
				ResetWaves();
			}
			else if (thirdWave){
				DestroyObject(thirdWave);
				fourthWaveLeft = Instantiate(formation4Left) as GameObject;
				fourthWaveRight = Instantiate(formation4Right) as GameObject;
				ResetWaves();
			}
		}
		if (wavesLeft <= 0)		{DestroyObject(fourthWaveLeft);}
		if (wavesRight <= 0)	{DestroyObject(fourthWaveRight);}
		
		if (wavesLeft <= 0 && wavesRight <= 0 && !fourthWaveLeft && !fourthWaveRight && transform.childCount <= 0 && bossLevel == false){
			boss = Instantiate (bossFormation) as GameObject;
			boss.transform.parent = gameObject.transform;
			bossLevel = true;
		}
		if (bossLevel == true && transform.childCount <= 0){			
			levelManager.LoadLevel("Win");
		}			
	}
	
	void ResetWaves(){
		waves = userInputWaves;
		wavesLeft = userInputWaves;
		wavesRight = userInputWaves;
	}
}
