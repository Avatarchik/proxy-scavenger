using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjectSlide : vp_Interactable {

	public Texture GrabStateCrosshair = null; // crosshair to display while holding this object
	public float Stiffness = 0.5f;			// how sluggishly the object will react when moved
	protected AudioSource m_Audio = null;
	protected vp_FPInteractManager m_InteractManager = null;	// caches the interaction manager
	protected Vector2 m_CurrentMouseMove;
	protected Vector3 m_CurrentSwayForce;

	public Vector3 CarryingOffset = new Vector3(0.0f, -0.5f, 1.5f);	// determines how the object is carried in relation to our body
	protected Vector3 m_TempCarryingOffset = Vector3.zero;			// work variable
	protected bool m_IsFetching = false;
	protected float duration = 0.0f;
	protected float m_FetchProgress = 0;

	protected int m_LastWeaponEquipped = 0;						// used to store the id of our weapon so we can reequip it
	protected bool m_IsGrabbed = false;

	// sounds
	public Vector2 SoundsPitch = new Vector2(1.0f, 1.5f);	// random pitch range for grab and release sounds
	public List<AudioClip> GrabSounds = new List<AudioClip>(); // list of sounds to randomly play on grab

	//public bool ClampMovement = true;
	public Transform initTransform;
	public Vector3 initPosition;

	public Vector3 Offset;
	public Vector3 MaxOffset;

	public bool lockX = false;
	public bool lockY = false;
	public bool lockZ = false;

	public bool clampX = false;
	public bool clampY = false;
	public bool clampZ = false;

	// timers
	protected vp_Timer.Handle m_DisableAngleSwayTimer = new vp_Timer.Handle();

	vp_FPPlayerEventHandler m_FPPlayer = null;
	vp_FPPlayerEventHandler FPPlayer
	{
		get
		{
			if (m_FPPlayer == null)
				m_FPPlayer = (m_Player as vp_FPPlayerEventHandler);
			return m_FPPlayer;
		}
	}
	protected Rigidbody m_Rigidbody = null;
	protected Rigidbody Rigidbody
	{
		get
		{
			if (m_Rigidbody == null)
				m_Rigidbody = GetComponent<Rigidbody>();
			return m_Rigidbody;
		}
	}

	protected Collider m_Collider = null;
	protected Collider Collider
	{
		get
		{
			if (m_Collider == null)
				m_Collider = GetComponent<Collider>();
			return m_Collider;
		}
	}

	protected override void Start()
	{

		base.Start();

		if ((Rigidbody == null) || (Collider == null))
			this.enabled = false;

		// cache rigidbody vars

		/*
		if (Rigidbody != null)
		{
			m_DefaultGravity = Rigidbody.useGravity;
			m_DefaultDrag = Rigidbody.drag;
			m_DefaultAngularDrag = Rigidbody.angularDrag;
		}
		*/

		// for normal interaction type
		InteractType = vp_InteractType.Normal;

		m_InteractManager = GameObject.FindObjectOfType(typeof(vp_FPInteractManager)) as vp_FPInteractManager;

		//initTransform = this.transform;
		//initPosition = initTransform.position;
		//Offset = initPosition;
		//Offset += MaxOffset;
		StartPosition();

	}

	public void StartPosition(){
		initTransform = this.transform;
		initPosition = initTransform.localPosition;

		Offset = initPosition + MaxOffset;
	}
	
	/// <summary>
	/// 
	/// </summary>
	protected virtual void FixedUpdate()
	{

		if (!m_IsGrabbed)
			return;

		//UpdateShake();

		UpdatePosition();

		//UpdateRotation();

		//UpdateBurden();

		DampenForces();

	}


	/// <summary>
	/// 
	/// </summary>
	protected virtual void Update()
	{

		if (!m_IsGrabbed)
			return;

		UpdateInput();

	}

	/// <summary>
	/// Handles mouse input and special cases for dropping the
	/// grabbable versus putting a weapon away.
	/// </summary>
	protected virtual void UpdateInput()
	{

		m_CurrentMouseMove.x = FPPlayer.InputRawLook.Get().x * Time.timeScale;
		m_CurrentMouseMove.y = FPPlayer.InputRawLook.Get().y * Time.timeScale;

		// toss object upon fire button pressed
		if (FPPlayer.InputGetButtonDown.Send("Attack"))
		{
			FPPlayer.Interact.TryStart();
			return;
		}

		// force-prevent wielding weapons and grabbables at the same time
		if (m_Player.CurrentWeaponIndex.Get() != 0)
		{
			m_Player.SetWeapon.TryStart(0);
			return;
		}

	}

	/// <summary>
	/// Interpolates the object smoothly to the player's grip,
	/// and applies positional sway while moving around.
	/// </summary>
	protected virtual void UpdatePosition()
	{

		// calculate positional sway force
		/*
		m_CurrentSwayForce += m_Player.Velocity.Get() * 0.005f;
		m_CurrentSwayForce.y += m_CurrentFootstepForce;
		m_CurrentSwayForce += m_Camera.Transform.TransformDirection(new Vector3(
			m_CurrentMouseMove.x * 0.05f,
			// prevent vertical sway if we hit lower pitch limit
			(m_Player.Rotation.Get().x > m_Camera.RotationPitchLimit.y) ? m_CurrentMouseMove.y * 0.015f : m_CurrentMouseMove.y * 0.05f,
			0.0f));
		*/

		//m_TempCarryingOffset = (m_Player.IsFirstPerson.Get() ? CarryingOffset : CarryingOffset - m_Camera.Position3rdPersonOffset);

		// update object position
		/*
		m_Transform.position = Vector3.Lerp(m_Transform.position, (m_Camera.Transform.position - m_CurrentSwayForce) +
			(m_Camera.Transform.right * m_TempCarryingOffset.x) +
			(m_Camera.Transform.up * m_Transform.localScale.y * (m_TempCarryingOffset.y + (m_CurrentShake.y * 0.5f))) +
			(m_Camera.Transform.forward * m_TempCarryingOffset.z),
			((m_FetchProgress < 1.0f) ? m_FetchProgress : (Time.deltaTime * (Stiffness * 60.0f))));
			*/

		m_CurrentSwayForce = m_Camera.Transform.TransformDirection(new Vector3(
			//m_CurrentMouseMove.x * 0.05f,
			m_CurrentMouseMove.x * 0.05f,
			// prevent vertical sway if we hit lower pitch limit
			(m_Player.Rotation.Get().x > m_Camera.RotationPitchLimit.y) ? m_CurrentMouseMove.y * 0.015f : m_CurrentMouseMove.y * 0.05f,
			0.0f));

		m_TempCarryingOffset = (m_Player.IsFirstPerson.Get() ? CarryingOffset : CarryingOffset - m_Camera.Position3rdPersonOffset);
		//m_TempCarryingOffset = Vector3.one;

		Vector3 lerpPos;


		lerpPos = Vector3.Lerp(m_Transform.position, (m_Camera.Transform.position - m_CurrentSwayForce)+
			(m_Camera.Transform.right * m_TempCarryingOffset.x) +
			(m_Camera.Transform.up * m_Transform.localScale.y * m_TempCarryingOffset.y) +
			(m_Camera.Transform.forward * m_TempCarryingOffset.z), 
			((m_FetchProgress < 1.0f) ? m_FetchProgress : (Time.deltaTime * (Stiffness * 60.0f))));


		Transform t = this.transform.parent;

		Vector3 newPos = t.InverseTransformPoint(lerpPos);


		if(lockX){
			newPos.x = initPosition.x;
		}
		if(lockY){
			newPos.y = initPosition.y;
		}
		if(lockZ){
			newPos.z = initPosition.z;
		}

		/*
		if(clampX){
			
			if(MaxOffset.x > 0){
				if(lerpPos.x > Offset.x){
					newPos.x = Offset.x;
				} else if (lerpPos.x < initPosition.x){
					newPos.x = initPosition.x;
				}
			} else if (MaxOffset.x < 0){
				if(lerpPos.x < Offset.x){
					newPos.x = Offset.x;
				} else if (lerpPos.x > initPosition.x){
					newPos.x = initPosition.x;
				}
			}
		}
		if(clampY){
			newPos.y = Mathf.Clamp(lerpPos.y, initPosition.y, Offset.y);
		}
		if(clampZ){
			newPos.z = Mathf.Clamp(lerpPos.z, initPosition.z, Offset.z);
		}
		clampedLerpPos = newPos;
		*/

		if(clampX){
			if(Offset.x > initPosition.x){
				newPos.x = Mathf.Clamp(newPos.x, initPosition.x, Offset.x);
			} else {
				newPos.x = Mathf.Clamp(newPos.x,  Offset.x, initPosition.x);
			}
		}
		if(clampY){
			if(Offset.y > initPosition.y){
				newPos.y = Mathf.Clamp(newPos.y, initPosition.y, Offset.y);
			} else {
				newPos.y = Mathf.Clamp(newPos.y,  Offset.y, initPosition.y);
			}
		}
		if(clampZ){
			if(Offset.z > initPosition.z){
				newPos.z = Mathf.Clamp(newPos.z, initPosition.z, Offset.z);
			} else {
				newPos.z = Mathf.Clamp(newPos.z,  Offset.z, initPosition.z);
			}
		}


		m_Transform.localPosition = newPos;

	}

	public void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(m_Transform.localPosition, 1);
	}

	public override bool TryInteract(vp_PlayerEventHandler player)
	{

		if (!(player is vp_FPPlayerEventHandler))
			return false;

		if (m_Player == null)
			m_Player = player;

		if (player == null)
			return false;

		if (m_Controller == null)
			m_Controller = m_Player.GetComponent<vp_FPController>();

		if (m_Controller == null)
			return false;

		if (m_Camera == null)
			m_Camera = m_Player.GetComponentInChildren<vp_FPCamera>();

		if (m_Camera == null)
			return false;

		if (m_WeaponHandler == null)
			m_WeaponHandler = m_Player.GetComponentInChildren<vp_WeaponHandler>();

		if (m_Audio == null)
			m_Audio = m_Player.GetComponent<AudioSource>();

		m_Player.Register(this);

		if (!m_IsGrabbed)
			StartGrab(); // start the grab
		else
			StopGrab(); // if object grabbed, stop the grab

		// set this object as the one the player is currently
		// interacting with
		m_Player.Interactable.Set(this);

		// if we have a grab state crosshair, set it
		if (GrabStateCrosshair != null)
			FPPlayer.Crosshair.Set(GrabStateCrosshair);
		else
			FPPlayer.Crosshair.Set(new Texture2D(0, 0));

		return true;

	}

	protected virtual void StartGrab()
	{
		// play a grab sound
		vp_AudioUtility.PlayRandomSound(m_Audio, GrabSounds, SoundsPitch);

		/*
		// show a HUD text, 
		if (!string.IsNullOrEmpty(OnGrabText))
			FPPlayer.HUDText.Send(OnGrabText);
		*/

		m_LastWeaponEquipped = m_Player.CurrentWeaponIndex.Get();

		// if we have no weapon wielded, and don't have a running 'Fetch'
		// coroutine, start one now. otherwise, unwield weapon and let
		// our 'OnStop_SetWeapon' callback start the coroutine
		m_FetchProgress = 0.0f;
		if (m_LastWeaponEquipped != 0)
			m_Player.SetWeapon.TryStart(0);
		else if (!m_IsFetching)
			StartCoroutine("Fetch");
		
		if (m_LastWeaponEquipped != 0)
			m_Player.SetWeapon.TryStart(0);

		// ready to start grabbing!
		m_IsGrabbed = true;

	}

	protected virtual void StopGrab()
	{
		// reset grab and fetch states
		m_IsGrabbed = false;
		m_FetchProgress = 1.0f;

		// ready the weapon we were using
		m_Player.SetWeapon.TryStart(m_LastWeaponEquipped);

		if (m_InteractManager == null)
			m_InteractManager = GameObject.FindObjectOfType(typeof(vp_FPInteractManager)) as vp_FPInteractManager;

		// disallow the crosshair to change again for half a second
		m_InteractManager.CrosshairTimeoutTimer = Time.time + 0.5f;
	}

	/// <summary>
	/// Stops interacting with this object.
	/// </summary>
	public override void FinishInteraction()
	{

		if (m_IsGrabbed)
			StopGrab();

	}

	protected virtual IEnumerator Fetch()
	{

		// reset various variables
		m_IsFetching = true;
		m_CurrentSwayForce = Vector3.zero;
		//m_CurrentSwayTorque = Vector3.zero;
		///m_CurrentFootstepForce = 0.0f;
		m_FetchProgress = 0.0f;

		// the time it takes to grab something will depend on the
		// distance to it
		duration = Vector3.Distance(m_Camera.Transform.position, m_Transform.position) * 0.5f;

		// prohibit angular sway for a while post fetching
		vp_Timer.In(duration + 1.0f, delegate() { }, m_DisableAngleSwayTimer);

		while (m_FetchProgress < 1.0f)
		{
			m_FetchProgress += Time.deltaTime / duration;
			yield return new WaitForEndOfFrame();
		}
		m_IsFetching = false;

	}

	/// <summary>
	/// Makes all physics forces continually wear off.
	/// </summary>
	protected virtual void DampenForces()
	{

		//m_CurrentSwayForce *= 0.9f;
		//m_CurrentSwayTorque *= 0.9f;
		//m_CurrentFootstepForce *= 0.9f;

	}
}
