using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms.Impl;
using System;

[Serializable]
public class InitUser
{
    // Start is called before the first frame update
    public string accType;
    public string id;
    public string username;

    public string classSubscribed;

    public Scores multiPlayer;
    public Scores singlePlayer;
    public Scores customPlayer;



    public InitUser()
    {

    }
    public InitUser(string _accType, string _username,string _id,string classSubscribed)
    {
        this.classSubscribed = classSubscribed;
        this.accType = _accType;
        this.username = _username;
        this.id = _id;
        this.multiPlayer = new Scores();
        this.singlePlayer = new Scores();
        this.customPlayer = new Scores();
    }

}