using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class DeckManager : MonoBehaviour {
	private int totalChance;
	private Dictionary<int, Data_ActionCard> actionCards;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LoadActionCards(){
        StreamReader reader = new StreamReader("Assets/Data/actioncards.data");
        string[] line = null;
        string[] firstLine = reader.ReadLine().Split('\t');
		totalChance = 0;
		int curChance = 0;
		int manaCost = 0;
		int enemyHPLoss = 0;
		int enemyMPLoss = 0;
		int selfHPRegen = 0;
		int selfMPRegen = 0;
        while(!reader.EndOfStream){
            line = reader.ReadLine().Split('\t');
			Sprite icon = Resources.Load<Sprite>("Sprites/" + line[0] + ".png");
			totalChance += Convert.ToInt32(line[3]);
			manaCost = Convert.ToInt32(line[2]);
			enemyHPLoss = Convert.ToInt32(line[4]);
			enemyMPLoss = Convert.ToInt32(line[5]);
			selfHPRegen = Convert.ToInt32(line[6]);
			selfMPRegen = Convert.ToInt32(line[7]);
			Data_ActionCard card = new Data_ActionCard(line[0],icon,manaCost,enemyHPLoss,enemyMPLoss,selfHPRegen,selfMPRegen);
		}
	}
}
