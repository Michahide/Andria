using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioOptionsManager : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI musicValue;
    private TextMeshProUGUI soundEffectsValue;

    void Start()
    {
        musicValue = GameObject.Find("MusikValue").GetComponent<TextMeshProUGUI>();
        soundEffectsValue= GameObject.Find("SFXValue").GetComponent<TextMeshProUGUI>();
        slider = gameObject.GetComponent<Slider>();
        string temp = gameObject.name;
        slider.onValueChanged.AddListener(delegate { AttachCallback(temp); });
    }

    public void AttachCallback(string temp)
    {
        if (temp.CompareTo("MusikSlider") == 0)
        {
            OnMusicSliderValueChange(slider.value);
        }
        else if (temp.CompareTo("SFXSlider") == 0)
        {
            OnSoundEffectsSliderValueChange(slider.value);
        }
    }

    public void OnMusicSliderValueChange(float value)
    {
        musicValue.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateMusicMixerVolume(value);
    }

    public void OnSoundEffectsSliderValueChange(float value)
    {
        soundEffectsValue.text = ((int)(value * 100)).ToString();
        AudioManager.Instance.UpdateSFXMixerVolume(value);
    }
}