using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeriousGameComponents.QuizComponent;
using UnityEngine.UI;

public class QuizFieldTests
{
    QuizField testSubject;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject("test subject");
        testSubject = go.AddComponent<QuizField>();
        testSubject.autodetectProperties = false;

        GameObject child;
        child = new GameObject("Label Text");
        child.transform.parent = go.transform;
        child.AddComponent<Text>();
        child = new GameObject("Text");
        child.transform.parent = go.transform;
        child.AddComponent<Text>();
    }

    [Test]
    public void DetectPropertiesTest()
    {
        testSubject.text = null;
        testSubject.labelText = null;

        LogAssert.NoUnexpectedReceived();
        testSubject.DetectProperties();

        Assert.IsNotNull(testSubject.text);
        Assert.IsNotNull(testSubject.labelText);
    }


    [Test]
    public void InstantiatePropertiesTest()
    {
        GameObject go = new GameObject();
        QuizField qf = go.AddComponent<QuizField>();

        qf.InstantiateProperties();

        Transform childLabelText = go.transform.Find("Label Text");
        Transform childText = go.transform.Find("Text");

        Assert.IsNotNull(childLabelText);
        Assert.IsNotNull(childLabelText.GetComponent<Text>());
        Assert.IsNotNull(childText);
        Assert.IsNotNull(childText.GetComponent<Text>());
    }
}
