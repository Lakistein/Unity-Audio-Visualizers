using Assets.Scripts;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    const int BAND_8 = 8;
    AudioSource _audioSource;
    public static AudioBand AudioFrequencyBand8 { get; private set; }

    [DllImport("__Internal")]
    private static extern bool StartSampling(string name, float duration, int bufferSize);

    [DllImport("__Internal")]
    private static extern bool CloseSampling(string name);

    [DllImport("__Internal")]
    private static extern bool GetSamples(string name, float[] freqData, int size);

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioFrequencyBand8 = new AudioBand(BandCount.Eight);
        //if starting
        StartSampling(name, _audioSource.clip.length, 512);

    }

    void Update()
    {
        if (_audioSource.isPlaying)
        {
            AudioFrequencyBand8.Update((sample) =>
            {
#if UNITY_EDITOR
                _audioSource.GetSpectrumData(sample, 0, FFTWindow.Blackman);
#endif
#if UNITY_WEBGL && !UNITY_EDITOR

                StartSampling(name, _audioSource.clip.length, 512);
                GetSamples(name, sample, sample.Length);

#endif
            });
        }
    }
}