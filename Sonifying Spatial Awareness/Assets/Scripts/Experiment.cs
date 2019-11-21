using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // data for analysis
    private float pathDistance;
    private float distanceTraveled;
    private Vector3 lastPosition;

    private float pathTime;
    private float timeTaken;


    private Vector3 refVel;

    private StreamWriter raw;

    private void Awake()
    {
        soundCue.transform.position = path.Next();

        pathDistance = path.TotalDistance();
        lastPosition = subject.position;

        pathTime = pathDistance / subject.GetComponent<Movement>().GetSpeed();

        // csv file to record raw data
        string filepath = "TestResults/" + SubjectInfo.name;
        string csvFile = "/" + SceneManager.GetActiveScene().name + ".csv";
        Directory.CreateDirectory(filepath);
        if (File.Exists(filepath + csvFile))
        {
            File.Delete(filepath + csvFile);
        }

        raw = File.CreateText(filepath + csvFile);
        raw.WriteLine("Time, Distance From Path, Angle From Next Waypoint");

    }

    private void Update()
    {
        float distanceFromNext = (path.Next() - subject.position).magnitude;
        if (distanceFromNext < distanceCutoff)
        {
            if (!path.NextLine())
            {
                // experiment is done (they got to the last point)
                FinishExperiment();
            }
            
        }

        // smooth the soundcue transition so they can't hear it teleporting
        soundCue.transform.position = Vector3.SmoothDamp(
            soundCue.transform.position,
            path.Next(),
            ref refVel,
            0.2f);

        float distanceFromPath = path.Distance(subject.position);

        float value;

        switch (type)
        {
            case ExperimentType.DecreaseTempo:
                value = tempoParams.high - Mathf.Lerp(0, tempoParams.high - tempoParams.low, distanceFromPath / tempoParams.effectiveMaxDistance);
                soundCue.SetTempo(value);
                break;
            case ExperimentType.IncreasePitch:
                value = pitchParams.low + Mathf.Lerp(0, pitchParams.high - pitchParams.low, distanceFromPath / pitchParams.effectiveMaxDistance);
                soundCue.SetPitch(value);
                break;
            case ExperimentType.IncreaseIntensity:
                value = intensityParams.low + Mathf.Lerp(0, intensityParams.high - intensityParams.low, distanceFromPath / intensityParams.effectiveMaxDistance);
                soundCue.SetIntensity(value);
                break;
        }


        distanceTraveled += (subject.position - lastPosition).magnitude;
        lastPosition = subject.position;

        timeTaken += Time.deltaTime;

        // write to csv if they moved
        if (distanceTraveled > 0)
        {
            // compute viewing angle
            Vector3 forwardY = Vector3.Scale(subject.GetComponentInChildren<Camera>().transform.forward, new Vector3(0, 1, 0));
            Vector3 toWaypointY = Vector3.Scale((path.Next() - subject.transform.position), new Vector3(0, 1, 0));

            float angleOffset = Vector3.Angle(forwardY, toWaypointY);
            raw.WriteLine(timeTaken + ", " + distanceFromPath + ", " + angleOffset);
        }
    }

    private void FinishExperiment()
    {
        RecordResults();

        raw.Close();

        SceneManager.LoadScene("Menu");
    }

    private void RecordResults()
    {
        string filepath = "TestResults/" + SubjectInfo.name;
        string file = "/" + SceneManager.GetActiveScene().name + ".txt";


        Directory.CreateDirectory(filepath);
        StreamWriter writer = new StreamWriter(filepath + file, true);
        writer.WriteLine("--------------------");
        writer.WriteLine(System.DateTime.Now);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine("Testing: " + SubjectInfo.name);
        writer.WriteLine("Experiment: " + SceneManager.GetActiveScene().name);
        writer.WriteLine("");

        writer.WriteLine("Ideal Distance: " + pathDistance);
        writer.WriteLine("Ideal Time: " + pathTime);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine("Subject's Distance: " + distanceTraveled);
        writer.WriteLine("Subject's Time: " + timeTaken);
        writer.WriteLine("");
        writer.WriteLine("");

        writer.WriteLine("Path Efficiency: " + 100 * pathDistance / distanceTraveled + "%");
        writer.WriteLine("Time Efficiency: " + 100 * pathTime / timeTaken + "%");

        writer.Close();
    }
}
