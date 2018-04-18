using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float damage;
	public GameObject explosion;
		
	public void Hit(){
		GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		explode.GetComponent<ParticleSystem>().startColor = gameObject.GetComponent<SpriteRenderer>().color;
		Destroy(gameObject);		
		Destroy(explode,1f);
	}
	
	public float GetDamage(){
		return damage;
	}
}
