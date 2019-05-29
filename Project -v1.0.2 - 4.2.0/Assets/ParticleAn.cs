using UnityEngine;
using System.Collections;

public class ParticleAn : animate {

	private ParticleSystem ps;
	private bool change;

	private int emmisionRate;
	public void Start() 
	{
		ps = GetComponent<ParticleSystem>();
		change = active;
		emmisionRate = ps.main.maxParticles;
		ParticleSystem.MainModule myModule = ps.main;
		myModule.maxParticles = 0;
        ParticleSystem.MainModule m= ps.main;
        m.maxParticles = 0;

	}

	// Update is called once per frame
	void Update () {
		if (change != active) {
			change = active;

			if (!change) {

                ParticleSystem.MainModule ma = ps.main;
                ma.maxParticles = 0;
            } else {
                ParticleSystem.MainModule mb = ps.main;
                mb.maxParticles = emmisionRate;
 
			}
            ParticleSystem.MainModule m = ps.main;
            m.maxParticles = 0;
            m.startLifetime = ps.main.startLifetime.constant;
           // ps.startLifetime = ps.main.startLifetime.constant;

		}
	
	}
}
