using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeriousGameComponents.QuizComponent;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class RegistryTests
{
    QuizController testSubject;
    Text titleObject;
    Text descriptionObject;
    Text statusMessageObject;

    QuizField questionField;
    List<QuizField> answerFields;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject();
        testSubject = go.AddComponent<QuizController>();

        testSubject.questions = new List<QuizController.QuizQuestion>
        {
            new QuizController.QuizQuestion("test question 0", 4),
            new QuizController.QuizQuestion("test question 1", 4),
            new QuizController.QuizQuestion("test question 2", 4),
            new QuizController.QuizQuestion("test question 3", 4),
        };

        int counter = 0;
        foreach(QuizController.QuizQuestion qq in testSubject.questions)
        {
            qq.answers = new List<QuizController.QuizQuestion.QuestionAnswer>
            {
                new QuizController.QuizQuestion.QuestionAnswer("question " + counter + " test answer 0", false),
                new QuizController.QuizQuestion.QuestionAnswer("question " + counter + " test answer 1", true),
                new QuizController.QuizQuestion.QuestionAnswer("question " + counter + " test answer 2", false),
                new QuizController.QuizQuestion.QuestionAnswer("question " + counter + " test answer 3", false),
            };
            counter++;
        }

        go = new GameObject();
        titleObject = go.AddComponent<Text>();
        testSubject.quizTitleObject = titleObject;

        go = new GameObject();
        descriptionObject = go.AddComponent<Text>();
        testSubject.quizDescriptionObject = descriptionObject;

        go = new GameObject();
        statusMessageObject = go.AddComponent<Text>();
        testSubject.quizStatusMessageObject = statusMessageObject;

        go = new GameObject();
        questionField = go.AddComponent<QuizField>();

        questionField.autodetectProperties = false;
        go = new GameObject();
        questionField.text = go.AddComponent<Text>();
        go = new GameObject();
        questionField.labelText = go.AddComponent<Text>();

        testSubject.questionField = questionField;

        answerFields = new List<QuizField>();
        for(int i=0; i<4; i++)
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
        testSubject.answerFields = answerFields;

        testSubject.wrongAnswerMessage = "wrong message test";
        testSubject.correctAnswerMessage = "correct message test";
    }

    [Test]
    public void ShowQuestionTest()
    {
        for (int questionCounter = 0; questionCounter < testSubject.questions.Count; questionCounter++)
        {
            testSubject.ShowQuestion(questionCounter);

            Assert.AreEqual("test question " + questionCounter, testSubject.questionField.text.text);
            Assert.AreEqual("" + (questionCounter + 1), testSubject.questionField.labelText.text);
            Assert.AreEqual("", testSubject.quizStatusMessageObject.text);

            int ansCounter = 0;
            foreach (QuizField qf in testSubject.answerFields)
            {
                Assert.AreEqual("question " + questionCounter + " test answer " + ansCounter, qf.text.text);

                if (testSubject.questions[questionCounter].listAnswersWithLetters)
                    Assert.AreEqual(((char)(65 + ansCounter)).ToString(), qf.labelText.text);
                else
                    Assert.AreEqual("" + (ansCounter + 1), qf.labelText.text);

                ansCounter++;
            }
        }
    }

    [Test]
    public void SelectAnswerTest()
    {
        testSubject.ShowQuestion(0);

        testSubject.SelectAnswer(0);
        Assert.AreEqual("question 0 test answer 0", testSubject.questions[0].answers[testSubject.selectedAnswer].answer);

        testSubject.SelectAnswer(1);
        Assert.AreEqual("question 0 test answer 1", testSubject.questions[0].answers[testSubject.selectedAnswer].answer);

        testSubject.SelectAnswer(2);
        Assert.AreEqual("question 0 test answer 2", testSubject.questions[0].answers[testSubject.selectedAnswer].answer);

        testSubject.SelectAnswer(3);
        Assert.AreEqual("question 0 test answer 3", testSubject.questions[0].answers[testSubject.selectedAnswer].answer);
    }

    [Test]
    public void UnselectAnswerTest()
    {
        testSubject.ShowQuestion(1);
        testSubject.SelectAnswer(1);
        testSubject.UnselectAnswer();

        Assert.AreEqual(-1, testSubject.selectedAnswer);
    }

    [Test]
    public void WrongAnswerMessageTest()
    {
        testSubject.ShowWrongAnswerMessage();

        Assert.AreEqual("wrong message test", testSubject.quizStatusMessageObject.text);
    }

    [Test]
    public void CorrectAnswerMessageTest()
    {
        testSubject.ShowCorrectAnswerMessage();

        Assert.AreEqual("correct message test", testSubject.quizStatusMessageObject.text);
    }

    [Test]
    public void ClearAnswerMessageTest()
    {
        testSubject.ClearStatusMessage();

        Assert.AreEqual("", testSubject.quizStatusMessageObject.text);
    }

    [Test]
    public void AnswerQuestionTests()
    {
        testSubject.ShowQuestion(0);

        testSubject.AnswerQuestion(0);
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.AnswerQuestion(2);
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.AnswerQuestion(3);
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.AnswerQuestion(1);
        Assert.IsTrue(testSubject.isLastAnswerCorrect);

        testSubject.ShowQuestion(0);

        testSubject.SelectAnswer(0);
        testSubject.AnswerQuestion();
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.UnselectAnswer();
        LogAssert.Expect(LogType.Error, new Regex(".*"));
        testSubject.AnswerQuestion();

        testSubject.SelectAnswer(2);
        testSubject.AnswerQuestion();
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.SelectAnswer(3);
        testSubject.AnswerQuestion();
        Assert.IsFalse(testSubject.isLastAnswerCorrect);

        testSubject.SelectAnswer(1);
        testSubject.AnswerQuestion();
        Assert.IsTrue(testSubject.isLastAnswerCorrect);
    }

    [Test]
    public void NextQuestionTest()
    {
        testSubject.ShowQuestion(0);

        int questionIndex = testSubject.currentQuestionIndex;
        testSubject.NextQuestion();
        Assert.AreEqual(questionIndex + 1,testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.NextQuestion();
        Assert.AreEqual(questionIndex + 1, testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.NextQuestion();
        Assert.AreEqual(questionIndex + 1, testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.NextQuestion();
        Assert.AreEqual(questionIndex, testSubject.currentQuestionIndex);
    }

    [Test]
    public void PreviousQuestionTest()
    {
        testSubject.ShowQuestion(3);

        int questionIndex = testSubject.currentQuestionIndex;
        testSubject.PreviousQuestion();
        Assert.AreEqual(questionIndex - 1, testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.PreviousQuestion();
        Assert.AreEqual(questionIndex - 1, testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.PreviousQuestion();
        Assert.AreEqual(questionIndex - 1, testSubject.currentQuestionIndex);

        questionIndex = testSubject.currentQuestionIndex;
        testSubject.PreviousQuestion();
        Assert.AreEqual(questionIndex, testSubject.currentQuestionIndex);
    }
}
