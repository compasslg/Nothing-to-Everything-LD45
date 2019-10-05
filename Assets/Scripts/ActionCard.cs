using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ActionCard : MonoBehaviour{
	[System.Serializable]
	private class UIDisplay{
		public Text cardName;
		public Text manaCost;
		public Image icon;
		public Text description;
	}
	[System.Serializable]
	private class PlayMotion{
		public bool beingPlayed;
		public bool goingCenter;
		public bool goingGrave;
		public float moveSpeed;
		public float stayTime;
		public float t;
		public Vector3 initialPosition;
		public Vector3 motionVector;
	}
	
	[SerializeField]private UIDisplay display;
	[SerializeField]private PlayMotion playMotion;
	public Data_ActionCard data;
	public bool interactable;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(!playMotion.beingPlayed){
			return;
		}
		if(playMotion.t < 1){
			playMotion.t += playMotion.moveSpeed * Time.deltaTime;
		}else{
			gameObject.transform.localPosition = playMotion.initialPosition + playMotion.motionVector;
			playMotion.t = 0;
			
			// From going center to staying in center
			if(playMotion.goingCenter){
				playMotion.goingCenter = false;
			}
			// From going grave to finish playing
			else if(playMotion.goingGrave){
				playMotion.goingGrave = false;
				playMotion.beingPlayed = false;
			}
			// from staying in the center to going to the grave
			else{
				playMotion.goingGrave = true;
				playMotion.initialPosition = transform.localPosition;
			}
		}
	}

	public void SetData(Data_ActionCard data){
		this.data = data;
		display.cardName.text = data.cardName;
		display.manaCost.text = data.manaCost.ToString();
		display.icon.sprite = data.icon;
		display.description.text = data.description;
	}
	public void Play(){
		if(interactable && GameController.instance.curState == GameController.GameState.PLAY){
			gameObject.transform.SetParent(GameController.instance.transform);
			playMotion.initialPosition = gameObject.transform.localPosition;
			playMotion.motionVector = new Vector3() - this.transform.localPosition;
			playMotion.t = 0;
		}
	}
}
