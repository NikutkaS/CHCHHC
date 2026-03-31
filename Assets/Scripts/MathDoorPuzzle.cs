using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Door))]
public class MathDoorPuzzle : MonoBehaviour
{
    private static MathDoorPuzzle currentOpenPuzzle;

    [Header("Панель загадки")]
    [SerializeField] private GameObject puzzlePanel;
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_InputField answerInput;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Пример")]
    [SerializeField] private bool useRandomQuestion = true;
    [SerializeField] private int fixedLeftValue = 7;
    [SerializeField] private int fixedRightValue = 9;
    [SerializeField] private int minValue = 1;
    [SerializeField] private int maxValue = 9;

    [Header("Награда")]
    [SerializeField] private bool resetTimeOnSolve = true;

    private Door door;
    private bool solved;
    private Vector3 lastEchoPosition;
    private int leftValue;
    private int rightValue;
    private int correctAnswer;
    private ActionTrigger sourceTrigger;

    private void Awake()
    {
        door = GetComponent<Door>();
    }

    private void Start()
    {
        GenerateQuestion();
        HidePuzzle();
    }

    private void Update()
    {
        if (!IsPuzzleVisible())
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            SubmitAnswer();
    }

    public void OpenPuzzle()
    {
        OpenPuzzle(transform.position, null);
    }

    public void OpenPuzzle(Vector3 echoPosition)
    {
        OpenPuzzle(echoPosition, null);
    }

    public void OpenPuzzle(Vector3 echoPosition, ActionTrigger triggerSource)
    {
        if (solved)
            return;

        lastEchoPosition = echoPosition;
        sourceTrigger = triggerSource;
        currentOpenPuzzle = this;
        ShowPuzzle();
    }

    public void SubmitAnswer()
    {
        if (solved)
            return;

        if (answerInput == null)
            return;

        if (!int.TryParse(answerInput.text.Trim(), out int answer))
        {
            SetFeedback("Введите число");
            return;
        }

        if (answer != correctAnswer)
        {
            SetFeedback("Неверно");
            return;
        }

        SolvePuzzle();
    }

    public void ClosePuzzle()
    {
        HidePuzzle();

        if (currentOpenPuzzle == this)
            currentOpenPuzzle = null;

        if (!solved && sourceTrigger != null)
        {
            sourceTrigger.ResetTrigger();
            sourceTrigger = null;
        }
    }

    public void RegenerateQuestion()
    {
        if (solved)
            return;

        GenerateQuestion();
        ShowPuzzle();
    }

    public static void CloseCurrentOpenPuzzle(bool resetTrigger)
    {
        if (currentOpenPuzzle == null)
            return;

        currentOpenPuzzle.CloseFromSystem(resetTrigger);
    }

    private void CloseFromSystem(bool resetTrigger)
    {
        HidePuzzle();

        if (currentOpenPuzzle == this)
            currentOpenPuzzle = null;

        if (resetTrigger && !solved && sourceTrigger != null)
        {
            sourceTrigger.ResetTrigger();
            sourceTrigger = null;
        }
    }

    private void SolvePuzzle()
    {
        solved = true;
        SetFeedback("Открыто");

        if (door != null)
            door.Open();

        if (sourceTrigger != null)
        {
            sourceTrigger.CompleteTrigger();
            sourceTrigger = null;
        }

        if (currentOpenPuzzle == this)
            currentOpenPuzzle = null;

        HidePuzzle();

        if (resetTimeOnSolve && TimeManager.Instance != null)
            TimeManager.Instance.ActionCompleted(lastEchoPosition);
    }

    private void GenerateQuestion()
    {
        if (useRandomQuestion)
        {
            leftValue = Random.Range(minValue, maxValue + 1);
            rightValue = Random.Range(minValue, maxValue + 1);
        }
        else
        {
            leftValue = fixedLeftValue;
            rightValue = fixedRightValue;
        }

        correctAnswer = leftValue + rightValue;

        if (questionText != null)
            questionText.text = $"{leftValue} + {rightValue} = ?";

        SetFeedback(string.Empty);

        if (answerInput != null)
            answerInput.text = string.Empty;
    }

    private void ShowPuzzle()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(true);

        if (answerInput != null)
            StartCoroutine(FocusInputNextFrame());

        SetFeedback(string.Empty);
    }

    private IEnumerator FocusInputNextFrame()
    {
        yield return null;

        if (answerInput == null)
            yield break;

        answerInput.text = string.Empty;
        answerInput.Select();
        answerInput.ActivateInputField();
    }

    private void HidePuzzle()
    {
        if (puzzlePanel != null)
            puzzlePanel.SetActive(false);
    }

    private bool IsPuzzleVisible()
    {
        return puzzlePanel != null && puzzlePanel.activeInHierarchy;
    }

    private void SetFeedback(string message)
    {
        if (feedbackText != null)
            feedbackText.text = message;
    }
}
