using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

	public void Quit()
	{
		Application.Quit();
	}

	public void Play()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);	
	}
}
