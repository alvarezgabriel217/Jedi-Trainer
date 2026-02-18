using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void PlayTraining()
    {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("TrainingArena");
    }

    public void PlayWave()
    {
        // Load the settings scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("WaveArena");
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }

}