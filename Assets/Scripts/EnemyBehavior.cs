using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public float hitPoints;
	public float projectileSpeed;	
	public float rateOfFire;	
	public float scoreValue;
	public GameObject projectile;
	public GameObject explosion;
	public GameObject powerUp;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	private ScoreKeeper score;

	void Start(){
		score = GameObject.FindObjectOfType<ScoreKeeper>();
	}
	
	void Update(){		
		float frequency = Time.deltaTime * rateOfFire;
		if (frequency > Random.value){
			Fire();
		}
	}

	void Fire(){
		GameObject enemyLaser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
		enemyLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -projectileSpeed);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);

	}
	
	void EnemyDestroyed(){
		int randNum = Random.Range(0,20);
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		if (randNum <= 0){
			GameObject drop = Instantiate(powerUp, transform.position, Quaternion.identity) as GameObject;
			drop.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -2f);
		}
		Destroy(gameObject);
		Destroy(explode,1f);
		score.ScorePoints(scoreValue);
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile){
			hitPoints -= missile.GetDamage();
			missile.Hit();
			if (hitPoints <= 0){	
				EnemyDestroyed();			
			}
		}
	}
	
}
