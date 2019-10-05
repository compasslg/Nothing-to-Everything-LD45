using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ThingsController : MonoBehaviour {
	[System.Serializable]
	private class CardPrefebs{
		public GameObject actionCardPrefab;
		public GameObject thingCardPrefab;
	}
	[System.Serializable]
	private class Collections{
		public GameObject actionCards;
		public GameObject thingCards;
	}
	[SerializeField]private CardPrefebs cardPrefebs;
	[SerializeField]private Collections collections;
	// Use this for initialization
	void Start () {
		LoadActionCards();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void LoadActionCards(){
		List<Data_ActionCard> actionCards = DeckManager.instance.GetAllActionCards();
		GameObject cardObj = null;
		foreach(Data_ActionCard card in actionCards){
			cardObj = Instantiate(cardPrefebs.actionCardPrefab, collections.actionCards.transform);
			cardObj.GetComponent<ActionCard>().SetData(card);
		}

	}
	public void ReturnToTitle(){
		SceneManager.LoadScene("Title");
	}
}
