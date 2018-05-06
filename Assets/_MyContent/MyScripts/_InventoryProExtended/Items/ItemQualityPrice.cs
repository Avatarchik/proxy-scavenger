using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventoryPro;
using Sirenix.OdinInspector;

[System.Serializable]
public partial class ItemQualityPrice {

	[BoxGroup("Item Rarity")]
	public ItemRarity Rarity;

	[BoxGroup("Item Value")]
	public float SellPrice;
	[BoxGroup("Item Value")]
	public float BuyPrice;

}
