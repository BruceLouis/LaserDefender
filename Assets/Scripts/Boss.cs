using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {

	public float hitPoints;
	
	public float twoShotProjectileSpeed;	
	public float twoShotSpeed;	
	
	public float fiveShotProjectileSpeed;	
	public float meteorShotsSpeed;	
	
	public float laserProjectileSpeed;
	public float laserSpeed;
	
	public float scoreValue;
	public float xPadding;
	public float yMinPadding;
	public float yMaxPadding;
	
	
	public AudioClip fireSound;
	public AudioClip meteorSound;
	public AudioClip laserSound;
	public AudioClip deathSound;
	
	public int patternTimer;
	public int explosionTimer;
	public int twoShotInterval;	
	public int fiveShotInterval;
	public int meteorShotInterval;		
	
	public GameObject twoShotProjectile;
	public GameObject fiveShotProjectile;
	public GameObject bigLaser;
	public GameObject meteorShots;
	public GameObject smallExplosion;
	public GameObject bigExplosion;
	
	//////////////////////////////
	private ScoreKeeper score;
	
	private enum Patterns {twoShotFire, laser, meteorShots, fiveShotFire, fiveShotMeteorCombo, transition, destroyed};
	private Patterns bossPattern;	
	
	private bool moveRight;
	private bool moveLeft;
	private bool moveDown;
	private bool moveUp;	
	
	private float distance;
	private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;
	
	private int twoShotFrequency;
	private int fiveShotFrequency;
	private int meteorShotFrequency;
	private int userInputPatternTimer;	
	private int randPattern;
	private int randExplosion;
	private int laserSoundFrequency;
	private float bossHalfHP;
	private float bossQuarterHP;
	
	private Vector3 originalPosition; 

	// Use this for initialization
	void Start(){
		
		originalPosition = transform.position;
		bossHalfHP = hitPoints/2;
		bossQuarterHP = hitPoints/4;
		laserSoundFrequency = 0;
		
		score = GameObject.FindObjectOfType<ScoreKeeper>();	
		bossPattern = Patterns.twoShotFire;
//		bossPattern = Patterns.laser;
//		bossPattern = Patterns.fiveShotFire;
//		bossPattern = Patterns.meteorShots;
		
		distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		Vector3 downMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 upMost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		
		xMin = leftMost.x + xPadding;
		xMax = rightMost.x - xPadding;
		yMin = downMost.y + yMinPadding;
		yMax = upMost.y - yMaxPadding;
		
		twoShotFrequency = twoShotInterval;
		fiveShotFrequency = fiveShotInterval;
		meteorShotFrequency = meteorShotInterval;
		userInputPatternTimer = patternTimer;
	}
	
	// Update is called once per frame
	void Update(){
		
		randExplosion = Random.Range(0,15);	
		
		if (bossPattern != Patterns.destroyed){	
			if (hitPoints > bossHalfHP){
				if (patternTimer <= 0){
					BossRepositionItself();
					if (transform.position == originalPosition){			
						randPattern = Random.Range(0,3);
						if (randPattern == 0){
							bossPattern = Patterns.twoShotFire;
							ResetPatternTimer();
							patternTimer *= 2;
						}
						else if (randPattern == 1){
							bossPattern = Patterns.laser;
							ResetPatternTimer();
						}
						else{
							bossPattern = Patterns.meteorShots;
							ResetPatternTimer();
						}
					}
				}
			}
			else if (hitPoints > bossQuarterHP){
				BossRepositionItself();
				if (transform.position == originalPosition){
					bossPattern = Patterns.fiveShotFire;
				}
			}
			else {
				BossRepositionItself();
				if (transform.position == originalPosition){
					bossPattern = Patterns.fiveShotMeteorCombo;
				}
			}				
				 	 
			if (bossPattern == Patterns.twoShotFire){
				if (twoShotFrequency <= 0){
					TwoShotFire();
					twoShotFrequency = twoShotInterval;
				}	
				if (transform.position.x < xMax && moveLeft == false){	
					transform.position += Vector3.right * twoShotSpeed * Time.deltaTime;
					moveRight = true;
				}
				else if (transform.position.x >= xMax && moveRight == true){
					moveRight = false;
				} 
				else if (transform.position.x > xMin && moveRight == false){
					transform.position += Vector3.left * twoShotSpeed * Time.deltaTime;
					moveLeft = true;
				}
				else if (transform.position.x <= xMin && moveLeft == true){
					moveLeft = false;
				} 
				twoShotFrequency--;
				patternTimer--;
			}
			else if (bossPattern == Patterns.laser){			
				LaserFire();
				if (laserSoundFrequency >= 60){					
					AudioSource.PlayClipAtPoint(laserSound, transform.position);	
					laserSoundFrequency = 0;
				}
				if (transform.position.x < xMax && moveLeft == false){	
					transform.position += Vector3.right * laserSpeed * Time.deltaTime;
					moveRight = true;
				}
				else if (transform.position.x >= xMax && moveRight == true){
					moveRight = false;
				} 
				else if (transform.position.x > xMin && moveRight == false){
					transform.position += Vector3.left * laserSpeed * Time.deltaTime;
					moveLeft = true;
				}
				else if (transform.position.x <= xMin && moveLeft == true){
					moveLeft = false;
				} 
				patternTimer--;
				laserSoundFrequency++;
				
			}
			else if (bossPattern == Patterns.fiveShotFire){
				if (fiveShotFrequency <= 0){
					FiveShotFire();
					fiveShotFrequency = fiveShotInterval;
				}	
				fiveShotFrequency--;
				
			}		
			else if (bossPattern == Patterns.meteorShots){
				if (meteorShotFrequency <= 0){
					MeteorShotFire();
					meteorShotFrequency = meteorShotInterval;
				}	
				meteorShotFrequency--;
				patternTimer--;			
			}				
			else if (bossPattern == Patterns.fiveShotMeteorCombo){
				if (fiveShotFrequency <= 0){
					FiveShotFire();
					fiveShotFrequency = fiveShotInterval;
				}	
				if (meteorShotFrequency <= 0){
					MeteorShotFire();
					meteorShotFrequency = meteorShotInterval;
				}	
				fiveShotFrequency--;
				meteorShotFrequency--;
			}	
		}
		else{
			Vector3 particleOffset = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f),0f);
			if (randExplosion == 0){
				GameObject smallExplode = Instantiate(smallExplosion, transform.position += particleOffset,Quaternion.identity) as GameObject;
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				Destroy(smallExplode,1f);
			}
			else if (randExplosion == 1){
				GameObject bigExplode = Instantiate(bigExplosion, transform.position += particleOffset,Quaternion.identity) as GameObject;
				AudioSource.PlayClipAtPoint(deathSound, transform.position);
				Destroy(bigExplode,1f);
			}
			explosionTimer--;
			if (explosionTimer <= 0){
				Destroy(gameObject);
			}
		}
	}
	
	void TwoShotFire(){
		GameObject bossLaserRight = Instantiate(twoShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(2f, -twoShotProjectileSpeed);
		
		GameObject bossLaserLeft = Instantiate(twoShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-2f, -twoShotProjectileSpeed);
		
		AudioSource.PlayClipAtPoint(fireSound, transform.position);		
	}
	
	void LaserFire(){
		GameObject bossBigLaser = Instantiate(bigLaser, transform.position, Quaternion.identity) as GameObject;
		bossBigLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -laserProjectileSpeed);
			
	}
	
	void FiveShotFire(){
		GameObject bossLaserRightRight = Instantiate(fiveShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserRightRight.GetComponent<Rigidbody2D>().velocity = new Vector2(5f, -fiveShotProjectileSpeed);
		
		GameObject bossLaserRight = Instantiate(fiveShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(2.5f, -fiveShotProjectileSpeed);
		
		GameObject bossLaserCenter = Instantiate(fiveShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserCenter.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -fiveShotProjectileSpeed);
		
		GameObject bossLaserLeft = Instantiate(fiveShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-2.5f, -fiveShotProjectileSpeed);
		
		GameObject bossLaserLeftLeft = Instantiate(fiveShotProjectile, transform.position, Quaternion.identity) as GameObject;
		bossLaserLeftLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, -fiveShotProjectileSpeed);
		
		AudioSource.PlayClipAtPoint(fireSound, transform.position);		
	}
	
	void MeteorShotFire(){
		Vector3 spawnOffset = new Vector3(Random.Range(2f,4f),0f,0f);
		
		GameObject meteorShotsRight = Instantiate(meteorShots, transform.position + spawnOffset, Quaternion.identity) as GameObject;
		meteorShotsRight.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2f,2f), -meteorShotsSpeed);
		
		GameObject meteorShotsLeft = Instantiate(meteorShots, transform.position - spawnOffset, Quaternion.identity) as GameObject;
		meteorShotsLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-2f,2f), -meteorShotsSpeed);
		
		AudioSource.PlayClipAtPoint(meteorSound, transform.position);		
	}
	
	void FiveShotMeteorComboFire(){
		MeteorShotFire();		
		FiveShotFire();	
	}
	void ResetPatternTimer(){
		patternTimer = userInputPatternTimer;
	}
	
	void BossRepositionItself(){
		transform.position = Vector3.MoveTowards(transform.position, originalPosition, 5f * Time.deltaTime);
		bossPattern = Patterns.transition;
	}
	
	void BossDestroyed(){
		bossPattern = Patterns.destroyed;
		score.ScorePoints(scoreValue);
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile){
			hitPoints -= missile.GetDamage();
			missile.Hit();
			if (hitPoints <= 0){	
				BossDestroyed();			
			}

		}
	}
}
