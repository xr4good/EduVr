using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeriousGameComponents.LmsComponent;
using System.Text.RegularExpressions;

public class FetchTests
{
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

        if (testSubject.lessonList == null)
            testSubject.lessonList = new List<string>();
    }

    [UnityTest]
    public IEnumerator P1_ShouldBeSingleton()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject go = new GameObject();
        go.AddComponent <LmsLoader>();
        yield return new WaitForSeconds(1f);

        Assert.IsNull(go.GetComponent<LmsLoader>());
        GameObject.Destroy(go);
    }

    [UnityTest]
    public IEnumerator P2_SubmitLoginTests()
    {
        testSubject.SubmitLogin(loginUsername, loginPassword);
        yield return null;

        while(testSubject.loggingIn)
        {
            yield return null;
        }

        if(testSubject.loginException != null)
        {
            Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");
        }

        Assert.IsTrue(testSubject.loginSuccessful);
        Assert.IsTrue(testSubject.IsLoggedIn());
    }

    [UnityTest]
    public IEnumerator P3_GetContentTests()
    {
        while (!testSubject.loginSuccessful)
        {
            if (testSubject.loginException != null)
            {
                Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");
            }

            yield return null;
        }

        testSubject.GetContent();
        yield return null;

        while(testSubject.fetchingContent)
        {
            yield return null;
        }

        Assert.IsNull(testSubject.fetchingContentException, "URL for fetching content no longer works!");
        Assert.IsTrue(testSubject.contentFetched);
        Assert.IsTrue(testSubject.ContentIsFetched());
    }

    [UnityTest]
    public IEnumerator P4_DownloadLessonAssetsTest()
    {
        while (!testSubject.loginSuccessful || !testSubject.contentFetched)
        {
            if (testSubject.loginException != null)
            {
                Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");
            }

            Assert.IsNull(testSubject.fetchingContentException, "URL for fetching content no longer works!");

            yield return null;
        }

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

        if(lessonIndex == -1)
        {
            Assert.Inconclusive("Inconclusive test. Could not find test lesson registered for this test user in the LMS");
        }

        testSubject.DownloadLessonAssets(lessonIndex);

        Assert.IsTrue(Regex.IsMatch(LmsLoader.description, @"this\sis\sa\stest\slesson",RegexOptions.IgnoreCase));
        Assert.IsTrue(Regex.IsMatch(LmsLoader.title, @"test\slesson\s3D\slab", RegexOptions.IgnoreCase));

        while(!LmsLoader.slidesFetched)
        {
            yield return null;
        }

        Assert.IsFalse(LmsLoader.slides.Count == 0);

        while(!LmsLoader.quizFetched)
        {
            yield return null;
        }

        Assert.IsFalse(LmsLoader.quizes.Count == 0);

        /*
        while(!LmsLoader.audiosFetched)
        {
            yield return null;
        }

        Assert.IsFalse(LmsLoader.audios.Count == 0);

        while(!LmsLoader.videosFetched)
        {
            yield return null;
        }

        Assert.IsFalse(LmsLoader.videos.Count == 0);
        */
    }

    private IEnumerator RequireLogin()
    {
        while (!testSubject.loginSuccessful)
        {
            if (testSubject.loginException != null)
            {
                Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");
            }

            yield return null;
        }
    }

    private IEnumerator RequireContent()
    {
        while (!testSubject.loginSuccessful || !testSubject.contentFetched)
        {
            if (testSubject.loginException != null)
            {
                Assert.Inconclusive("Inconclusive test! Either the login credentials in this file no longer work or you are not connected to the internet!");
            }

            Assert.IsNull(testSubject.fetchingContentException, "URL for fetching content no longer works!");

            yield return null;
        }
    }
}
