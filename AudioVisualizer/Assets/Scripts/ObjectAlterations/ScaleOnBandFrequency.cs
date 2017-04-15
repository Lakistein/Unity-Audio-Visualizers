public class ScaleOnBandFrequency : ScaleOnBase
{
    public int Band = 1;
    void Update()
    {
        UpdateScale(AudioPeer.AudioFrequencyBand8.GetFrequencyBand(1, UseBuffer));
    }
}
