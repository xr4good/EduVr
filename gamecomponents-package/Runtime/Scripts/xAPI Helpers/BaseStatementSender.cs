using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace SeriousGameComponents
{

    ///<summary> BaseStatementSender is a base for all statement senders of all components </summary>
    public class BaseStatementSender : MonoBehaviour
    {
        protected RemoteLRS lrs;

        [HideInInspector]
        [Tooltip("Wheter to use the credentials set on the LRS Information component in this scene or not")]
        public bool useLRSInformation = true;
        [Tooltip("URL of the wished LRS")]
        public string LRSUrl = "";
        [Tooltip("LRS credential Key value")]
        public string remoteKey = "";
        [Tooltip("LRS credential Secret value")]
        public string remoteSecret = "";
        [Space]
        [Tooltip("Identification of the player for the xAPI to register. Usually it's an email address")]
        public string agent = "";

        //These two methods are the respective Start and Update for classes that implements this one

        /// <summary>
        /// Override this method instead of creating a new Start method: <code>Example: public override void ChildStart() { ... } </code>
        /// If you wish, you can also override the regular Start method using a base call at the start:  <code>Example: public override void Start() { base.Start(); } </code>
        /// This is to ensure proper functionality of the functions implemented in the superclass. It is not advised to override both Start and ChildStart, as this may cause inconsistencies in game behaviour.
        /// </summary>
        public virtual void ChildStart() { }

        /// <summary>
        /// Override this method instead of creating a new Update method: <code>Example: public override void ChildUpdate() { ... } </code>
        /// If you wish, you can also override the regular Update method using a base call at the start: <code>Example: public override void Update() { base.Update(); ... } </code> 
        /// This is to ensure proper functionality of the functions implemented in the superclass. It is not advised to override both Update and ChildUpdate, as this may cause inconsistencies in game behaviour.
        /// </summary>
        public virtual void ChildUpdate() { }
        //-------------------------------------------------------------------------------------------

        public virtual void Start()
        {
            if(useLRSInformation)
            {
                LRSUrl = LRSComponent.LRSInformation.LRSILRSUrl;
                remoteKey = LRSComponent.LRSInformation.LRSIremoteKey;
                remoteSecret = LRSComponent.LRSInformation.LRSIremoteSecret;
                agent = LRSComponent.LRSInformation.LRSIAgent;
            }

            try
            {
                lrs = new RemoteLRS(
                LRSUrl,
                remoteKey,
                remoteSecret
                );
            }
            catch (UriFormatException)
            {
                Debug.LogWarning("URI format is invalid, login to LRS failed!");
            }

            ChildStart();
        }

        public virtual void Update()
        {
            if (useLRSInformation)
            {
                LRSUrl = LRSComponent.LRSInformation.LRSILRSUrl;
                remoteKey = LRSComponent.LRSInformation.LRSIremoteKey;
                remoteSecret = LRSComponent.LRSInformation.LRSIremoteSecret;
            }

            ChildUpdate();
        }
        public IEnumerator SaveStatement(Agent actor,
                                  TinCan.Verb verb,
                                  TinCan.Activity activity,
                                  Result result)
        {

            //Build out full Statement details
            Statement statement = new Statement();
            statement.actor = actor;
            statement.verb = verb;
            statement.target = activity;
            statement.result = result;
            double dd;
            double.TryParse(result.score.raw.ToString(), out dd);
            Debug.Log("Built Statement\nActor: " + statement.actor.name + " | Verb: " + verb.display.ToJSON() + " | Activity: " + activity.definition.name.ToJSON() + " | Extension :" + activity.definition.extensions.ToJSON() + " | Result raw: " + Math.Round(dd, 2));
            //Send the statement

            //Creates an async task to save the statement in the LRS
            StatementLRSResponse lrsResponse = null;
            Task t = Task.Run(() => lrsResponse = lrs.SaveStatement(statement)) ;

            //The coroutine stalls the execution to the next frame while the task is still running, to prevent game stuttering
            while (!t.IsCompleted)
            {
                yield return null;
            }

            if (lrsResponse.success) //Success
            {
                Debug.Log("Statement successfully sent!\nId: " + lrsResponse.content.id);
            }
            else //Failure
            {
                Debug.LogError("Statement Failed: " + lrsResponse.errMsg);
            }

            yield break;
        }

        public Agent getActor(string email)
        {
            Agent actor = new Agent();
            actor.mbox = "mailto:" + email;
            actor.name = email.Split('@')[0];

            return actor;
        }

        public TinCan.Verb getVerb(string verbURL, string description)
        {
            TinCan.Verb verb = new TinCan.Verb();
            verb.id = new Uri(verbURL);
            verb.display = new LanguageMap();
            verb.display.Add("en-US", description);

            return verb;
        }

        public TinCan.Activity getActivity(string activityURL, string description, params string[] extensionStrings)
        {
            return getActivity(activityURL, description, GetExtensions(extensionStrings));
        }

        public TinCan.Activity getActivity(string activityURL, string description, TinCan.Extensions extension)
        {
            TinCan.Activity activity = new TinCan.Activity();
            activity.id = new Uri(activityURL).ToString();

            ActivityDefinition activityDefinition = new ActivityDefinition();

            activityDefinition.name = new LanguageMap();
            activityDefinition.name.Add("en-US", (description));
            if (extension != null)
                activityDefinition.extensions = extension;

            activity.definition = activityDefinition;
            return activity;
        }

        public Result getResult(Boolean completion, Boolean success, Double raw)
        {
            Result result = new Result();
            result.completion = completion;
            result.success = success;
            Score score = new Score();
            score.raw = raw;
            result.score = score;

            return result;
        }

        public string GetExtensionJson(string url, string value, bool closed = false)
        {
            return (closed?"{ ":"") + $"\"{url}\" : \"{value}\"" + (closed ? "} " : "");
        }

        public TinCan.Extensions GetExtension(string extensionString)
        {
            JObject json = JObject.Parse(extensionString);
            TinCan.Extensions ext = new TinCan.Extensions(json);
            return ext;
        }

        public TinCan.Extensions GetExtensions(params string[] extensionStrings)
        {
            if (extensionStrings.Length == 0)
                return null;

            string json = "{ ";
            bool first = true;
            foreach(string extensionString in extensionStrings)
            {
                if (!first)
                    json += ", ";
                else first = false;

                json += extensionString;
            }
            json += " }";

            JObject jsonObj = JObject.Parse(json);
            TinCan.Extensions ext = new TinCan.Extensions(jsonObj);
            return ext;
        }
    }
}
