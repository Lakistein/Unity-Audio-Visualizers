using Assets.Scripts;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    const int BAND_8 = 8;
    AudioSource _audioSource;
    public static AudioBand AudioFrequencyBand8 { get; private set; }



    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioFrequencyBand8 = new AudioBand(BandCount.Eight);
    }

    void Update()
    {
        if (_audioSource.isPlaying)
        {
            AudioFrequencyBand8.Update((sample) =>
            {
                _audioSource.GetSpectrumData(sample, 0, FFTWindow.Blackman);
            });
        }
    }
}