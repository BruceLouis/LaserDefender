using UnityEngine;
using System.Collections;

public class FormationController2 : FormationControllerParent {
	
	// Update is called once per frame
	void Update () {
	
		if (transform.position.x < xMax && moveLeft == false){	
			transform.position += Vector3.right * speed * Time.deltaTime;
			moveRight = true;
		}
		else if (transform.position.x >= xMax && moveRight == true){
			moveRight = false;
		} 
		else if (transform.position.x > xMin && moveRight == false){
			transform.position += Vector3.left * speed * Time.deltaTime;
			moveLeft = true;
		}
		else if (transform.position.x <= xMin && moveLeft == true){
			moveLeft = false;
		} 
		
		
		if (transform.position.y < yMax && moveDown == false){	
			transform.position += Vector3.up * speed * Time.deltaTime;
			moveUp = true;
		}
		else if (transform.position.y >= yMax && moveUp == true){
			moveUp = false;
		} 
		else if (transform.position.y > yMin && moveUp == false){
			transform.position += Vector3.down * speed * Time.deltaTime;
			moveDown = true;
		}
		else if (transform.position.y <= yMin && moveDown == true){
			moveDown = false;
		} 
		
		
		Debug.Log ("AllMembersDead() : " + AllMembersDead());
		Debug.Log ("waves " + formationSpawner.waves);
		if (AllMembersDead() && formationSpawner.waves > 0){
			Debug.Log ("Empty formation");
			SpawnUntilFull();	
			formationSpawner.waves--;
		}
	}
}
