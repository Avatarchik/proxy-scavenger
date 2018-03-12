using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

public class SuitBankVisualizer : MonoBehaviour {

	public BankUI Bank;
	public ItemCollectionBase SuitBank;
	public uint SuitContainerCount;
	public ICollectionItem[] Suits;
	public float SuitContainerOffset = -2f;
	public Transform SuitContainerInitSpawnPoint;
	public GameObject SuitContainer;
	public GameObject[] SuitContainers;

	// Use this for initialization
	void Awake () {
		MakeContainers();
		this.enabled = true;
		SuitBank.OnAddedItem += ItemAdded;
		SuitBank.OnRemovedItem += ItemRemoved;
		SuitBank.OnResized += ContainerResized;
		//SuitBank.OnSetItem += ItemSet;
		SuitBank.OnSwappedItems += ItemSwapped;
		Bank.OnAddedItem += ItemAdded;
	}

	public void ItemSwapped(ItemCollectionBase fromCollection, uint fromSlot, ItemCollectionBase toCollection, uint toSlot){
		Debug.Log("Item Swapped");
		FillContainers();
	}

	public void ItemAdded (IEnumerable<InventoryItemBase> i, uint amount, bool cameFromCollection){
		
		//Debug.Log(SuitBank.FindAllByCategory(
		//Debug.Log(item.name + " item added");
		Debug.Log("Item Added was Called");
		FillContainers();
	}

	public void ItemRemoved(InventoryItemBase item, uint ID , uint slot, uint amount){
		Debug.Log("Item Removed was called");
		FillContainers();
	}

	public void ContainerResized(uint fromSize, uint toSize){
		MakeContainers();
		//FillContainers();
	}

	public void MakeContainers(){
		SuitContainerCount = SuitBank.initialCollectionSize;
		SuitContainers = new GameObject[SuitContainerCount];
		for(uint i = 0; i < SuitContainerCount; i++){
			Debug.Log(i + " Suit Container Counter");
			GameObject g = GameObject.Instantiate(SuitContainer,SuitContainerInitSpawnPoint.position,SuitContainerInitSpawnPoint.rotation);
			SuitContainers[i] = g;
			if(i != 0){
				float p = SuitContainerInitSpawnPoint.position.z + SuitContainerOffset;
				Vector3 v = new Vector3(SuitContainerInitSpawnPoint.position.x, SuitContainerInitSpawnPoint.position.y, p);
				g.transform.position = v;
			}
		}
		Debug.Log("Made it to the end of creating suit containers");
	}

	public void FillContainers(){
		
		ICollectionItem[] sbi = SuitBank.items;
		uint c = 0;
		foreach(ICollectionItem i in sbi){
			bool isNull = true;

			if( i.item == null){
				isNull = true;
				Debug.Log("i.item IS null!");
			} else {
				isNull = false;
			}

			if(!isNull){
				InventoryItemBase s = i.item;
				SuitHolder sh = SuitContainers[c].GetComponent<SuitHolder>();
				if(sh != null){
					sh.SuitObject(s.ID);
				}
			} else {
				SuitHolder sh = SuitContainers[c].GetComponent<SuitHolder>();
				if(sh != null){
					Destroy(sh.CurrentSuitObject);
				}
			}

			c++;
		}

		/*
		Debug.Log("Fill Containers Called");
		uint i = item.ID;
		uint o = 0;
		foreach(var u in SuitContainers){
			SuitHolder sh = SuitContainers[o].GetComponent<SuitHolder>();
			if(sh != null){
				sh.SuitObject(i);
			}
			o++;
		}
		*/
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
