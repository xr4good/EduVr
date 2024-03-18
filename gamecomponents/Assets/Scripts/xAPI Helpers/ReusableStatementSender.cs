using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinCan;
using TinCan.LRSResponses;
using System;
using Newtonsoft.Json.Linq;

///<summary> ReusableStatementSender is a base for all statement senders of all components </summary>
public class ReusableStatementSender : MonoBehaviour
{
    protected RemoteLRS lrs;
    protected String email = "teste@teste.com";
    public String remoteLRSURL = "";
    public String remoteKey = "";
    public String remoteSecret = "";

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
        try
        {
            lrs = new RemoteLRS(
            remoteLRSURL,
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
        ChildUpdate();
    }

    public IEnumerator SaveStatement(Agent actor,
                              TinCan.Verb verb, 
                              TinCan.Activity activity, 
                              Result result) {

        //Build out full Statement details
        Statement statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;
        double dd;
        double.TryParse(result.score.raw.ToString(), out dd);
        Debug.Log("Statement: " + statement.actor.name + verb.display.ToJSON() + activity.definition.name.ToJSON() + Math.Round(dd, 2));
        //Send the statement
        StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
        if (lrsResponse.success) //Success
        {
            Debug.Log("Save statement: " + lrsResponse.content.id);
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrsResponse.errMsg);
        }
        yield break;
    }

    public Agent getActor(string email) {
        Agent actor = new Agent();
        actor.mbox = "mailto:" + email;
        actor.name = email.Split('@')[0];

        return actor;
    }

    public TinCan.Verb getVerb(string verbURL, string description) {
        TinCan.Verb verb = new TinCan.Verb();
        verb.id = new Uri(verbURL);
        verb.display = new LanguageMap();
        verb.display.Add("en-US", description);

        return verb;
    }

    public TinCan.Activity getActivity(string activityURL, string description, string extensionString = "") {
        return getActivity(activityURL, description, extensionString != ""? GetExtension(extensionString) : null);
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

    public Result getResult(Boolean completion, Boolean success, Double raw) {
        Result result = new Result();
        result.completion= completion;
        result.success= success;
        Score score = new Score();
        score.raw = raw;
        result.score=score;

        return result;
    }

    public TinCan.Extensions GetExtension(string url, string value)
    {
        JObject json = JObject.Parse("{ " + $"'{url}' : '{value}'" + " }");
        TinCan.Extensions ext = new TinCan.Extensions(json);

        return ext;
    }

    public TinCan.Extensions GetExtension(string extensionString)
    {
        JObject json = JObject.Parse(extensionString);
        TinCan.Extensions ext = new TinCan.Extensions(json);
        return ext;
    }
}
