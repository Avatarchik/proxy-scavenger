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
	Nentitium = 6,
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

		[BoxGroup("Currency Generators")]
		public GeneratorUnit[] CurrencyGenerators;

		public bool HackingRunning = false;

		// Use this for initialization
		void Start () {
			//StartHackingGame();
			foreach(GeneratorUnit g in CurrencyGenerators){
				g.init();
			}
		}
		
		// Update is called once per frame
		void Update () {
			if(HackingRunning){
				GameUpdate();
			}
		}

		public void StartHackingGame(){
			HackingRunning = true;

			while(HackingRunning)
			{
				GameUpdate();
			}
		}

		public void GameUpdate(){
			foreach(GeneratorUnit g in CurrencyGenerators){
				if(g.IsThisLocked() == false){
					g.GeneratorUpdate();
				}
			}
		}

		public void AddCurrency(float a){
			AccruedCurrency += a;
		}

		public void RemoveCurrency(float r){
			if(r <= AccruedCurrency){
				AccruedCurrency -= r;
			}
		}

		public float GetCurrentCurrency(){
			return AccruedCurrency;
		}

		public void Reset(){
			AccruedCurrency = 0f;
		}
	}
}
