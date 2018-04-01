using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

		public bool HackingRunning = false;
		public bool HackLoopRunning = false;

		// Use this for initialization
		void Start () {

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
			StartCoroutine("StartHackingGame");
		}

		public void StopHack(){
			HackingRunning = false;
		}

		public void Reset(){
			HackingRunning = false;
			AccruedCurrency = 0f;
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.Reset();
			}
		}
	}
}
