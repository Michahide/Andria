using UnityEngine;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    public static float musicVolume;
    public static float soundEffectsVolume;

    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI soundEffectsSliderText;

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        Debug.Log(value);
        
        musicSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsVolume = value;

        soundEffectsSliderText.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMixerVolume();
    }
}