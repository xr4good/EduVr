using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;

//using UnityEditor.VersionControl;
namespace QuizComponent
{
    [RequireComponent(typeof(QuizController))]
    public class StatementSender : ReusableStatementSender
    {
        private QuizController slideController;
        Agent actor;

        public override void ChildStart()
        {
            slideController = GetComponent<QuizController>();
            actor = getActor(email);
        }
    }
}