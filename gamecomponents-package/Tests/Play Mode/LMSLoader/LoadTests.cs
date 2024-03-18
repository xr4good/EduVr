using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using SeriousGameComponents.AudioPlayerComponent;
using SeriousGameComponents.SlideComponent;
using SeriousGameComponents.QuizComponent;
using SeriousGameComponents.LmsComponent;
using UnityEngine.UI;

public class LoadTests
{
    QuizController quizTestSubject;
    SlideController slideTestSubject;
    AudioPlayer audioTestSubject;
    LmsLoader testSubject;

    string loginUsername = "testUsername@test.com";
    string loginPassword = "testPassword123";

    [SetUp]
    public void Setup()
    {
        if (LmsLoader.singleton == null)
        {
            GameObject go = new GameObject("TEST SUBJECT");
            testSubject = go.AddComponent<LmsLoader>();
        }
        else
        {
            testSubject = LmsLoader.singleton;
        }

    }

    public IEnumerator RequireAssets()
    {
        if (!testSubject.loginSuccessful && !testSubject.loggingIn)
            testSubject.SubmitLogin(loginUsername,loginPassword);

        while (testSubject.loggingIn)
            yield return null;

        if (testSubject.loginException != null)
            Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");

        if (!testSubject.fetchingContent && testSubject.contentFetched)
            testSubject.GetContent();

        while (!testSubject.contentFetched && testSubject.fetchingContentException == null)
            yield return null;

        if (testSubject.fetchingContentException != null)
            Assert.Inconclusive("Inconclusive test! Could not fetch contents for logged user. Maybe it's a problem in the LMS");

        int lessonIndex = -1;
        int counter = 0;
        foreach (string lessonTitle in testSubject.lessonList)
        {
            if (lessonTitle == "Test Lesson 3D lab")
            {
                lessonIndex = counter;
                break;
            }
            counter++;
        }

        if (lessonIndex == -1)
        {
            Assert.Inconclusive("Inconclusive test. Could not find test lesson registered for this test user in the LMS");
        }

        if (!LmsLoader.quizFetched)
            testSubject.DownloadLessonAssets(lessonIndex);

        while (!LmsLoader.slidesFetched)
            yield return null;

        /*
        while (!LmsLoader.audiosFetched)
            yield return null

        while (!LmsLoader.videosFetched)
            yield return null
         */
    }

    [UnityTest]
    public IEnumerator QuizComponentLMSLoadTest()
    {
        GameObject go = new GameObject("Quiz");
        QuizController qc = go.AddComponent<QuizController>();
        qc.identifier = LmsLoader.DEFAULT_IDENTIFIER;
        SetQuizEnvironment(qc);
        qc.useLmsLoader = true;

        yield return RequireAssets();

        yield return null;

        Assert.IsTrue(qc.questions.Count == 1);
        Assert.AreEqual("Test 3D Quiz", qc.quizTitle);
        Assert.AreEqual("Single question Quiz", qc.quizDescription);
        Assert.IsTrue(qc.answerFields.Count == 4);

        Assert.AreEqual("Yes", qc.questions[0].answers[0].answer);
        Assert.IsTrue (qc.questions[0].answers[0].isCorrect);

        Assert.AreEqual("No", qc.questions[0].answers[1].answer);
        Assert.IsFalse(qc.questions[0].answers[1].isCorrect);

        Assert.AreEqual("I need explore more", qc.questions[0].answers[2].answer);
        Assert.IsFalse(qc.questions[0].answers[2].isCorrect);

        Assert.AreEqual("I don't need use it", qc.questions[0].answers[3].answer);
        Assert.IsFalse(qc.questions[0].answers[3].isCorrect);
    }

    [UnityTest]
    public IEnumerator SlideComponentLMSLoadTest()
    {
        GameObject go = new GameObject("Slide");
        SlideController sc = go.AddComponent<SlideController>();
        sc.identifier = LmsLoader.DEFAULT_IDENTIFIER;
        sc.useLmsLoader = true;

        yield return RequireAssets();

        yield return null;

        Assert.IsNotNull(sc.slideFiles[0].slide);
        Assert.IsNotNull(sc.slideFiles[1].slide);
    }

    /// <summary>
    /// Instantiates all necessary game objects for the quiz to work
    /// </summary>
    /// <param name="qc"></param>
    private void SetQuizEnvironment(QuizController qc)
    {
        QuizField quizFieldObject;
        GameObject go = new GameObject();
        Text textObject = go.AddComponent<Text>();
        qc.quizTitleObject = textObject;

        go = new GameObject();
        textObject = go.AddComponent<Text>();
        qc.quizDescriptionObject = textObject;

        go = new GameObject();
        textObject = go.AddComponent<Text>();
        qc.quizStatusMessageObject = textObject;

        go = new GameObject();
        quizFieldObject = go.AddComponent<QuizField>();

        quizFieldObject.autodetectProperties = false;
        go = new GameObject();
        quizFieldObject.text = go.AddComponent<Text>();
        go = new GameObject();
        quizFieldObject.labelText = go.AddComponent<Text>();

        qc.questionField = quizFieldObject;

        List<QuizField> answerFields = new List<QuizField>();
        for (int i = 0; i < 4; i++)
        {
            go = new GameObject();
            QuizField qf = go.AddComponent<QuizField>();

            qf.autodetectProperties = false;
            go = new GameObject();
            qf.text = go.AddComponent<Text>();
            go = new GameObject();
            qf.labelText = go.AddComponent<Text>();

            answerFields.Add(qf);
        }
        qc.answerFields = answerFields;

        qc.wrongAnswerMessage = "wrong message test";
        qc.correctAnswerMessage = "correct message test";
    }
}
