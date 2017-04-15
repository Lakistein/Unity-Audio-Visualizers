public class ScaleOnAmplitude : ScaleOnBase
{
    void Update()
    {
        UpdateScale(AudioPeer.AudioFrequencyBand8.GetAmplitude(UseBuffer));
    }
}
