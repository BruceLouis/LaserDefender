using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float padding;
	public float projectileSpeed;
	public float firingRate;
	public float hitPoints;
	public GameObject projectile;
	public AudioClip fireSound;
	public AudioClip deathSound;
	
	public enum fireLevels {one, two, three};
	public fireLevels fireLevel;
	
	private LevelManager levelManager;
	private HPBar hpBar;
	private float distance;
	private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;
	private float maxHitPoints;
	private float numDamageFlames;
	
	// Use this for initialization
	void Start () {
		maxHitPoints = hitPoints;
		
		distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		Vector3 downMost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 upMost = Camera.main.ViewportToWorldPoint(new Vector3(0,1,distance));
		
		fireLevel = fireLevels.one;
		
		xMin = leftMost.x + padding;
		xMax = rightMost.x - padding;
		yMin = downMost.y + padding;
		yMax = upMost.y - padding;
		
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		hpBar = GameObject.FindObjectOfType<HPBar>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		hpBar.SetHP(hitPoints);
		
		if (Input.GetKey(KeyCode.LeftArrow)){
			transform.position += Vector3.left * speed * Time.deltaTime;	
		}		
		if (Input.GetKey(KeyCode.RightArrow)){			
			transform.position += Vector3.right * speed * Time.deltaTime;	
		}		
		if (Input.GetKey(KeyCode.UpArrow)){
			transform.position += Vector3.up * speed * Time.deltaTime;
		}	
		if (Input.GetKey(KeyCode.DownArrow)){ 
			transform.position += Vector3.down * speed * Time.deltaTime;
		}	
		if (Input.GetKeyDown(KeyCode.Space)){
			InvokeRepeating("Fire", 0.0001f, firingRate);
		}
		if (Input.GetKeyUp(KeyCode.Space)){
			CancelInvoke("Fire");
		}
		
		float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);		
		
		float newY = Mathf.Clamp(transform.position.y, yMin, yMax);
		transform.position = new Vector3(transform.position.x, newY, transform.position.z);	
	}
	
	void Fire(){
		if (fireLevel == fireLevels.one){
			GameObject laser  = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;			
			laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
		}
		else if (fireLevel == fireLevels.two){
			Vector3 doubleShotOffset = new Vector3(0.5f,0f,0f);
			GameObject laserLeft = Instantiate(projectile, transform.position - doubleShotOffset, Quaternion.identity) as GameObject;			
			laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
			GameObject laserRight = Instantiate(projectile, transform.position + doubleShotOffset, Quaternion.identity) as GameObject;			
			laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
		}
		else if (fireLevel == fireLevels.three){
			GameObject laserLeft = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;			
			laserLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.5f, projectileSpeed);
			GameObject laserCenter = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;			
			laserCenter.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, projectileSpeed);
			GameObject laserRight = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;			
			laserRight.GetComponent<Rigidbody2D>().velocity = new Vector2(1.5f, projectileSpeed);
		}
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}	

	void PlayerDead(){		
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		Destroy(gameObject);
		levelManager.LoadLevel("Lose");
	}
		
	public void HPRecovery(float percentageToRecover){
		float hitPointsRecovery = percentageToRecover * GetMaxHP();
		hitPoints += hitPointsRecovery;
		if (hitPoints >= GetMaxHP()){
			hitPoints = GetMaxHP();
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile){
			hitPoints -= missile.GetDamage();
			hpBar.SetHP(hitPoints);
			missile.Hit();
			if (hitPoints <= 0){
				PlayerDead();
			}
		}
	}
	
	public float GetMaxHP(){
		return maxHitPoints;
	}
}
	