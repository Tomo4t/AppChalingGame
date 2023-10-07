using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManger : MonoBehaviour
{
    public static TransitionManger instance;
    [SerializeField] Animator transitionAnim;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void loadlevel(bool LoadString, string Name)
    {
        if (LoadString)
        { StartCoroutine(LoadMnue(Name)); }
        else 
        { StartCoroutine(StratLoading()); }
        

    }
     IEnumerator StratLoading() 
    {
        transitionAnim.gameObject.SetActive(true);
        SoundManager.instance.MuteAll(1);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        LevelManager.GameStarted = false;
        MenuManager.currentLevelIndex++;
        if (SceneManager.GetSceneByName("Level" + (SceneManager.GetActiveScene().buildIndex + 1)) != null)
        {
            SceneManager.LoadScene("Level" + (SceneManager.GetActiveScene().buildIndex + 1));
        }
        else
        {
            SceneManager.LoadScene("Start");
        }
       
        
        SoundManager.instance.UnMuteAll(1);
        transitionAnim.SetTrigger("End");
        


    }
    IEnumerator LoadMnue(string name)
    {
        transitionAnim.gameObject.SetActive(true);
        SoundManager.instance.MuteAll(1);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        LevelManager.GameStarted = false;
        SceneManager.LoadScene(name);
        
        SoundManager.instance.UnMuteAll(1);
        transitionAnim.SetTrigger("End");


    }
}
