using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //game manager has two methods, game over win and game over lose
    //depending on which one is triggered the game will be paused while the 
    //game over animation starts and triggers the game over screen to drop
    //down off the screen with several options to end hte game.


    //THE BLOODY GAME IS AUTO LOADING RESTART LEVEL AS SOON AS THE ANIMATION IS LOADED ON THE GAME
    //I HAVE TO SOMEHOW PAUSE EVERYTHING IN GAME EXCEPT THE ANIMATION ONTHE BACKGROUND IN THE BACKGROUND WHEN THE 

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitTheGame()
	{
		Debug.Log ("Has Quit the game");
		Application.Quit();
	}
}
