
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    public Button Qgame, BummpyRoads;
    void Start()
    {
        BummpyRoads.GetComponent<Button>().onClick.AddListener(() => StartBummpyRoads());
        Qgame.GetComponent<Button>().onClick.AddListener(() => StartqGame());
    }
    public void StartBummpyRoads()
    {
        TransitionManger.instance.loadlevel(true,"Menu");
    }

    public void StartqGame()
    {
        TransitionManger.instance.loadlevel(true, "QgMenu");
        
    }

}
