using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;

//using UnityEditor.VersionControl;
namespace SlideComponent
{
    [RequireComponent(typeof(SlideController))]
    public class StatementSender : ReusableStatementSender
    {
        private SlideController slideController;
        Agent actor;

        public override void ChildStart()
        {
            slideController = GetComponent<SlideController>();
            actor = getActor(email);
        }

        public void LogPassSlide()
        {
            LogViewedSlide();
            if (slideController.isLastSlide)
                LogCompleted();
        }


        //TODO - verify if and where to put the time spent at the previous slide. Use extensions or not
        public void LogViewedSlide()
        {
            TinCan.Verb verb = getVerb(LogVerb.Viewed.url, LogVerb.Viewed.descriptionEnUS);
            TinCan.Activity activity = getActivity(LogActivity.Slide.url, LogActivity.Slide.descriptionEnUS);

            Result result = getResult(true,
                                      true,
                                      slideController.currentSlideIndex);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogCompleted()
        {
            TinCan.Verb verbCompleted = getVerb(LogVerb.Completed.url, LogVerb.Completed.descriptionEnUS);
            TinCan.Activity activity = getActivity(LogActivity.Slide.url, LogActivity.Slide.descriptionEnUS);

            Result resultCompleted = getResult(true, true, 100);
            StartCoroutine(SaveStatement(actor, verbCompleted, activity, resultCompleted));
        }
    }
}