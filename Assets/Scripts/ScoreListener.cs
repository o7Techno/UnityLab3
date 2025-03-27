using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreListener : MonoBehaviour
{
    ObservableInt score;

    [SerializeField]
    TextMeshProUGUI text;

    GameActions gameActions;
    private void Awake()
    {
        gameActions = FindAnyObjectByType<GameActions>();
        if (gameActions)
        {
            score = gameActions.score;
        }
        //else
        //{
        //    Debug.LogError("Game Actions not found.");
        //}
        text = GetComponent<TextMeshProUGUI>();

        //if (!TryGetComponent<TextMeshProUGUI>(out text))
        //{
        //    Debug.LogError("Score field not found");
        //}

        score.OnValueChanged += UpdateScore;
    }

    void UpdateScore(int score)
    {
        text.text = $"Score: {score}";
    }
}
