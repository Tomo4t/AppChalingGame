using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    public Slider Music_Slider, SFX_Slider;
    public Toggle Music, SFX;
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        if (Music != null) {
            if (PlayerPrefs.GetInt("MuteMusic") == 1)
            {
                Music.isOn = true;
                MuteMuisc();
            }
            else
            {
                Music.isOn = false;
                UnMuteMuisc();
            }
        }
        if(SFX != null) 
        { 
            if (PlayerPrefs.GetInt("MuteSFX") == 1)
            {
                SFX.isOn = true;
                MuteSFX();
            }
            else
            {
                SFX.isOn = false;
                UnMuteSFX();
            }
        }
        if (Music_Slider != null)
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                LoadMusicVolume();
            }
            else
            {
                SetMusic();
            }
                
        }

        if (SFX_Slider != null)
        {
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                LoadSFXVolume();
            }
            else
            {
                SetSFX();
            }
            
        }
    }

    public void MuteMuisc()
    {
        bool isOn = Music.isOn;
        if (isOn == true)
        {
           
            m_AudioMixer.SetFloat("MuteM", -80.0f);
            PlayerPrefs.SetInt("MuteMusic", 1);
        }
    }
    public void UnMuteMuisc()
    {
        bool isOn = Music.isOn;
        if (isOn == false)
        {
            
            m_AudioMixer.SetFloat("MuteM", 0.0f);
            PlayerPrefs.SetInt("MuteMusic", 0);
        }
    }

    public void MuteSFX()
    {
        bool isOn = SFX.isOn;
        if (isOn == true)
        {
            
            m_AudioMixer.SetFloat("MuteS", -80.0f);
            PlayerPrefs.SetInt("MuteSFX", 1);
        }
    }
    public void UnMuteSFX()
    {
        bool isOn = SFX.isOn;
        if (isOn == false)
        {
            
            m_AudioMixer.SetFloat("MuteS", 0.0f);
            PlayerPrefs.SetInt("MuteSFX", 0);
        }
    }

    public void SetMusic() 
    {
        float volum = Music_Slider.value;
        m_AudioMixer.SetFloat("Music",Mathf.Log10(volum)*20);
        PlayerPrefs.SetFloat("MusicVolume", volum);
    }
    private void LoadMusicVolume()
    {
        Music_Slider.value = PlayerPrefs.GetFloat("MusicVolume");
       
        SetMusic();
        
    }
    private void LoadSFXVolume()
    {
       
        SFX_Slider.value = PlayerPrefs.GetFloat("SFXVolume");
        
        SetSFX();
    }


    public void SetSFX()
    {
        float volum = SFX_Slider.value;
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volum) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volum);
    }

    public void MuteAll(float time)
    {
        StartCoroutine(MuteMainGroupGradually(time));
    }
    public void UnMuteAll(float time)
    {
        StartCoroutine(UnmuteMainGroupGradually(time));
    }

    public IEnumerator MuteMainGroupGradually(float fadeDuration)
    {
        float elapsedTime = 0;
       
        float targetVolume = -80.0f; 

        while (elapsedTime < fadeDuration)
        {
            float newVolume = Mathf.Lerp(0, targetVolume, elapsedTime / fadeDuration);
            m_AudioMixer.SetFloat("MasterVolume", Mathf.Log10(newVolume) * 20);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_AudioMixer.SetFloat("MasterVolume", Mathf.Log10(targetVolume) * 20);
        
    }

    // Coroutine to gradually unmute the main group
    public IEnumerator UnmuteMainGroupGradually(float fadeDuration)
    {
      
        m_AudioMixer.SetFloat("MasterVolume", 1);
        yield return null;
    }



}
