using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ThingsController : MonoBehaviour {
	[System.Serializable]
	private class CardPrefebs{
		public GameObject actionCardPrefab;
		public GameObject thingPrefab;
	}
	[System.Serializable]
	private class Collections{
		public GameObject actionCards;
		public GameObject things;
	}
	[SerializeField]private CardPrefebs cardPrefabs;
	[SerializeField]private Collections collections;
	// Use this for initialization
	void Start () {
		LoadActionCards();
		LoadThings();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void LoadActionCards(){
		List<Data_ActionCard> actionCards = DeckManager.instance.GetAllActionCards();
		GameObject cardObj = null;
		foreach(Data_ActionCard card in actionCards){
			cardObj = Instantiate(cardPrefabs.actionCardPrefab, collections.actionCards.transform);
			cardObj.GetComponent<ActionCard>().SetData(card);
		}

	}

	public void LoadThings(){
		GameObject thingObj = null;
		foreach(DeckManager.Data_Thing thing in DeckManager.instance.things){
			thingObj = Instantiate(cardPrefabs.thingPrefab, collections.things.transform);
			thingObj.GetComponent<Thing>().SetData(thing.name, thing.description);
		}
	}
	public void ReturnToTitle(){
		AudioManager.instance.PlaySound("Button Click");
		SceneManager.LoadScene("Title");
	}
}
