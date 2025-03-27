using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.TestTools;
using System.Reflection;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using FluentAssertions;

public class TestInputManager
{
    private GameObject inputManagerObj;
    private InputManager inputManager;
    private bool actionCalled;

    [SetUp]
    public void Setup()
    {
        inputManagerObj = new GameObject("InputManager", typeof(InputManager));
        inputManager = inputManagerObj.GetComponent<InputManager>();
        actionCalled = false;
    }

    [TearDown]
    public void Teardown()
    {
        UnityEngine.Object.DestroyImmediate(inputManagerObj);
    }

    [Test]
    public void HandleInput_DoesNotInvokeAction_WhenMovingCellAmountIsGreaterThanZero()
    {
        InputManager.movingCellAmount = 1;
        MethodInfo method = typeof(InputManager).GetMethod("HandleInput", BindingFlags.Instance | BindingFlags.NonPublic);
        Action testAction = () => { actionCalled = true; };
        method.Invoke(inputManager, new object[] { testAction });

        actionCalled.Should().BeFalse("потому что HandleInput не должен вызывать действие, если movingCellAmount > 0");

        InputManager.movingCellAmount = 0;
    }
}
