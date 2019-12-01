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


    private Transform angleT;

    private float distanceFromPath;

    // Setting experiment type
    [System.Serializable] public enum ExperimentType
    {
        DecreaseTempo,
        IncreasePitch,
        DecreaseIntensity
    }
    private ExperimentType type;

    [System.Serializable] private struct VariableParameters
    {
        public float low;
        public float high;
        public float effectiveMaxDistance;
    }
    [SerializeField] private VariableParameters tempoParams;
    [SerializeField] private VariableParameters pitchParams;
    [SerializeField] private VariableParameters intensityParams;
    [SerializeField] private Transform next;

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
        type = SubjectInfo.testType;
        angleT = new GameObject().transform;

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
        if (next)
            next.position = path.Next();

        float distanceFromNext = (path.Next() - subject.position).magnitude;
        if (distanceFromNext < distanceCutoff)
        {
            if (!path.NextLine())
            {
                // experiment is done (they got to the last point)
                FinishExperiment();
                return;
            }
            
        }

        // smooth the soundcue transition so they can't hear it teleporting
        soundCue.transform.position = Vector3.SmoothDamp(
            soundCue.transform.position,
            path.Next(),
            ref refVel,
            0.2f);

        float distanceFromPath = path.Distance(subject.position);

        float value = 0f;
        float numerator = 0f;


        switch (type)
        {
            case ExperimentType.DecreaseTempo:
                numerator = Mathf.Clamp(distanceFromNext, 0.001f, tempoParams.effectiveMaxDistance);
                value = tempoParams.high - Mathf.Lerp(0, tempoParams.high - tempoParams.low, numerator / tempoParams.effectiveMaxDistance);
                soundCue.SetTempo(value);
                break;
            case ExperimentType.IncreasePitch:
                numerator = Mathf.Clamp(distanceFromNext, 0.001f, pitchParams.effectiveMaxDistance);
                value = pitchParams.low + Mathf.Lerp(0, pitchParams.high - pitchParams.low, numerator / pitchParams.effectiveMaxDistance);
                soundCue.SetPitch(value);
                break;
            case ExperimentType.DecreaseIntensity:
                numerator = Mathf.Clamp(distanceFromNext, 0.001f, intensityParams.effectiveMaxDistance);
                value = intensityParams.high - Mathf.Lerp(0, intensityParams.high - intensityParams.low, numerator / intensityParams.effectiveMaxDistance);
                soundCue.SetIntensity(value);
                break;
        }


        distanceTraveled += (subject.position - lastPosition).magnitude;

        timeTaken += Time.deltaTime;

        // write to csv if they moved
        //if ((subject.position - lastPosition).magnitude > 0.05f)
        {
            // compute viewing angle
            Vector3 forwardY = Vector3.Scale(subject.GetComponentInChildren<Camera>().transform.eulerAngles, new Vector3(0, 1, 0));

            angleT.position = subject.transform.position;
            angleT.LookAt(path.Next());



            Vector3 toWaypointY = Vector3.Scale(angleT.eulerAngles, new Vector3(0, 1, 0));

            float angleOffset = (forwardY - toWaypointY).y;
            while (angleOffset > 180)
                angleOffset -= 360;
            while (angleOffset < -180)
                angleOffset += 360;
            Debug.Log(angleOffset);
            raw.WriteLine(timeTaken + ", " + distanceFromPath + ", " + angleOffset);
        }

        lastPosition = subject.position;

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
        writer.WriteLine("Experiment: " + type.ToString());
        writer.WriteLine("Scene: " + SceneManager.GetActiveScene().name);
        writer.WriteLine("");

        writer.WriteLine("Direct Distance: " + pathDistance);
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
