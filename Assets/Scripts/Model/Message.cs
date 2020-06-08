using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{
    public int Zone { get; set; }
    public string Parameter { get; set; }
    public string Value { get; set; }

    public Message(int Zone, string Parameter, string Value)
    {
        this.Zone = Zone;
        /*zones:0=green, 1=yellow, 2=red*/
        this.Parameter = Parameter;
        this.Value = Value;
    }
}
