using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SeriousGameComponents.AudioPlayerComponent;

public class RegistryTests
{
    private AudioPlayer testSubject;
    private List<string> audioLabels;

    [SetUp]
    public void Setup()
    {
        GameObject go = new GameObject();
        testSubject = go.AddComponent<AudioPlayer>();

        audioLabels = new List<string>
        {
            "test audio 0",
            "test audio 1",
            "test audio 2",
            "test audio 3",
            "test audio 4",
            "test audio 5"
        };

        testSubject.audioFiles = new List<AudioPlayer.AudioFilesRegistry>
        {
            new AudioPlayer.AudioFilesRegistry(audioLabels[0]),
            new AudioPlayer.AudioFilesRegistry(audioLabels[1], "https://www2.cs.uic.edu/~i101/SoundFiles/BabyElephantWalk60.wav", AudioType.WAV),
            new AudioPlayer.AudioFilesRegistry(audioLabels[2]),
            new AudioPlayer.AudioFilesRegistry(audioLabels[3], "https://www2.cs.uic.edu/~i101/SoundFiles/Fanfare60.wav", AudioType.WAV),
            new AudioPlayer.AudioFilesRegistry(audioLabels[4], "https://www2.cs.uic.edu/~i101/SoundFiles/PinkPanther30.wav", AudioType.WAV),
            new AudioPlayer.AudioFilesRegistry(audioLabels[5])
        };
    }

    [Test]
    public void AudioCountTest()
    {
        Assert.AreEqual(testSubject.audioFiles.Count, testSubject.GetAudioCount());
    }

    [Test]
    public void ShouldHaveUndownloadedAudios()
    {
        Assert.IsTrue(testSubject.HasUndownloadedAudios());
    }

    [Test]
    public void ShouldGetAudioByLabel()
    {
        Assert.AreEqual(testSubject.audioFiles[2], testSubject.GetAudioByLabel(audioLabels[2]));
    }

    [Test]
    public void ShouldGetAudioIndexByLabel()
    {
        Assert.AreEqual(2, testSubject.GetAudioIndexByLabel(audioLabels[2]));
    }

    [UnityTest]
    public IEnumerator DownloadAllAudiosTests()
    {
        testSubject.DownloadAllAudioClips();

        while(testSubject.audioFiles[1].downloading ||
            testSubject.audioFiles[3].downloading ||
            testSubject.audioFiles[4].downloading)
        {
            Assert.IsTrue(testSubject.IsDownloading());

            for (int i = 0; i < testSubject.audioFiles.Count; i++)
            {
                Assert.AreEqual(testSubject.audioFiles[i].downloading, testSubject.IsDownloading(i));
                Assert.AreEqual(testSubject.audioFiles[i].downloading, testSubject.IsDownloading(audioLabels[i]));
            }
                
            yield return null;
        }

        if (testSubject.audioFiles[1].httpError != "" ||
            testSubject.audioFiles[3].httpError != "" ||
            testSubject.audioFiles[4].httpError != "")
        {
            Assert.Inconclusive("Inconclusive tests. Either the URLs defined in this file don't work anymore or you are not connected to the internet");
        }

        Assert.IsTrue(testSubject.IsDownloaded(1));
        Assert.IsTrue(testSubject.audioFiles[1].audioClip != null);
        Assert.IsTrue(testSubject.IsDownloaded(audioLabels[1]));

        Assert.IsTrue(testSubject.IsDownloaded(3));
        Assert.IsTrue(testSubject.audioFiles[3].audioClip != null);
        Assert.IsTrue(testSubject.IsDownloaded(audioLabels[3]));

        Assert.IsTrue(testSubject.IsDownloaded(4));
        Assert.IsTrue(testSubject.audioFiles[4].audioClip != null);
        Assert.IsTrue(testSubject.IsDownloaded(audioLabels[4]));
    }

    [Test]
    public void ChangeCurrentAudioClipTest()
    {
        for (int i = 0; i < testSubject.audioFiles.Count; i++)
        {
            testSubject.ChangeCurrentAudioClip(i);
            Assert.AreEqual(audioLabels[i], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);
        }
    }

    [Test]
    public void NextAudioClipTest()
    {
        testSubject.ChangeCurrentAudioClip(0);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[1], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[2], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[3], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[4], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[5], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.NextAudio();
        Assert.AreEqual(audioLabels[5], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);
    }

    [Test]
    public void PreviousAudioClipTest()
    {
        testSubject.ChangeCurrentAudioClip(5);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[4], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[3], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[2], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[1], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[0], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);

        testSubject.PreviousAudio();
        Assert.AreEqual(audioLabels[0], testSubject.audioFiles[testSubject.currentAudioIndex].audioLabel);
    }
}
