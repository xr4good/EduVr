using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;

//using UnityEditor.VersionControl;
namespace SeriousGameComponents.QuizComponent
{
    [RequireComponent(typeof(QuizController))]
    public class StatementSender : BaseStatementSender
    {
        private QuizController quizController;
        Agent actor;

        public override void ChildStart()
        {
            quizController = GetComponent<QuizController>();
            actor = getActor(agent);
        }
        //extensions = quiz identifier

        public void LogAnsweredQuestion()
        {
            LogAnsweredQuestion(quizController.selectedAnswer);
        }

        public void LogAnsweredQuestion(int answerIndex)
        {
            TinCan.Verb verb = getVerb(LogVerb.Answered.url, LogVerb.Answered.descriptionEnUS);
            TinCan.Activity activity = getActivity(
                LogActivity.Question.url, 
                LogActivity.Question.descriptionEnUS, 
                GetExtensionJson(LogExtensions.AttemptID.url, quizController.identifier)
                );

            //raw = question index
            Result result = getResult(true,
                                      true,
                                      answerIndex);

            StartCoroutine(SaveStatement(actor, verb, activity, result));

            if (quizController.quizCompleted)
                LogCompletedQuiz();
        }

        public void LogOpenedQuiz()
        {
            TinCan.Verb verb = getVerb(LogVerb.Opened.url, LogVerb.Opened.descriptionEnUS);

            TinCan.Activity activity = getActivity(
                LogActivity.Assessment.url, 
                LogActivity.Assessment.descriptionEnUS, 
                GetExtensionJson(LogExtensions.AttemptID.url, quizController.identifier)
                );

            //raw = nothing
            Result result = getResult(true,
                                      true,
                                      1f);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogSelectedAnswer()
        {
            TinCan.Verb verb = getVerb(LogVerb.Selected.url, LogVerb.Selected.descriptionEnUS);
            TinCan.Activity activity = getActivity(
                LogActivity.ChecklistItem.url, 
                LogActivity.ChecklistItem.descriptionEnUS, 
                GetExtensionJson(LogExtensions.AttemptID.url, quizController.identifier)
                );

            //raw = selected answer
            Result result = getResult(true,
                                      true,
                                      quizController.selectedAnswer);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogClosedQuiz()
        {
            TinCan.Verb verb = getVerb(LogVerb.Closed.url, LogVerb.Closed.descriptionEnUS);
            TinCan.Activity activity = getActivity(
                LogActivity.Assessment.url, 
                LogActivity.Assessment.descriptionEnUS, 
                GetExtensionJson(LogExtensions.AttemptID.url, quizController.identifier)
                );

            //raw = selected answer
            Result result = getResult(true,
                                      true,
                                      1f);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }

        public void LogCompletedQuiz()
        {
            TinCan.Verb verb = getVerb(LogVerb.Completed.url, LogVerb.Completed.descriptionEnUS);
            TinCan.Activity activity = getActivity(
                LogActivity.Assessment.url, 
                LogActivity.Assessment.descriptionEnUS, 
                GetExtensionJson(LogExtensions.AttemptID.url, quizController.identifier));

            //raw = selected answer
            Result result = getResult(true,
                                      true,
                                      1f);

            StartCoroutine(SaveStatement(actor, verb, activity, result));
        }
    }

}