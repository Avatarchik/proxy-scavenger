using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii;
using com.ootii.Messages;

public class SpawnQualityItemOnShipComplete : MonoBehaviour {

	public GameObject[] BasicSpawnableObjects;
	public GameObject[] StandardSpawnableObjects;
	public GameObject[] SpecializedSpawnableObjects;
	public GameObject[] SuperiorSpawnableObjects;
	public GameObject[] HighEndSpawnableObjects;

	public bool forceObjects = false;
	public bool RandomlySpawnAtAll = false;
	public int RandomSpawnChance = 100;

	public Quality shipQuality = Quality.Basic;

	public GameObject SpawnedObject;
	public bool spawnedOBJ = false;

	private DungeonInfo di;

	// Use this for initialization
	void Awake () {
		di = GameObject.FindGameObjectWithTag("DungeonHead").GetComponent<DungeonInfo>();
		shipQuality = di.ShipQuality;
		//MessageDispatcher.AddListener("ShipDungeonComplete", OnComplete, true);
	}

	void Update(){
		if(!spawnedOBJ){
			if(di.GenerationComplete){
				SpawnObject();
				spawnedOBJ = true;
			}
		}
	}

	private void OnComplete(IMessage m){
		SpawnObject();
	}

	private void SpawnObject(){
		bool spawn = true;

		if(RandomlySpawnAtAll){
			int r = UnityEngine.Random.Range(0, 101);
			if(r < RandomSpawnChance){
				spawn = true;
			} else {
				spawn = false;
			}
		}
		if(spawn){
			GameObject spawnOBJ = RandomObject();

			SpawnedObject = GameObject.Instantiate(spawnOBJ ,this.transform.position, this.transform.rotation);

			Transform go = this.transform.parent;

			SpawnedObject.transform.parent = go;
			
		}
	}

	private GameObject RandomObject(){
		if(!forceObjects){
			switch(shipQuality){
			case Quality.Basic:
				int a = BasicSpawnableObjects.Length;
				int b = UnityEngine.Random.Range(0,a);
				return BasicSpawnableObjects[b];
				break;
			case Quality.Standard:
				int c = StandardSpawnableObjects.Length;
				int d = UnityEngine.Random.Range(0,c);
				return StandardSpawnableObjects[d];
				break;
			case Quality.Specialized:
				int e = SpecializedSpawnableObjects.Length;
				int f = UnityEngine.Random.Range(0,e);
				return SpecializedSpawnableObjects[f];
				break;
			case Quality.Superior:
				int g = SuperiorSpawnableObjects.Length;
				int h = UnityEngine.Random.Range(0,g);
				return SuperiorSpawnableObjects[h];
				break;
			case Quality.HighEnd:
				int i = HighEndSpawnableObjects.Length;
				int j = UnityEngine.Random.Range(0,i);
				return HighEndSpawnableObjects[j];
				break;
			default:
				return BasicSpawnableObjects[0];
				break;
			}
		} else {
			int a = BasicSpawnableObjects.Length;
			int b = UnityEngine.Random.Range(0,a);
			return BasicSpawnableObjects[b];
		}
	}
}
