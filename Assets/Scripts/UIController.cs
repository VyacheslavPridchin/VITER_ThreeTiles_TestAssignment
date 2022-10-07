using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Text gameResultText, volumeText;
    [SerializeField]
    private GameObject ResultScreen, VolumeScreen;
    [SerializeField]
    private Slider volumeSlider;
    public static UIController Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }

    public void ShowResult(string result)
    {
        gameResultText.text = result;
        ResultScreen.SetActive(true);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ChangeVolume()
    {
        volumeText.text = $"Звук: {volumeSlider.value}%";
        AudioManager.Singleton.ChangeVolume(volumeSlider.value);
    }

    public void CloseVolumeSettings()
    {
        VolumeScreen.SetActive(false);
    }
    public void ShowVolumeSettings()
    {
        volumeText.text = $"Звук: {AudioManager.Singleton.GetVolume()}%";
        volumeSlider.value = AudioManager.Singleton.GetVolume();
        VolumeScreen.SetActive(true);
    }
}
