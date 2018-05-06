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

		[BoxGroup("Item Object")]
		public InventoryInteractableItemObject itemObject;

		[BoxGroup("Empty Item")]
		public bool initialItemEmpty = false;

		public void Setup(GameObject parent, GameObject anchor, ItemCollectionSlotUI slotUI)
		{
			parentObject = parent;
			itemAnchor = anchor;
			slot = slotUI;
		}

		public void init()
		{
			if(initialItemEmpty){
				currentItem = null;
				DestroyItem();
			} else {
				if(!currentItemGO){
					if(itemObject){
						currentItemGO = itemObject.InventoryItem;
						visualItem = MakeVisualObject(currentItemGO);
						if(currentItemGO.GetComponent<InventoryItemBase>() != null){
							currentItem = currentItemGO.GetComponent<InventoryItemBase>();
						}
					} else {
						Debug.Log("No CurrentItemGO AND No itemObject");
					}
				} else {
					visualItem = MakeVisualObject(currentItemGO);
					if(currentItemGO.GetComponent<InventoryItemBase>() != null){
						currentItem = currentItemGO.GetComponent<InventoryItemBase>();
					}
				}
			}

			if(itemObject){
				mountingItem = itemObject.mountingObject;
			} else {
				Debug.Log("No mounting item because there's no itemObject");
			}

			if(mountingItem != null){
				mountingItemGO = GameObject.Instantiate(mountingItem,itemAnchor.transform);
				mountingItemGO.transform.localPosition = Vector3.zero;
				mountingItemGO.transform.localRotation = Quaternion.identity;
			} else {
				Debug.Log("No Mounting Item because mounting item is null");
			}

			if(currentItem != null && visualItem != null){
				Debug.Log(currentItem.name + " : " + visualItem.transform.localRotation + " visual item local rotation");
			}
		}

		public void DestroyItem()
		{
			if(visualItem != null){
				GameObject.Destroy(visualItem);
				visualItem = null;
			}
			if(currentItemGO != null){
				//GameObject.Destroy(currentItemGO);
				currentItemGO = null;
			}
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
