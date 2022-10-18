using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//^^^ for changing scenes and such


public class MainMenu : MonoBehaviour
{
 

    public static void PlayGame()
    {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


    public static void PlayGame(string scene)
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        SceneManager.LoadScene(scene);
    }

    /*
    public static IEnumerator PlayGameAsync(string scene)
    {
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            Debug.Log(asyncLoad.progress);
            yield return null;
        }
        yield return asyncLoad;
    }
    */


    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void ChangeVolume(float volume)
    {

        AudioListener.volume = volume;
    }


}
