using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreGrabber : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TMP_InputField inputName;
    private int score;

    public UnityEvent<string, int> OnSubmit;
    void Start()
    {
        ScoreTracker.UpdateScoreUI += OnNewScore;
    }

    private void OnDisable()
    {
        ScoreTracker.UpdateScoreUI += OnNewScore;
    }

    private void OnNewScore(int arg1, int arg2, int arg3, int arg4, float arg5)
    {
        if (scoreField != null)
            scoreField.text = arg1.ToString();

        score = arg1;
    }

    public void Submit()
    {
        if (inputName.text.Trim() == string.Empty)
            inputName.text = "AAA";

        OnSubmit.Invoke(inputName.text, int.Parse(scoreField.text));
    }
}
