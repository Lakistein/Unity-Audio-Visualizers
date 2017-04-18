using UnityEngine;
using System.Collections;

public class CameraShakeOnBand0 : MonoBehaviour
{
    public bool debugMode = false;
    Vector3 startRotation;

    public float maxAmount;
    public float shakeDurationMax;
    public float bassTreshold;
    public float shakeAmount;
    public float shakeDuration;

    float shakePercentage;
    float startAmount;
    float startDuration;
    bool isRunning = false;


    public bool smooth;
    public float smoothAmount = 5f;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles;
        if (debugMode) ShakeCamera();
    }

    private void Update()
    {
        float bass = AudioPeer.AudioFrequencyBand8.GetAudioBand(0, true);
        print(AudioPeer.AudioFrequencyBand8.GetAmplitude(true));
        float amount = bass > bassTreshold ? bass : 0;

        ShakeCamera(amount * shakeAmount, Mathf.Max(bass - bassTreshold, 0));
    }

    void ShakeCamera()
    {
        startAmount = shakeAmount;
        startDuration = shakeDuration;

        if (!isRunning) StartCoroutine(Shake());
    }

    public void ShakeCamera(float amount, float duration)
    {
        smoothAmount = Mathf.Pow(amount, 2);
        shakeAmount += amount;
        shakeAmount = Mathf.Min(shakeAmount, maxAmount);
        startAmount = shakeAmount;
        shakeDuration += duration;
        shakeDuration = Mathf.Min(shakeDuration, shakeDurationMax);

        startDuration = shakeDuration;

        if (!isRunning) StartCoroutine(Shake());
    }


    IEnumerator Shake()
    {
        isRunning = true;

        while (shakeDuration > 0)
        {
            Vector3 rotationAmount = Random.insideUnitSphere * shakeAmount;
            rotationAmount.x = startRotation.x + rotationAmount.x;
            rotationAmount.y = startRotation.y + rotationAmount.y;
            rotationAmount.z = startRotation.z;

            shakePercentage = shakeDuration / startDuration;

            shakeAmount = startAmount * shakePercentage;
            shakeDuration -= Time.deltaTime;


            if (smooth)
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rotationAmount), Time.deltaTime * smoothAmount);
            else
                transform.localRotation = Quaternion.Euler(rotationAmount);

            yield return null;
        }
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(startRotation));
        isRunning = false;
    }
}