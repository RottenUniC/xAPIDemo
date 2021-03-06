using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TinCan;
using TinCan.LRSResponses;

public class StatementSender : MonoBehaviour
{
    public string _actor;
    public string _verb;
    public string _definition;
    public int _value;

    private RemoteLRS lrs;

    public RemoteLRS Lrs { get => lrs; set => lrs = value; }

    // Start is called before the first frame update
    void Start()
    {
        Lrs = new RemoteLRS(
       "https://trial-lrs.yetanalytics.io/xapi",
       "a2486aad83b6621e2cc5815bd65bc382",
       "39430534dd77d0e2b47877df1f5b0a29"
       );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendStatement();
        }
    }

    public void SendStatement()
    {

        //Build out Actor details
        Agent actor = new Agent();
        actor.mbox = "mailto:" + _actor.Replace(" ", "") + "@email.com";
        actor.name = _actor;

        //Build out Verb details
        Verb verb = new Verb();
        verb.id = new Uri("http://www.example.com/​" + _verb.Replace(" ", ""));
        verb.display = new LanguageMap();
        verb.display.Add("en-US", _verb);

        //Build out Activity details
        Activity activity = new Activity();
        activity.id = new Uri("http://www.example.com/​" + _definition.Replace(" ", "")).ToString();

        //Build out Activity Definition details
        ActivityDefinition activityDefinition = new ActivityDefinition();
        activityDefinition.description = new LanguageMap();
        activityDefinition.name = new LanguageMap();
        activityDefinition.name.Add("en-US", (_definition));
        activity.definition = activityDefinition;

        Result result = new Result();
        Score score = new Score();

        score.raw = _value;
        result.score = score;

        //Build out full Statement details
        Statement statement = new Statement();
        statement.actor = actor;
        statement.verb = verb;
        statement.target = activity;
        statement.result = result;

        //Send the statement
        StatementLRSResponse lrsResponse = Lrs.SaveStatement(statement);
        if (lrsResponse.success) //Success
        {
            Debug.Log("Save statement: " + lrsResponse.content.id);
        }
        else //Failure
        {
            Debug.Log("Statement Failed: " + lrsResponse.errMsg);
        }

    }
}
