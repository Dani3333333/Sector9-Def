using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
    
public class TutorialTaskManager : MonoBehaviour
{
    public static TutorialTaskManager Instance;

    [System.Serializable]
    public class Task
    {
        public string name;
        public Toggle wallToggle;      // Toggle en el panel A
        public Toggle detailToggle;    // Toggle en el panel B
    }

    public List<Task> tasks = new List<Task>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        foreach (var task in tasks)
        {
            task.detailToggle.onValueChanged.AddListener((value) =>
            {
                task.wallToggle.isOn = value;
            });
        }
    }
}
