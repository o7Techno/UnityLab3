using NUnit.Framework;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.TestTools;
using FluentAssertions;
using System.Reflection;

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

        scoreListenerObj = new GameObject("ScoreListener");
        scoreListenerObj.SetActive(false);
        scoreListener = scoreListenerObj.AddComponent<ScoreListener>();
        FieldInfo field = typeof(ScoreListener).GetField("gameActions", BindingFlags.Instance | BindingFlags.NonPublic);
        field.SetValue(scoreListener, gameActions);
        scoreListenerObj.SetActive(true);
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
        dummyGameActionsObj.GetComponent<GameActions>().score.Value = 100;
        yield return new WaitUntil(() => tmp.text.Contains("100"));

        tmp.text.Should().Contain("100", "потому что текстовое поле должно обновляться при изменении счёта");
    }


}
