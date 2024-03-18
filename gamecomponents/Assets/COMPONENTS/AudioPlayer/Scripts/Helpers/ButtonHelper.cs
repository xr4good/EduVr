using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioPlayerComponent
{
    [RequireComponent(typeof(AudioPlayer))]
    public class ButtonHelper : ButtonHelperAbstract
    {
        private AudioPlayer audioPlayer;
        protected override void ChildStart()
        {
            audioPlayer = GetComponent<AudioPlayer>();
        }

        protected override void ChildUpdate()
        {
            if (audioPlayer.IsAudioAvailable(audioPlayer.currentAudioIndex))
            {
                if (!audioPlayer.startedAudio)
                {
                    EnableButton("play");
                    DisableButton("pause");
                    DisableButton("stop");
                }
                else if (audioPlayer.audioIsPlaying)
                {
                    DisableButton("play");
                    EnableButton("pause");
                    EnableButton("stop");
                }
                else
                {
                    EnableButton("play");
                    DisableButton("pause");
                    EnableButton("stop");
                }
            }
            else
            {
                DisableButton("play");
                DisableButton("pause");
                DisableButton("stop");
            }
            
            if(audioPlayer.GetAudioCount() > 0)
            {
                if (audioPlayer.isLastAudio)
                    DisableButton("next");
                else
                    EnableButton("next");

                if (audioPlayer.isFirstAudio)
                    DisableButton("previous");
                else
                    EnableButton("previous");
            }
            else
                DisableAllButtons();
        }
    }

}
