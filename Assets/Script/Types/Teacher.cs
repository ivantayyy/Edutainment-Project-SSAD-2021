using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Teacher
{
    // Start is called before the first frame update
    public string accType;
    public string username;
    public string id;
    public List<string> assignedAssignments;

    public Teacher(string _accType, string _username, string _id)
    {
        this.accType = _accType;
        this.username = _username;
        this.id = _id;
        this.assignedAssignments = new List<string>();
        this.assignedAssignments.Add("NONE");
    }
}
