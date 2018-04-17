using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Devdog.General;
using Devdog.General.UI;
using Devdog.InventoryPro.Dialogs;
using Devdog.General.ThirdParty.UniLinq;
using Sirenix.OdinInspector;

namespace mindler.hacking 
{
	public partial class HackableBox : MonoBehaviour, ITriggerCallbacks
	{

		[BoxGroup("Managers")]
		public GameManager GM;
		[BoxGroup("Managers")]
		public HackingGameManager HM;

		[BoxGroup("Inventory Items")]
		public InventoryItemBase CurrentEquippedItem;
		[BoxGroup("Inventory Items")]
		public InventoryItemBase HackingInventoryItem;
		[BoxGroup("Inventory Items")]
		public InventoryItemBase LocalHackingInventoryItem;

		[BoxGroup("Hacking Game Objects")]
		public GameObject RemoteHackingItem;
		[BoxGroup("Hacking Game Objects")]
		public GameObject LocalHackingItem;
		[BoxGroup("Hacking Game Objects")]
		public GameObject HackingObject;

		[BoxGroup("Remote Hacking Game Objects")]
		public GameObject RemoteHackingItemAnchor;
		[BoxGroup("Remote Hacking Game Objects")]
		public GameObject RemoteHackingSpawnedItem;

		private Trigger myTrigger;

		// Use this for initialization
		void Start () {
			GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
			HM = GM.GetComponentInChildren<HackingGameManager>();
			HackingInventoryItem = RemoteHackingItem.GetComponent<InventoryItemBase>();
			LocalHackingInventoryItem = LocalHackingItem.GetComponent<InventoryItemBase>();
			HackingObject.SetActive(false);
			myTrigger = this.gameObject.GetComponent<Trigger>();
		}

		void Update(){
			if(RemoteHackingSpawnedItem == null && myTrigger.enabled == false){
				myTrigger.enabled = true;
			}
		}

		public bool OnTriggerUsed(Player player)
		{
			Debug.Log("Hacking Box Used!");
			if(GM.CurrentEquippedItem != null){
				if(GM.CurrentEquippedItem.name == HackingInventoryItem.name){
					Debug.Log("Hacking Item Used on Box");
					//HackingObject.SetActive(true);

					if(RemoteHackingSpawnedItem == null){
						SpawnHackingItem();

						GM.RemoveCurrentItem();
						HM.AddRemoveRemoteHackingUnits(1);

						myTrigger.enabled = false;
					}

				} else if(GM.CurrentEquippedItem.name == LocalHackingInventoryItem.name){
					HM.SetLocalHack(true);
					HM.ShowHackWindow();
				} else {
					Debug.Log(GM.CurrentEquippedItem.name + " - " + HackingInventoryItem.name);
				}
			} else {
				Debug.Log("CurrentItem is null");
			}
			//OnTriggerUnUsed(player);
			return true;
			// The trigger has been used.

			// Return true to consume the event (other callback listeners won't receive it)
			// Return false to not cosnume the event.
		}

		public bool OnTriggerUnUsed(Player player)
		{
			Debug.Log("Trigger Unused?");

			// The trigger has een unused
			// Return true to consume the event (other callback listeners won't receive it)
			// Return false to not cosnume the event.
			return true;
		}

		public void SpawnHackingItem(){
			GameObject g = RemoteHackingItem;
			Vector3 v = Vector3.zero;
			Quaternion q = Quaternion.identity;

			GameObject i = GameObject.Instantiate(g,v,q);
			//UnityEngine.Object.Destroy(i.GetComponent<ITriggerInputHandler>() as UnityEngine.Component);
			//UnityEngine.Object.Destroy(i.GetComponent<TriggerBase>());
			//UnityEngine.Object.Destroy(i.GetComponent<InventoryItemBase>());
			//UnityEngine.Object.Destroy(i.GetComponent<SphereCollider>());
			if(i.GetComponent<SphereCollider>()){
				i.GetComponent<SphereCollider>().enabled = false;
			}
			//UnityEngine.Object.Destroy(i.GetComponent<BoxCollider>());

			//UnityEngine.Object.Destroy(i.GetComponent<Rigidbody>());
			Rigidbody r = i.GetComponent<Rigidbody>();
			r.useGravity = false;
			r.constraints = RigidbodyConstraints.FreezeAll;

			Vector3 p = RemoteHackingItemAnchor.transform.position;

			i.transform.position = p;
			i.transform.rotation = q;
			//i.transform.localScale = Vector3.one;

			i.transform.parent = RemoteHackingItemAnchor.transform;

			Vector3 lp = RemoteHackingItemAnchor.transform.localPosition;

			i.transform.localPosition = lp;
			i.transform.localRotation = q;
			RemoteHackingSpawnedItem = i;
		}

		public void RemoveSpawnedItem(){
			if(RemoteHackingSpawnedItem != null){
				Destroy(RemoteHackingSpawnedItem);
			}
		}
	}
}
