using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Task
{
    public int ID;
    public string Name;
    public string Description;
    public DateTime Date;
    public string Status;
    public int User_ID;
    public int Requirement_ID;

    public Task ()
    {

    }

    public Task(int ID, string Name, string Description, DateTime Date, string Status, int User_ID, int Requirement_ID)
    {
        this.ID = ID;
        this.Name = Name;
        this.Description = Description;
        this.Date = Date;
        this.Status = Status;
        this.User_ID = User_ID;
        this.Requirement_ID = Requirement_ID;
    }

    public Task(string Name, string Description, DateTime Date, string Status, int User_ID, int Requirement_ID)
    {
        this.Name = Name;
        this.Description = Description;
        this.Date = Date;
        this.Status = Status;
        this.User_ID = User_ID;
        this.Requirement_ID = Requirement_ID;
    }

    public Task(string Name, string Description, DateTime Date, string Status)
    {
        this.Name = Name;
        this.Description = Description;
        this.Date = Date;
        this.Status = Status;
    }
}
