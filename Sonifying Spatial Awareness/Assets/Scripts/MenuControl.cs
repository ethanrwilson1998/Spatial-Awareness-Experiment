using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private InputField name;
    [SerializeField] private Dropdown testType;

    private void Awake()
    {
        name.text = SubjectInfo.name;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        SubjectInfo.name = name.text;
        switch (testType.options[testType.value].text)
        {
            case "Intensity":
                SubjectInfo.testType = Experiment.ExperimentType.DecreaseIntensity;
                break;
            case "Pitch":
                SubjectInfo.testType = Experiment.ExperimentType.IncreasePitch;
                break;
            case "Tempo":
                SubjectInfo.testType = Experiment.ExperimentType.DecreaseTempo;
                break;
        }
    }

    public void GoToScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
