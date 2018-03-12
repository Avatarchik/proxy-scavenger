using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoHUD : MonoBehaviour {

	public Text AmmoClip;
	public Text AmmoReserves;
	public Image AmmoTypeImage;

	public Color AmmoColor = Color.white;
	public Color AmmoLowColor = new Color(0, 0, 0, 1);
	public Color InvisibleColor = new Color(0, 0, 0, 0);

	private vp_FPPlayerEventHandler player;
	private AudioSource audio;



	void Awake(){
		player = transform.GetComponent<vp_FPPlayerEventHandler>();
		audio = player.transform.GetComponent<AudioSource>();
	}

	/// <summary>
	/// registers this component with the event handler (if any)
	/// </summary>
	protected virtual void OnEnable()
	{

		if (player != null)
			player.Register(this);

	}


	/// <summary>
	/// unregisters this component from the event handler (if any)
	/// </summary>
	protected virtual void OnDisable()
	{

		if (player != null)
			player.Unregister(this);

	}
	
	// Update is called once per frame
	void Update () {
		AmmoClip.text = player.CurrentWeaponClipCount.Get().ToString();
		AmmoReserves.text = player.CurrentWeaponAmmoCount.Get().ToString();

		if ((player.CurrentWeaponIndex.Get() == 0) || (player.CurrentWeaponType.Get() == (int)vp_Weapon.Type.Melee)){
			AmmoClip.text = "";
			AmmoReserves.text = "";
			AmmoTypeImage.color = InvisibleColor;
		} else {
			AmmoTypeImage.color = new Color(1, 1, 1, 1);
		}

		if ((player.CurrentWeaponAmmoCount.Get() < 1) && (player.CurrentWeaponType.Get() != (int)vp_Weapon.Type.Thrown)){
			Color c = Color.Lerp(Color.white, AmmoLowColor, (vp_MathUtility.Sinus(8.0f, 0.1f, 0.0f) * 5) + 0.5f);
			AmmoClip.color = c;
			AmmoReserves.color = c;
		} else {
			AmmoClip.color = Color.white;
			AmmoReserves.color = Color.white;
		}



		Texture2D t = player.CurrentAmmoIcon.Get();
		Sprite s = Sprite.Create(t, new Rect (0, 0, 512, 512), new Vector2 (0,0));
		AmmoTypeImage.sprite = s;
	}
}
