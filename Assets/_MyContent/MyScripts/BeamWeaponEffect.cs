using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamWeaponEffect : MonoBehaviour {

	protected float m_FadeSpeed = 0.075f;				// amount of alpha to be deducted each frame
	protected bool m_ForceShow = false;					// used to set the muzzleflash 'always on' in the editor

	public float FadeSpeed { get { return m_FadeSpeed; } set { m_FadeSpeed = value; } }

	[Header("Prefabs")]
	public GameObject[] beamLineRendererPrefab;
	public GameObject[] beamStartPrefab;
	public GameObject[] beamEndPrefab;

	private GameObject beamStart;
	private GameObject beamEnd;
	private GameObject beam;
	private LineRenderer line;

	[Header("Adjustable Variables")]
	public float beamEndOffset = 1f; //How far from the raycast hit point the end effect is positioned
	public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
	public float textureLengthScale = 3; //Length of the beam texture

	public vp_FPPlayerEventHandler m_Player;

	void Start()
	{

		// if a weapon camera is used, put muzzleflash in the weapon layer,
		// but only if the muzzleflash has the same parent as the weapon
		// camera (the local player). if there is no weapon camera, we leave
		// layer as-is, or the muzzleflash will be invisible for local player
		GameObject weaponCam = GameObject.Find("WeaponCamera");
		if (weaponCam != null)
		{
			if (weaponCam.transform.parent == transform.parent)
				gameObject.layer = vp_Layer.Weapon;
		}

		//StartBeam();
	}

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

	void Update()
	{

		// editor force show
		if (m_ForceShow)
			Show();
	}

	public void Show()
	{
		FireBeam();
	}

	public void Shoot(){
		//FireBeam();
	}


	void OnStart_Attack()
	{
		Debug.Log("OnStartAttack was actually called");
		StartBeam();
		FireBeam();
	}

	void OnStop_Attack()
	{
		Debug.Log("OnStopAttack was actually called");
		EndBeam();
	}



	public void StartBeam () {
		beamStart = Instantiate(beamStartPrefab[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		beamStart.transform.parent = this.transform;
		beamEnd = Instantiate(beamEndPrefab[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		beamEnd.transform.parent = this.transform;
		beam = Instantiate(beamLineRendererPrefab[0], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		line = beam.GetComponent<LineRenderer>();
		beam.transform.parent = this.transform;
	}

	public void EndBeam(){
		Destroy(beamStart);
		Destroy(beamEnd);
		Destroy(beam);
	}

	public void FireBeam(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray.origin, ray.direction, out hit))
		{
			Vector3 tdir = hit.point - transform.position;
			ShootBeamInDir(transform.position, tdir);
		}
	}

	void ShootBeamInDir(Vector3 start, Vector3 dir)
	{
		line.SetVertexCount(2);
		line.SetPosition(0, start);
		beamStart.transform.position = start;

		Vector3 end = Vector3.zero;
		RaycastHit hit;
		if (Physics.Raycast(start, dir, out hit))
			end = hit.point - (dir.normalized * beamEndOffset);
		else
			end = transform.position + (dir * 100);

		beamEnd.transform.position = end;
		line.SetPosition(1, end);

		beamStart.transform.LookAt(beamEnd.transform.position);
		beamEnd.transform.LookAt(beamStart.transform.position);

		float distance = Vector3.Distance(start, end);
		line.sharedMaterial.mainTextureScale = new Vector2(distance / textureLengthScale, 1);
		line.sharedMaterial.mainTextureOffset -= new Vector2(Time.deltaTime * textureScrollSpeed, 0);
	}
}
