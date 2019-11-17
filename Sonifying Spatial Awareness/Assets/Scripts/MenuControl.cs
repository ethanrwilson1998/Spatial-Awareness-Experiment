using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private InputField name;

    private void Awake()
    {
        name.text = SubjectInfo.name;
    }

    private void Update()
    {
        SubjectInfo.name = name.text;
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
