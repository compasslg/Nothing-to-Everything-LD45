using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StoryController : MonoBehaviour {
	[System.Serializable]
	public class Story{
		public Sprite img;
		public string text;
	}
	public List<Story> story;
	private int index;
	private int transitionState;
	private float t;
	[SerializeField]private Image background;
	[SerializeField]private Text text;
	// Use this for initialization
	void Start () {
		SceneManager.LoadScene("InGame");
		index = 0;
		t = 0;
		background.sprite = story[0].img;
		background.color = Color.black;
		transitionState = 2;
	}
	
	// Update is called once per frame
	void Update () {
		// fade
		if(transitionState == 1){
			t += Time.deltaTime;
			if(t > 1){
				t = 0;
				background.color = Color.black;
				transitionState++;
				// enter game
				if(index == story.Count){
					SceneManager.LoadScene("InGame");
					return;
				}
				background.sprite = story[index].img;
			}else{
				background.color = (1 - t) * Color.white + t * Color.black;
			}
			return;
		}
		// reappear
		else if(transitionState == 2){
			t += Time.deltaTime;
			if(t > 1){
				t = 0;
				background.color = Color.white;
				transitionState = 0;
				text.text = story[index].text;
			}else{
				background.color = (1 - t) * Color.black + t * Color.white;
			}
			return;
		}
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)){
			transitionState = 1;
			index++;
			text.text = "";
		}
	}
}
