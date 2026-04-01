using System;
using TMPro;
using UnityEngine;

public class LevelObjectiveManager : MonoBehaviour
{
    public static LevelObjectiveManager Instance { get; private set; }

    public event Action<string> OnObjectiveChanged;

    [System.Serializable]
    public class ObjectiveStep
    {
        [TextArea(2, 4)]
        public string description;
    }

    [Header("UI")]
    [SerializeField] private TMP_Text currentObjectiveText;

    [Header("Objectives")]
    [SerializeField] private ObjectiveStep[] steps;
    [SerializeField] private int startStepIndex = 0;

    private int currentStepIndex;

    public string CurrentObjective => GetCurrentObjectiveText();
    public int CurrentStepIndex => currentStepIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetStep(startStepIndex);
    }

    public void SetStep(int index)
    {
        if (steps == null || steps.Length == 0)
        {
            currentStepIndex = 0;
            UpdateObjectiveText(string.Empty);
            return;
        }

        index = Mathf.Clamp(index, 0, steps.Length - 1);
        currentStepIndex = index;
        UpdateObjectiveText(GetCurrentObjectiveText());
    }

    public void CompleteCurrentObjective()
    {
        if (steps == null || steps.Length == 0)
            return;

        if (currentStepIndex < steps.Length - 1)
        {
            SetStep(currentStepIndex + 1);
            return;
        }

        currentStepIndex = -1;
        UpdateObjectiveText(string.Empty);
    }

    public void RefreshObjectiveText()
    {
        UpdateObjectiveText(GetCurrentObjectiveText());
    }

    private string GetCurrentObjectiveText()
    {
        if (steps == null || steps.Length == 0)
            return string.Empty;

        if (currentStepIndex < 0 || currentStepIndex >= steps.Length)
            return string.Empty;

        return steps[currentStepIndex] != null ? steps[currentStepIndex].description : string.Empty;
    }

    private void UpdateObjectiveText(string text)
    {
        if (currentObjectiveText != null)
            currentObjectiveText.text = text;

        OnObjectiveChanged?.Invoke(text);
    }
}
