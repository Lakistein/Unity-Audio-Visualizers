using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightOnAudioBand : MonoBehaviour
{
    public int Band;
    public float minIntenisty, maxIntensity;
    Light _light;

    // Use this for initialization
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = AudioPeer.AudioFrequencyBand8.GetAudioBand(Band) * (maxIntensity - minIntenisty) + minIntenisty;
    }
}
