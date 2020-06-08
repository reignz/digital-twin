using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Model
{
    public List<string> Name = new List<string>();

    public List<Vector3> Position = new List<Vector3>();

    public List<Quaternion> Rotation = new List<Quaternion>();

    public List<string> dateTime = new List<string>();


    public void AddModelData(string name, Vector3 position, Quaternion rotation, DateTime time)
    {
        Name.Add(name);
        dateTime.Add(time.ToString());
        Position.Add(position);
        Rotation.Add(rotation);
    }
}
