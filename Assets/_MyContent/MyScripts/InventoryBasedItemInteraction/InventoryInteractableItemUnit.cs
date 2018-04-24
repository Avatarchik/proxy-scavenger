using System.Collections.Generic;
using Devdog.General.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;
using Devdog.General;
using Devdog.InventoryPro;
using Devdog.InventoryPro.Integration.UFPS;
using Sirenix.OdinInspector;

namespace mindler{
	[System.Serializable]
	public partial class InventoryInteractableItemUnit {
		
		[FoldoutGroup("Parent Object")]
		public GameObject parentObject;

		[FoldoutGroup("Mounting Items")]
		public GameObject mountingItem;
		[FoldoutGroup("Mounting Items")]
		public GameObject mountingItemGO;

		[FoldoutGroup("Item Anchor")]
		public GameObject itemAnchor;

		[FoldoutGroup("Visual Item")]
		public GameObject visualItem;

		[FoldoutGroup("Item Slot")]
		public ItemCollectionSlotUI slot;

		[FoldoutGroup("Current Item Game Object")]
		public GameObject currentItemGO;

		[FoldoutGroup("Current Item")]
		public InventoryItemBase currentItem;


		public void Setup(GameObject parent, GameObject anchor, ItemCollectionSlotUI slotUI)
		{
			parentObject = parent;
			itemAnchor = anchor;
			slot = slotUI;
			//mountingItem = mount;
		}

		public void init()
		{

			GameObject c = MakeVisualObject(currentItemGO);

			visualItem = c;

			if(mountingItemGO != null){
				mountingItemGO = GameObject.Instantiate(mountingItem,parentObject.transform);
				mountingItemGO.transform.position = itemAnchor.transform.position;
				mountingItemGO.transform.rotation = itemAnchor.transform.rotation;
			}

		}

		public void DestroyItem()
		{
			GameObject.Destroy(currentItemGO);
		}

		public void SetCurrentItem(){
			DestroyItem();
			if(currentItem != null){
				GameObject go = currentItem.gameObject;
				currentItemGO = MakeVisualObject(go);
				currentItemGO.SetActive(true);
			} else {
				Debug.Log("Can't make visual item because current item is null");
			}

		}

		public GameObject MakeVisualObject(GameObject g){
			Vector3 v = Vector3.zero;
			Quaternion q = Quaternion.identity;

			GameObject i = GameObject.Instantiate(g,itemAnchor.transform);

			UnityEngine.Object.Destroy(i.GetComponent<InventoryItemBase>());
			if(i.GetComponent<ItemTriggerUFPS>()){
				UnityEngine.Object.Destroy(i.GetComponent<ItemTriggerUFPS>());
			}
			if(i.GetComponent<vp_Grab>()){
				UnityEngine.Object.Destroy(i.GetComponent<vp_Grab>());
			}
			UnityEngine.Object.Destroy(i.GetComponent<ITriggerInputHandler>() as UnityEngine.Component);
			UnityEngine.Object.Destroy(i.GetComponent<TriggerBase>());
			UnityEngine.Object.Destroy(i.GetComponent<SphereCollider>());
			if(i.GetComponent<SphereCollider>()){
				i.GetComponent<SphereCollider>().enabled = false;
			}
			UnityEngine.Object.Destroy(i.GetComponent<BoxCollider>());

			UnityEngine.Object.Destroy(i.GetComponent<Rigidbody>());

			i.transform.localPosition = v;
			i.transform.localRotation = q;
			i.transform.localScale = Vector3.one;

			return i;
		}
	}
}
