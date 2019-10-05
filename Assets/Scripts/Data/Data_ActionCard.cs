using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_ActionCard {
	public readonly string cardName;
	public readonly Sprite icon;
	public readonly string description;
	public readonly int manaCost;
	public readonly int enemyHPLoss, enemyMPLoss, selfHPRegen, selfMPRegen;
	public readonly bool evasion;
	public readonly int dmgBlock;
	public Data_ActionCard(string cardName, Sprite icon, int manaCost, int enemyHPLoss, 
	int enemyMPLoss, int selfHPRegen, int selfMPRegen, int evasion, int dmgBlock){
		this.cardName = cardName;
		this.icon = icon;
		this.manaCost = manaCost;
		this.enemyHPLoss = enemyHPLoss;
		this.enemyMPLoss = enemyMPLoss;
		this.selfHPRegen = selfHPRegen;
		this.selfMPRegen = selfMPRegen;
		this.dmgBlock = dmgBlock;
		if(evasion == 0){
			this.evasion = false;
		}else{
			this.evasion = true;
		}
	}
}
