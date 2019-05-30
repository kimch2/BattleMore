using UnityEngine;
using System.Collections;

public class GetColor_Particle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem ps = this.gameObject.GetComponent<ParticleSystem>();

        ParticleSystem.MainModule ma = ps.main;

		ma.startColor = this.transform.root.gameObject.GetComponent<BeamParam>().BeamColor;
		ma.startSize = ma.startSize.constant * this.transform.root.gameObject.GetComponent<BeamParam>().Scale;
	}
	
}
