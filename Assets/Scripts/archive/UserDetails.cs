using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserDetails
{
    public string url;
    public string user; /*FK:User_ID*/
    public DateTime birthday;
    public bool sex; /*true=Men, false=Female*/
    public string position;
    public string status;
    public string third_name;

    public UserDetails (string Url, string User, DateTime Birthday, bool Sex, string Position, string Status, string ThirdName)
    {
        this.url = Url;
        this.user = User;
        this.birthday = Birthday;
        this.sex = Sex;
        this.position = Position;
        this.status = Status;
        this.third_name = ThirdName;
    }

    public UserDetails(string User, DateTime Birthday, bool Sex, string Position, string Status, string ThirdName)
    {
        this.user = User;
        this.birthday = Birthday;
        this.sex = Sex;
        this.position = Position;
        this.status = Status;
        this.third_name = ThirdName;
    }
}
