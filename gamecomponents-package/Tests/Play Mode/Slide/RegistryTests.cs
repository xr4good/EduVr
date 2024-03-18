using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeriousGameComponents.SlideComponent;

public class RegistryTests
{
    SlideController testSubject;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject();
        testSubject = go.AddComponent<SlideController>();

        testSubject.slideFiles = new List<SlideController.SlideRegistry>
        {
            new SlideController.SlideRegistry("test slide 1"),
            new SlideController.SlideRegistry("test slide 2", "https://cdn.pixabay.com/photo/2014/06/03/19/38/board-361516_960_720.jpg"),
            new SlideController.SlideRegistry(new Texture2D(10, 10), "test slide 3"),
            new SlideController.SlideRegistry("test slide 4", "https://cdn.pixabay.com/photo/2017/06/28/10/53/board-2450236_960_720.jpg")
        };
    }

    [UnityTest]
    public IEnumerator DownloadAllSlidesTests()
    {
        testSubject.DownloadAllSlides();

        while(testSubject.slideFiles[1].downloading || testSubject.slideFiles[3].downloading)
            yield return null;

        if(testSubject.slideFiles[1].httpError != "" || testSubject.slideFiles[3].httpError != "")
        {
            Assert.Inconclusive("Inconclusive tests. Either the URLs defined in this file don't work anymore or you are not connected to the internet");
        }

        Assert.IsTrue(testSubject.slideFiles[0].slide == null);
        Assert.IsTrue(testSubject.slideFiles[1].slide != null);
        Assert.IsTrue(testSubject.slideFiles[3].slide != null);
    }

    [Test]
    public void SlideShouldNotBeAvailable()
    {
        Assert.IsTrue(!testSubject.IsSlideAvailable(0));
        Assert.IsTrue(!testSubject.IsSlideAvailable(1));
        Assert.IsTrue(testSubject.IsSlideAvailable(2));
        Assert.IsTrue(!testSubject.IsSlideAvailable(3));
    }

    [Test]
    public void SlideCountTest()
    {
        Assert.AreEqual(testSubject.slideFiles.Count, testSubject.GetSlideCount());
    }

    [Test]
    public void ChangeCurrentSlideTest()
    {
        testSubject.ChangeCurrentSlide(0);
        Assert.AreEqual("test slide 1", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.ChangeCurrentSlide(1);
        Assert.AreEqual("test slide 2", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.ChangeCurrentSlide(2);
        Assert.AreEqual("test slide 3", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.ChangeCurrentSlide(3);
        Assert.AreEqual("test slide 4", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.ChangeCurrentSlide(0);
    }

    [Test]
    public void NextSlideTest()
    {
        testSubject.NextSlide();
        Assert.AreEqual("test slide 2", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.NextSlide();
        Assert.AreEqual("test slide 3", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.NextSlide();
        Assert.AreEqual("test slide 4", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.NextSlide();
        Assert.AreEqual("test slide 4", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.ChangeCurrentSlide(0);
    }

    [Test]
    public void PrevSlideTest()
    {
        testSubject.ChangeCurrentSlide(3);

        testSubject.PreviousSlide();
        Assert.AreEqual("test slide 3", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.PreviousSlide();
        Assert.AreEqual("test slide 2", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.PreviousSlide();
        Assert.AreEqual("test slide 1", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

        testSubject.PreviousSlide();
        Assert.AreEqual("test slide 1", testSubject.slideFiles[testSubject.currentSlideIndex].slideLabel);

    }
}
