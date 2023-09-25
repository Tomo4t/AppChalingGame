using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menumanager : MonoBehaviour
{
    public void Onsign()
    {
        SceneManager.LoadScene("SIGN");
    }
public void OnMean()
    {
        SceneManager.LoadScene("Mean");
    }
public void OnGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Onhome()
    {
        SceneManager.LoadScene("Start");
    }
    public void OnExit()
    {
        Application.Quit();
    }
    public void Scenelood()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
