using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experiment : MonoBehaviour
{
    // references to components
    [SerializeField] private Transform subject;
    [SerializeField] private Path path;
    [SerializeField] private SoundCue soundCue; // probably attached to next point or subject?
    // tweakable values
    [SerializeField] private float distanceCutoff;
    
    private float distanceFromPath;

    // Setting experiment type
    [System.Serializable] private enum ExperimentType
    {
        DecreaseTempo,
        IncreasePitch,
        IncreaseIntensity
    }
    [SerializeField] private ExperimentType type;

    [System.Serializable] private struct VariableParameters
    {
        public float low;
        public float high;
        public float effectiveMaxDistance;
    }
    [SerializeField] private VariableParameters tempoParams;
    [SerializeField] private VariableParameters pitchParams;
    [SerializeField] private VariableParameters intensityParams;

    private void Awake()
    {
        soundCue.transform.position = path.Next();
    }

    private void Update()
    {
        float distanceFromNext = (path.Next() - subject.position).magnitude;
        if (distanceFromNext < distanceCutoff)
        {
            path.NextLine();
            soundCue.transform.position = path.Next();
        }

        float distanceFromPath = path.Distance(subject.position);

        float value;

        switch (type)
        {
            case ExperimentType.DecreaseTempo:
                value = tempoParams.high - Mathf.Lerp(0, tempoParams.high - tempoParams.low, distanceFromPath / tempoParams.effectiveMaxDistance);
                soundCue.SetTempo(value);
                break;
            case ExperimentType.IncreasePitch:
                value = tempoParams.low + Mathf.Lerp(0, tempoParams.high - tempoParams.low, distanceFromPath / tempoParams.effectiveMaxDistance);
                soundCue.SetPitch(value);
                break;
            case ExperimentType.IncreaseIntensity:
                value = tempoParams.low + Mathf.Lerp(0, tempoParams.high - tempoParams.low, distanceFromPath / tempoParams.effectiveMaxDistance);
                soundCue.SetIntensity(value);
                break;
        }



    }
}
