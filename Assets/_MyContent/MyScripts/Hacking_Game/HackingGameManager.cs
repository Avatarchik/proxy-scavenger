using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Devdog.InventoryPro;
using Devdog.General.UI;
using Sirenix.OdinInspector;

public enum GeneratorType {
	Nanite = 0,
	NanoMachine = 1,
	NanoCortex = 2,
	Nanocite = 3,
	Nentite = 4,
	NentiteController = 5,
	Nentium = 6,
}

public enum HackingUnlock {
	UnlockTerminal = 0,
	UnlockMap = 1,
	UnlockBrokenUnitsLocation = 2,
}

namespace mindler.hacking 
{
	public class HackingGameManager : MonoBehaviour {

		[BoxGroup("Current Currency")]
		public float AccruedCurrency = 0f;

		[BoxGroup("Current Currency Text")]
		public HackingTopBar TopBar;

		[BoxGroup("Generator Objects")]
		public GameObject GeneratorHolder;
		[BoxGroup("Generator Objects")]
		public GameObject GeneratorUI;
		[BoxGroup("Generator Objects")]
		public GameObject[] Generators;

		[BoxGroup("Currency Generator Bases")]
		public GeneratorBase[] CurrencyGeneratorBase;

		[BoxGroup("Currency Generators")]
		public GeneratorUnit[] CurrencyGenerators;

		[BoxGroup("Hacking")]
		public bool Hackable = false;
		[BoxGroup("Hacking")]
		public bool HackingRunning = false;
		[BoxGroup("Hacking")]
		public bool HackLoopRunning = false;
		[BoxGroup("Hacking")]
		public bool HackShown = false;

		[BoxGroup("HackingUI")]
		public UIWindow HackWindow;
		[BoxGroup("HackingUI")]
		public UIWindow HackGame;
		[BoxGroup("HackingUI")]
		public UIWindow HackStartScreen;
		[BoxGroup("HackingUI")]
		public Button StartHackButton;

		[BoxGroup("HackingItems")]
		public GameObject HackingTool;
		[BoxGroup("HackingItems")]
		public InventoryItemBase HackingToolItem;
		[BoxGroup("HackingAttachment")]
		public GameObject HackingAttachment;
		[BoxGroup("HackingAttachment")]
		public InventoryItemBase HackingAttachmentItem;


		// Use this for initialization
		void Start () {

			HackingToolItem = HackingTool.GetComponent<InventoryItemBase>();
			HackingAttachmentItem = HackingAttachment.GetComponent<InventoryItemBase>();

			int p = CurrencyGeneratorBase.Length;
			Generators = new GameObject[p];
			CurrencyGenerators = new GeneratorUnit[p];

			int t = 0;
			foreach(GeneratorBase i in CurrencyGeneratorBase){
				GameObject g = GameObject.Instantiate(GeneratorUI, GeneratorHolder.transform);
				GeneratorUnit gu = g.GetComponent<GeneratorUnit>();
				gu.baseObject = i;
				gu.HGM = this;
				if(t == 0){
					gu.Locked = false;
				}
				gu.init();

				Generators[t] = g;
				CurrencyGenerators[t] = gu;

				t++;
			}
			/*
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.init();
			}
			*/
			TopBar.UpdateCurrentCurrency(0f);
			if(HackingRunning){
				StartCoroutine("StartHackingGame");
			}

		}


		// Update is called once per frame
		void Update () {
			/*
			if(HackingRunning){
				GameUpdate();
				TopBar.UpdateCurrentCurrency(AccruedCurrency);
			}
			*/

		}


		IEnumerator StartHackingGame()
		{
			
			while(HackingRunning)
			{
				GameUpdate();

				TopBar.UpdateCurrentCurrency(AccruedCurrency);
				yield return null;
			}
		}

		public void GameUpdate(){
			if(!HackingRunning){
				StopHack();
				return;
			}

			foreach(GeneratorUnit g in CurrencyGenerators){
				if(g.IsThisLocked() == false){
					g.GeneratorUpdate();
				} else {
					g.CheckPurchaseable();
				}
			}
		}

		public void AddCurrency(float a){
			AccruedCurrency += a;
			double d = System.Math.Round(AccruedCurrency, 2);
			AccruedCurrency = (float)d;
		}

		public void RemoveCurrency(float r){
			if(r <= AccruedCurrency){
				AccruedCurrency -= r;

				double d = System.Math.Round(AccruedCurrency, 2);
				AccruedCurrency = (float)d;
			}
		}

		public float GetCurrentCurrency(){
			return AccruedCurrency;
		}

		public void StartHack(){
			HackingRunning = true;
			HackStartScreen.Hide();
			HackGame.Show();
			StartCoroutine("StartHackingGame");
		}

		public void StopHack(){
			HackingRunning = false;
			StopCoroutine("StartHackingGame");
			HackStartScreen.Show();
			HackGame.Hide();
			StartHackButton.interactable = false;
			Reset();
		}

		public void Reset(){
			AccruedCurrency = 0f;
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.Reset();
			}
		}

		public void ShipHackable(bool value){
			Debug.Log(value + " Ship Hackable?");
			Hackable = value;
			StartHackButton.interactable = value;
		}

		public void ShowHackWindow(){
			if(!HackShown){

				HackShown = true;
				HackWindow.Show();
				if(HackingRunning){
					HackStartScreen.Hide();
					HackGame.Show();
				} else {
					HackStartScreen.Show();
					HackGame.Hide();
				}
			}
		}

		public void HideHackWindow(){
			HackShown = false;
			HackWindow.Hide();
		}
	}
}
