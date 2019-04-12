using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour {

	public void ExitTheGame()
	{
		Debug.Log ("Has quit the game");
		Application.Quit();
	}
}
