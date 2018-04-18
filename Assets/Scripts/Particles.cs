using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour {

	void Start (){
		// Set the sorting layer of the particle system.
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = "Explosion";
	}	
}