using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class User
{
    public int ID;
    public string Surname;
    public string Patronymic;
    public string Name;
    public DateTime Birthday;
    public bool Sex;
    public string Position;
    public string Status;
    public string Email;
    public string Password;

    public User()
    {

    }

    public User(string Surname, string Name, string Patronymic, DateTime Birthday, bool Sex, string Position, string Status, string Email, string Password)
    {
        this.Surname = Surname;
        this.Name = Name;
        this.Patronymic = Patronymic;
        this.Birthday = Birthday;
        this.Sex = Sex;
        this.Position = Position;
        this.Status = Status;
        this.Email = Email;
        this.Password = Password;
    }

    public User(int ID, string Surname, string Name, string Patronymic, DateTime Birthday, bool Sex, string Position, string Status, string Email, string Password)
    {
        this.ID = ID;
        this.Surname = Surname;
        this.Name = Name;
        this.Patronymic = Patronymic;
        this.Birthday = Birthday;
        this.Sex = Sex; /*true - men, false - female*/
        this.Position = Position;
        this.Status = Status;
        this.Email = Email;
        this.Password = Password;
    }

    public override string ToString()
    {
        return base.ToString() + " - " + (Name != null && Name.Length > 0 ? Name : "Empty");
    }
}
