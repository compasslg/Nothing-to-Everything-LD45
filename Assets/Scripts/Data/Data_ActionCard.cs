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
	int enemyMPLoss, int selfHPRegen, int selfMPRegen, bool evasion, int dmgBlock){
		this.cardName = cardName;
		this.icon = icon;
		this.manaCost = manaCost;
		this.enemyHPLoss = enemyHPLoss;
		this.enemyMPLoss = enemyMPLoss;
		this.selfHPRegen = selfHPRegen;
		this.selfMPRegen = selfMPRegen;
		this.dmgBlock = dmgBlock;
		this.evasion = evasion;
		this.description = "";
		if(enemyHPLoss > 0){
			this.description += "Enemy HP -" + enemyHPLoss + "\n"; 
		}
		if(enemyMPLoss > 0){
			this.description += "Enemy MP -" + enemyMPLoss + "\n";
		}
		if(selfHPRegen > 0){
			this.description += "Self HP +" + enemyMPLoss + "\n";
		}
		if(selfMPRegen > 0){
			this.description += "Self MP +" + enemyMPLoss + "\n";
		}
		if(evasion){
			this.description += "Dodge next attack\n";
		}
		if(dmgBlock > 0){
			this.description += "Next damage block: " + dmgBlock;
		}
	}
}
