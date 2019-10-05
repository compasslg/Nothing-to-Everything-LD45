﻿// IN YOUR EDITOR FOLDER, have SimpleEditorUtils.cs.
 // paste in this text.
 // to play, HIT COMMAND-ZERO rather than command-P
 // (the zero key, is near the P key, so it's easy to remember)
 // simply insert the actual name of your opening scene
 // "__preEverythingScene" on the second last line of code below.
 
 using UnityEditor;
 using UnityEngine;
 using System.Collections;
 using UnityEditor.SceneManagement;
 [InitializeOnLoad]
 public static class CustomStart {
    // click command-0 to go to the prelaunch scene and then play
    [MenuItem("Edit/Play-Unplay %0")]
    public static void PlayFromPrelaunchScene(){
		if ( EditorApplication.isPlaying == true ){
			EditorApplication.isPlaying = false;
			return;
		}
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene(
					"Assets/Scenes/Preload.unity");
		EditorApplication.isPlaying = true;
    }
}