using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Requirement
{
    public int ID;
    public string Name;
    public string SerialNumber;
    public int Lifetime;
    public DateTime ImplementationDate;
    public string Status;

    public int PressureParameter1;
    public int PressureParameter2;
    public int PressureParameter3;
    public int PressureParameter4;

    public int TemperatureParameter1;
    public int TemperatureParameter2;
    public int TemperatureParameter3;
    public int TemperatureParameter4;

    public Requirement()
    {

    }

    public Requirement (string Name, string SerialNumber, int Lifetime, DateTime ImplementationDate, string Status)
    {
        this.Name = Name;
        this.SerialNumber = SerialNumber;
        this.Lifetime = Lifetime;
        this.ImplementationDate = ImplementationDate;
        this.Status = Status;
    }

    public Requirement(int ID, string Name, string SerialNumber, int Lifetime, DateTime ImplementationDate, string Status)
    {
        this.ID = ID;
        this.Name = Name;
        this.SerialNumber = SerialNumber;
        this.Lifetime = Lifetime;
        this.ImplementationDate = ImplementationDate;
        this.Status = Status;
    }

    public Requirement(string Name, string SerialNumber, int Lifetime, DateTime ImplementationDate, string Status, 
        int PressureParameter1, int PressureParameter2, int PressureParameter3, int PressureParameter4,
        int TemperatureParameter1, int TemperatureParameter2, int TemperatureParameter3, int TemperatureParameter4)
    {
        this.Name = Name;
        this.SerialNumber = SerialNumber;
        this.Lifetime = Lifetime;
        this.ImplementationDate = ImplementationDate;
        this.Status = Status;

        this.PressureParameter1 = PressureParameter1;
        this.PressureParameter2 = PressureParameter2;
        this.PressureParameter3 = PressureParameter3;
        this.PressureParameter4 = PressureParameter4;

        this.TemperatureParameter1 = TemperatureParameter1;
        this.TemperatureParameter2 = TemperatureParameter2;
        this.TemperatureParameter3 = TemperatureParameter3;
        this.TemperatureParameter4 = TemperatureParameter4; 
    }
}
