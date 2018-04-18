using UnityEngine;
using System.Collections;

public class FormationControllerParent : MonoBehaviour {

	public GameObject enemyPrefab;
	public float width;
	public float height;
	public float speed;
	public float xMinPadding;
	public float xMaxPadding;
	public float yMinPadding;
	public float yMaxPadding;
	public float spawnDelay;
	
	protected float distance;
	protected float xMin;
	protected float xMax;
	protected float yMin;
	protected float yMax;
	protected bool moveRight;
	protected bool moveLeft;
	protected bool moveDown;
	protected bool moveUp;
	
	protected FormationSpawner formationSpawner;
	
	// Use this for initialization
	void Start () {
	
		formationSpawner = GameObject.FindObjectOfType<FormationSpawner>();
		
		SpawnEnemies();
		
		moveRight = false;
		moveLeft = false;
		moveUp = false;
		moveDown = false;
		
		distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		Vector3 downMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 upMost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		
		xMin = leftMost.x + xMinPadding;
		xMax = rightMost.x - xMaxPadding;
		yMin = downMost.y + yMinPadding;
		yMax = upMost.y - yMaxPadding;
		
	}		

	protected Transform NextFreePosition(){
		foreach(Transform gameObjectsInPosition in transform){
			if (gameObjectsInPosition.childCount <= 0){
				return gameObjectsInPosition;
			}
		}
		return null;
	}
	
	protected bool AllMembersDead(){
		foreach(Transform gameObjectsInPosition in transform){
			if (gameObjectsInPosition.childCount > 0){
				return false;
			}
		}	
		return true;	 
	}
	
	protected void SpawnEnemies(){		
		foreach(Transform positionName in transform){
			GameObject enemy = Instantiate(enemyPrefab, positionName.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = positionName;		
		}
	}
	
	protected void SpawnUntilFull(){
		Transform freePosition = NextFreePosition();
		if (freePosition){
			GameObject enemy = Instantiate(enemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition;
			Invoke("SpawnUntilFull", spawnDelay);
		}	
	}
	
	protected void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
	}
}
