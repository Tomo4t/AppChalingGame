using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public Button Open,Close,BackToStartB,DeleatHestory,TutorialButten,SignLink,CloseTou;
    public GameObject TutorialPnael,settingsPanel;
    public string URL = "";
    public Slider Music, sound;
    public Toggle MuteMusic, MuteSound;
    public AudioSource SFX;
    public AudioClip Click;

    public static bool setingsActive;
    public static Settings Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Open.GetComponent<Button>().onClick.AddListener(() => TriggerSitings());
        Close.GetComponent<Button>().onClick.AddListener(() => TriggerSitings());

        DeleatHestory.GetComponent<Button>().onClick.AddListener(() => ResetAll());

        TutorialButten.GetComponent<Button>().onClick.AddListener(() => triggerTutorial());

        SignLink.GetComponent<Button>().onClick.AddListener(() => OpenLink());

        BackToStartB.GetComponent<Button>().onClick.AddListener(() => BackToStart());

        CloseTou.GetComponent<Button>().onClick.AddListener(() => BackToSettings());
    }


    public void playClickSound()
    {
        SFX.Play();
    }

    public void OpenLink() 
    {
        Application.OpenURL(URL);

    }

    public void TriggerSitings()
    {
        if (MenuManager.allowTouch == true)
        {
            MenuManager.allowTouch = false;
        }
        else
        {
            StartCoroutine( alowtoch());
        }
        setingsActive = !setingsActive;
        BackToStartB.gameObject.SetActive(!setingsActive);
        Open.gameObject.SetActive(!setingsActive);
        settingsPanel.gameObject.SetActive(setingsActive);
        
    }
    public void triggerTutorial()
    {
        CloseTou.gameObject.SetActive(true);
        settingsPanel.gameObject.SetActive(false);
        TutorialPnael.gameObject.SetActive(true);
    }
    public void BackToSettings()
    {
        CloseTou.gameObject.SetActive(false);
        TutorialPnael.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(true);
    }

    public void ResetAll()
    {
        setingsActive = false;
        MenuManager.currentLevelIndex = 0;
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SoundManager.instance.SetMusic();
        SoundManager.instance.SetSFX();

        SceneManager.LoadScene("Start");
    }
    private void BackToStart()
    {
        SceneManager.LoadScene("Start");
    }

    public IEnumerator alowtoch()
    {
        yield return new WaitForSeconds(0.5f);
        if (setingsActive == false)
        {
            MenuManager.allowTouch = true;
        }
       
    }

}
