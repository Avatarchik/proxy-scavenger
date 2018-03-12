using UnityEngine;
using System.Collections;

public class HalcyonLaser : MonoBehaviour {

	protected virtual void OnEnable()

	{
		if (m_Player != null)

			m_Player.Register(this);
	}



	protected virtual void OnDisable()

	{

		if (m_Player != null)

			m_Player.Unregister(this);

	}


	vp_FPPlayerEventHandler m_Player;

	void Awake()

	{

		m_Player = transform.GetComponent<vp_FPPlayerEventHandler>();

	}

	//Caching the Laser Prefab via inspector
	public GameObject RedLaserInverted;

	//Activate/Enable the Laser Effect Prefab pre-placed on a Laser Gun, when attack event starts. 
	void OnStart_Attack()
	{
		/*
		Start(); //I want to force the event to start, because "OnStart_Attack()" event requires ammo to start. The Laser doesn't use ammo. 
		{
			RedLaserInverted.SetActive(true); 
		}
		*/
	}

	void OnStop_Attack()
	{
		/*
		Stop(); //"Stop" isn't recognized as a method. I am using it wrong. 
		{
			RedLaserInverted.SetActive(false);
		}
		*/
	} 
}