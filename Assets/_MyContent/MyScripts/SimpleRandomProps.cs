using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRandomProps : MonoBehaviour {

	public int min;
	public int max;
	public GameObject[] Props;

	public int randomAmount;


	// Use this for initialization
	void Start () {
		randomAmount = Random.Range(min, max + 1);
		//RandomizeProps();
	}

	public void RandomizeProps(){
		for(var i = 0; i <= randomAmount; i++){
			bool nonUsedProp = false;
			int iterations = 0;
			while(nonUsedProp == false || iterations <= Props.Length){
				nonUsedProp = RandomProp();
				iterations++;
			}

			if(iterations >= Props.Length){
				bool found = false;
				for(var t = 0; t < Props.Length; t++){
					found = RandomProp();
					if(found){
						break;
					}
				}
			}
		}
	}

	private bool RandomProp(){
		int r = Random.Range(0, Props.Length);
		if(Props[r].activeSelf == false){
			Props[r].SetActive(true);
			return true;
		}else {
			return false;
		}
	}

}
