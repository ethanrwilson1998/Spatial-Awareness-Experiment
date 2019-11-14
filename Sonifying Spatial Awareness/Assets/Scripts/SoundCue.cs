using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCue : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private float tempo;
    [SerializeField] private float pitch;
    [SerializeField] private float intensity;
    private float tempoModifier = 1;
    private float pitchModifier = 1;
    private float intensityModifier = 1;

    private void Awake()
    {
        source.pitch = pitch;
        source.volume = intensity;


        StartCoroutine(PlayCue());
    }

    private IEnumerator PlayCue()
    {
        while (true)
        {
            source.Stop();
            source.Play();
            yield return new WaitForSeconds(tempo * tempoModifier);
        }
    }

    public void SetTempo(float value)
    {
        tempoModifier = value;
    }

    public void SetPitch(float value)
    {
        pitchModifier = value;
        source.pitch = pitch * pitchModifier;
    }

    public void SetIntensity(float value)
    {
        intensityModifier = value;
        source.volume = intensity * intensityModifier;
    }
}
