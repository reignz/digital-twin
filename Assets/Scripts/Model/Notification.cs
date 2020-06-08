using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Notification
{
    public int ID;
    public bool Priority; /*true = critical, false - warning*/
    public string Placeholder;
    public string Text;
    public DateTime CreationDate;
    public int? Requirement_ID;
    public int? User_ID;
    public bool Active;

    public Notification()
    { }

    public Notification (int ID, bool Priority, string Placeholder, string Text, DateTime CreationDate, int Requirement_ID, int User_ID, bool Active)
    {
        this.ID = ID;
        this.Priority = Priority; 
        this.Placeholder = Placeholder;
        this.Text = Text;
        this.CreationDate = CreationDate;
        this.Requirement_ID = Requirement_ID;
        this.User_ID = User_ID;
        this.Active = Active;
    }

    public Notification(bool Priority, string Placeholder, string Text, DateTime CreationDate, int Requirement_ID, int User_ID, bool Active)
    {
        this.Priority = Priority;
        this.Placeholder = Placeholder;
        this.Text = Text;
        this.CreationDate = CreationDate;
        this.Requirement_ID = Requirement_ID;
        this.User_ID = User_ID;
        this.Active = Active;
    }

    public Notification(bool Priority, string Placeholder, string Text, DateTime CreationDate, bool Active)
    {
        this.Priority = Priority;
        this.Placeholder = Placeholder;
        this.Text = Text;
        this.CreationDate = CreationDate;
        this.Active = Active;
    }
}
