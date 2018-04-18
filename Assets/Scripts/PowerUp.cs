using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public float hpPercentage;
	public Sprite[] powerUpSprites;
	public GameObject shieldPowerUp;
	private PlayerController playerController;
	private HPBar hpBar;
	private int randomPowerUp;
	
	void Start () {		
		randomPowerUp = Random.Range(0, powerUpSprites.Length);
//		randomPowerUp = powerUpSprites.Length-1;
//		randomPowerUp = 0;
		gameObject.GetComponent<SpriteRenderer>().sprite = powerUpSprites[randomPowerUp];
		playerController = GameObject.FindObjectOfType<PlayerController>();
		hpBar = GameObject.FindObjectOfType<HPBar>();
	}

	void OnTriggerEnter2D(Collider2D collider){	
		if (collider.gameObject.GetComponent<PlayerController>()){
			DeterminePowerUp();
			Destroy(gameObject);
		}
	}
	
	
	void DeterminePowerUp(){
		if (randomPowerUp == 0){			
			playerController.HPRecovery(.1f);
		}
		else if (randomPowerUp == 1){
			if (!playerController.GetComponentInChildren<Shield>()){
				GameObject shield = Instantiate(shieldPowerUp, playerController.transform.position, Quaternion.identity) as GameObject;
				shield.transform.parent = playerController.transform;
			}
			else{			
				Shield existingShield = playerController.GetComponentInChildren<Shield>();
				Debug.Log(existingShield);
				existingShield.ResetShieldTimer();
			}
		}
		else{
			if (playerController.fireLevel == PlayerController.fireLevels.one){
				playerController.fireLevel = PlayerController.fireLevels.two;
			}
			else if (playerController.fireLevel == PlayerController.fireLevels.two){
				playerController.fireLevel = PlayerController.fireLevels.three;
			}
		}
	}
}
