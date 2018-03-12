using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devdog.QuestSystemPro.Dialogue;
using Devdog.QuestSystemPro.Dialogue.UI;

public class PlayerDecisionAndItemUI : MonoBehaviour {

	public Button button;
	public GameObject textGO;
	public Text text;
	public GameObject itemGO;
	public Image itemIcon;
	public GameObject itemNameGO;
	public Text itemName;
	public Image itemRarity;
	public GameObject spacerGO;

	public PlayerDecisionAndItem decision { get; protected set; }

	public void Repaint(PlayerDecisionAndItem dec, Edge edge, bool canUse)
	{
		decision = dec;

		if(dec.option.message == ""){
			textGO.SetActive(false);
		} else {
			text.text = dec.option.message;
		}

		if(dec.item != null){
			spacerGO.SetActive(false);
			itemIcon.sprite = dec.item.icon;
			itemName.text = dec.item.name;
			itemRarity.color = dec.item.rarity.color;
		} else {
			itemGO.SetActive(false);
			itemNameGO.SetActive(false);
			spacerGO.SetActive(true);
		}

		button.interactable = canUse;
	}
}
