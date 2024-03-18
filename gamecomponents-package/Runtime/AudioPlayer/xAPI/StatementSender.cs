using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;

namespace SeriousGameComponents.AudioPlayerComponent
{
    [RequireComponent(typeof(AudioPlayer))]
    public class StatementSender : BaseStatementSender
    {
        private AudioPlayer audioPlayer;

        // Start is called before the first frame update
        public override void ChildStart()
        {
            audioPlayer = GetComponent<AudioPlayer>();
        }

        // Update is called once per frame
        public override void ChildUpdate()
        {
            if(pauseTimer)
            {
                pauseTimerCount += Time.deltaTime;
            }
        }

        public void LogStartAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Started.url, LogVerb.Started.descriptionEnUS);
            string audioClipLenght = audioPlayer.audioClip.length.ToString();

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.Duration.url, audioClipLenght + " seconds"),
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
            );

            Result result = getResult(true,
                                      true,
                                      audioPlayer.audioClip.length);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogPlayAudio()
        {
            if (audioPlayer.audioSource.time < 0.2f)
                LogStartAudio();
            else
                LogResumeAudio();
        }

        public void LogResumeAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Resumed.url, LogVerb.Resumed.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
            );

            Result result = getResult(true,
                                      true,
                                      pauseTimerCount);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
            pauseTimer = false;
            pauseTimerCount = 0f;
        }

        float pauseTimerCount = 0f;
        bool pauseTimer = false;
        public void LogPauseAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Paused.url, LogVerb.Paused.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
            );

            Result result = getResult(true,
                                      true,
                                      audioPlayer.audioSource.time);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
            pauseTimer = true;
            pauseTimerCount = 0f;
        }

        public void LogCancelAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Canceled.url, LogVerb.Canceled.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
            );

            Result result = getResult(true,
                                      true,
                                      audioPlayer.audioSource.time);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
            pauseTimer = false;
            pauseTimerCount = 0f;
        }

        public void LogListenedAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Listened.url, LogVerb.Listened.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
            );

            Result result = getResult(true,
                                      true,
                                      audioPlayer.audioSource.time);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogSkipAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Skipped.url, LogVerb.Skipped.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
               );

            Result result = getResult(true, true, audioPlayer.audioSource.time);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogReturnAudio()
        {
            Agent actor = getActor(agent);

            TinCan.Verb verb = getVerb(LogVerb.Returned.url, LogVerb.Returned.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Audio.url,
                LogActivity.Audio.descriptionEnUS,
                GetExtensionJson(LogExtensions.AttemptID.url, audioPlayer.identifier)
               );

            Result result = getResult(true, true, audioPlayer.audioSource.time);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }
    }
}

