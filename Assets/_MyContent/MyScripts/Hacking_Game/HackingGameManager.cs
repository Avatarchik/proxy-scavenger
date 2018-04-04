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
	UnlockAllTerminals = 0,
	UnlockMap = 1,
	UnlockBrokenUnitsLocation = 2,
	UnlockDoors = 3,
	UnlockImproveShipData = 4,
	UnlockEngineTerminal = 5,
	UnlockPowerCoreTerminal = 6,
	UnlockLifeSupportTerminal = 7,
	UnlockWarpDriveTerminal = 8,
	UnlockThermalShieldingTerminal = 9,
	UnlockControlsTerminal = 10
}

namespace mindler.hacking 
{
	public class HackingGameManager : MonoBehaviour {

		[BoxGroup("Debug Bonuses")]
		public float speedBoost = 1f;
		[BoxGroup("Debug Bonuses")]
		public float revenueBoost = 1f;
		[BoxGroup("Debug Bonuses")]
		public bool debugBoost = false;

		[FoldoutGroup("Currency"), BoxGroup("Currency/Current Currency")]
		public float AccruedCurrency = 0f;

		[FoldoutGroup("Currency"), BoxGroup("Currency/Number to Text Formatter")]
		public NumberStringFormatter NumberTextFormatter;

		[FoldoutGroup("Currency"), BoxGroup("Currency/Current Currency Text")]
		public HackingTopBar TopBar;


		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Generators")]
		public GameObject GeneratorHolder;
		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Generators")]
		public GameObject GeneratorUI;
		//[BoxGroup("Generator Objects")]
		private GameObject[] Generators;

		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Generators")]
		public GeneratorBase[] CurrencyGeneratorBase;
		//[BoxGroup("Currency Generators")]
		private GeneratorUnit[] CurrencyGenerators;


		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Unlocks")]
		public GameObject UnlockHolder;
		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Unlocks")]
		public GameObject UnlockUI;
		//[BoxGroup("Unlock Objects")]
		private GameObject[] UnlockObjects;

		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Unlocks")]
		public UnlockBase[] UnlockBases;
		//[BoxGroup("Unlock Units")]
		private UnlockUnit[] UnlockUnits;


		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Improvements")]
		public GameObject ImprovementHolder;
		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Improvements")]
		public GameObject ImprovementUI;
		//[BoxGroup("Improvements")]
		private GameObject[] ImprovementObjects;

		[FoldoutGroup("Generators Unlocks Improvements"), FoldoutGroup("Generators Unlocks Improvements/Improvements")]
		public ImprovementBase[] ImprovementBases;
		//[BoxGroup("Improvment Bases and Units")]
		private ImprovementUnit[] ImprovementUnits;


		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking")]
		public bool Hackable = false;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking")]
		public bool HackingRunning = false;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking")]
		public bool HackLoopRunning = false;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking")]
		public bool HackShown = false;

		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingUI")]
		public UIWindow HackWindow;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingUI")]
		public UIWindow HackGame;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingUI")]
		public UIWindow HackStartScreen;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingUI")]
		public Button StartHackButton;

		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingItems")]
		public GameObject HackingTool;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/HackingItems")]
		public InventoryItemBase HackingToolItem;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking Attachments")]
		public GameObject HackingAttachment;
		[FoldoutGroup("Hacks"), BoxGroup("Hacks/Hacking Attachments")]
		public InventoryItemBase HackingAttachmentItem;


		// Use this for initialization
		void Start () 
		{

			init();

		}

		private void init()
		{

			if(!NumberTextFormatter){
				NumberTextFormatter = this.gameObject.GetComponent<NumberStringFormatter>();
			}

			HackingToolItem = HackingTool.GetComponent<InventoryItemBase>();
			HackingAttachmentItem = HackingAttachment.GetComponent<InventoryItemBase>();

			MakeGenerators();

			MakeUnlocks();

			MakeImprovements();

			if(debugBoost){
				ApplyAllBonus(revenueBoost);
				ApplyAllSpeedBonus(speedBoost);
			}

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


		//=======================================//

		public void GameUpdate()
		{
			
			if(!HackingRunning){
				StopHack();
				return;
			}

			CheckPurchaseableUnits();

		}

		//=======================================//


		private void CheckPurchaseableUnits()
		{
			
			foreach(GeneratorUnit g in CurrencyGenerators){
				if(g.IsThisLocked() == false){
					g.GeneratorUpdate();
				} else {
					g.CheckPurchaseable();
				}
			}

			foreach(UnlockUnit u in UnlockUnits){
				if(u.Locked){
					u.CheckPurchaseable();
				}
			}

			foreach(ImprovementUnit i in ImprovementUnits){
				if(i != null){
					if(i.Locked){
						i.CheckPurchaseable();
					}
				}
			}

		}

		public void ApplyAllSpeedBonus(float multiplier){
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.AddSpeedBonus(multiplier);
			}
		}

		public void ApplySpecificSpeedBonus(float multiplier, GeneratorType g)
		{
			foreach(GeneratorUnit gu in CurrencyGenerators){
				if(gu.type == g){
					gu.AddSpeedBonus(multiplier);
				}
			}
		}

		public void ApplyAllBonus(float multiplier)
		{
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.AddBonus(multiplier);
			}
		}

		public void ApplySpecificBonus(float multiplier, GeneratorType g)
		{
			foreach(GeneratorUnit gu in CurrencyGenerators){
				if(gu.type == g){
					gu.AddBonus(multiplier);
				}
			}
		}

		public void AddCurrency(float a)
		{
			AccruedCurrency += a;
			double d = System.Math.Round(AccruedCurrency, 2);
			AccruedCurrency = (float)d;
		}

		public void RemoveCurrency(float r)
		{
			if(r <= AccruedCurrency){
				AccruedCurrency -= r;

				double d = System.Math.Round(AccruedCurrency, 2);
				AccruedCurrency = (float)d;
			}
		}

		public float GetCurrentCurrency()
		{
			return AccruedCurrency;
		}

		public void StartHack()
		{
			HackingRunning = true;
			HackStartScreen.Hide();
			HackGame.Show();
			StartCoroutine("StartHackingGame");
		}

		public void StopHack()
		{
			HackingRunning = false;
			StopCoroutine("StartHackingGame");
			HackStartScreen.Show();
			HackGame.Hide();
			StartHackButton.interactable = false;
			Reset();
		}

		public void Reset()
		{
			AccruedCurrency = 0f;
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.Reset();
			}
			foreach(UnlockUnit u in UnlockUnits){
				u.Reset();
			}
			foreach(ImprovementUnit i in ImprovementUnits){
				i.Reset();
			}
		}

		public void ShipHackable(bool value)
		{
			Debug.Log(value + " Ship Hackable?");
			Hackable = value;
			StartHackButton.interactable = value;
		}

		public void ShowHackWindow()
		{
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

		public void HideHackWindow()
		{
			HackShown = false;
			HackWindow.Hide();
		}

		private void MakeGenerators()
		{
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
		}

		private void MakeUnlocks()
		{
			int ul =  UnlockBases.Length;
			UnlockObjects = new GameObject[ul];
			UnlockUnits = new UnlockUnit[ul];

			int u = 0;
			foreach(UnlockBase un in UnlockBases){
				GameObject ug = GameObject.Instantiate(UnlockUI, UnlockHolder.transform);
				UnlockUnit uu = ug.GetComponent<UnlockUnit>();
				uu.baseObject = un;
				uu.HGM = this;
				uu.init();
				UnlockObjects[u] = ug;
				UnlockUnits[u] = uu;

				u++;
			}
		}

		private void MakeImprovements()
		{
			int il = ImprovementBases.Length;
			ImprovementObjects = new GameObject[il];
			ImprovementUnits = new ImprovementUnit[il];

			int ic = 0;
			foreach(ImprovementBase ib in ImprovementBases){
				GameObject ig = GameObject.Instantiate(ImprovementUI, ImprovementHolder.transform);
				ImprovementUnit iu = ig.GetComponent<ImprovementUnit>();
				iu.baseObject = ib;
				iu.HGM = this;
				iu.init();
				ImprovementObjects[ic] = ig;
				ImprovementUnits[ic] = iu;

				ic++;
			}
		}
	}
}
