using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {

	public int shieldTimer;
	private int maxShieldTimer;
	
	void Start(){
		maxShieldTimer = shieldTimer;
	}

	// Update is called once per frame
	void Update(){
		Debug.Log("shieldTimer " + shieldTimer);		
		shieldTimer--;
		if (shieldTimer <= 0){
			Destroy(gameObject);
		}	
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if(missile){
			missile.Hit();
		}
	}
	
	public void ResetShieldTimer(){		
		shieldTimer = maxShieldTimer;
	}
}
