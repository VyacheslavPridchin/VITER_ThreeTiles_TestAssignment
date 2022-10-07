using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    public static AudioManager Singleton { get; private set; }

    private void Awake()
    {
        Singleton = this;
        DontDestroyOnLoad(this.gameObject);
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume / 100f;
        PlayerPrefs.SetFloat("Volume", audioSource.volume);
    }

    public float GetVolume()
    {
        return audioSource.volume * 100f;
    }
}
