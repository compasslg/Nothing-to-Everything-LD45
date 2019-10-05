using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void GoToScene(string sceneName){
		if(sceneName.Equals("Exit")){
			Application.Quit();
			return;
		}
		SceneManager.LoadScene(sceneName);
	}
}
