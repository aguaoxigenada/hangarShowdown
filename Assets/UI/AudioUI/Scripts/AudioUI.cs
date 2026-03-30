using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider sliderVolMaster;

    void Start()
    {
        float value;
        mixer.GetFloat("VolMaster", out value);
        sliderVolMaster.value = DecibelToLinear(value);
    }

    public void SetVolMaster(float sliderValue)
    {
        mixer.SetFloat("VolMaster", LinearToDecibel(sliderValue));
    }

    public void SetVolMusica(float sliderValue)
    {
        mixer.SetFloat("VolMusica", LinearToDecibel(sliderValue));
    }

    public void SetVolSonido(float sliderValue)
    {
        mixer.SetFloat("VolSonido", LinearToDecibel(sliderValue));
    }

    private float DecibelToLinear(float dB)
    {
        float linear = Mathf.Pow(10.0f, dB / 20.0f);

        return linear;
    }

    private float LinearToDecibel(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }
}