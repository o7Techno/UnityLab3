using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;
using FluentAssertions;

public class TestScoreListener
{
    private GameObject scoreListenerObj;
    private ScoreListener scoreListener;
    private TextMeshProUGUI tmp;
    private GameObject dummyGameActionsObj;

    [SetUp]
    public void Setup()
    {
        dummyGameActionsObj = new GameObject("GameActions", typeof(GameActions));
        GameActions gameActions = dummyGameActionsObj.GetComponent<GameActions>();

        scoreListenerObj = new GameObject("ScoreListener", typeof(ScoreListener));
        scoreListener = scoreListenerObj.GetComponent<ScoreListener>();
        tmp = scoreListenerObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "";
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(scoreListenerObj);
        Object.DestroyImmediate(dummyGameActionsObj);
    }

    [UnityTest]
    public IEnumerator UpdateScore_Event_ChangesText()
    {
        yield return 0;

        dummyGameActionsObj.GetComponent<GameActions>().score.Value = 100;

        float timeout = Time.time + 3f;
        yield return new WaitUntil(() => tmp.text.Contains("100") || Time.time > timeout);

        tmp.text.Should().Contain("100", "потому что текстовое поле должно обновляться при изменении счёта");
    }


}
