﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
public class DeckManager : MonoBehaviour {
	[Serializable]
	public class Data_Thing{
		public string name;
		public string description;
	}
	[Serializable]
	public class Data_ThingCard{
		public string name;
		public string description;
	}
	private int totalChance;
	private List<int> chances;
	private List<Data_ActionCard> actionCards;
	public List<Data_Thing> things;
	public static DeckManager instance;
	// Use this for initialization
	void Start () {
		instance = this;
		LoadActionCards();
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene("Title");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Data_Thing GetRandomThing(){
		int index = UnityEngine.Random.Range(0, things.Count);
		return things[index];
	}
	public Data_ActionCard DealActionCard(){
		int roll = UnityEngine.Random.Range(0, totalChance);
		for(int i = 0; i < chances.Count; i++){
			if(roll < chances[i]){
				return actionCards[i];
			}
		}
		return null;
		
	}
	public List<Data_ActionCard> GetAllActionCards(){
		return actionCards;
	}
	private void LoadActionCards(){
		chances = new List<int>();
		actionCards = new List<Data_ActionCard>();
        StreamReader reader = new StreamReader("Assets/Data/actioncards.data");
        string[] line = null;
        reader.ReadLine().Split('\t');
		totalChance = 0;
		int manaCost = 0;
		int enemyHPLoss = 0;
		int enemyMPLoss = 0;
		int selfHPRegen = 0;
		int selfMPRegen = 0;
		bool evasion = false;
		int block = 0;
        while(!reader.EndOfStream){
            line = reader.ReadLine().Split('\t');
			Sprite icon = Resources.Load<Sprite>("Sprites/" + line[0]);
			//Sprite icon = Resources.Load<Sprite>("Sprites/white.png");
			totalChance += Convert.ToInt32(line[3]);
			manaCost = Convert.ToInt32(line[2]);
			enemyHPLoss = Convert.ToInt32(line[4]);
			enemyMPLoss = Convert.ToInt32(line[5]);
			selfHPRegen = Convert.ToInt32(line[6]);
			selfMPRegen = Convert.ToInt32(line[7]);
			block = Convert.ToInt32(line[9]);
			if(Convert.ToInt32(line[8]) == 0){
				evasion = false;
			}else{
				evasion = true;
			}
			Data_ActionCard card = new Data_ActionCard(line[0],icon, line[1], manaCost,enemyHPLoss,enemyMPLoss,selfHPRegen,selfMPRegen, evasion, block);
			actionCards.Add(card);
			chances.Add(totalChance);
		}
	}
}
