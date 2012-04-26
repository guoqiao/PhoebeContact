using System;

public class State
{
    public int id { get; set; }

    public string name { get; set; }
    public int    period { get; set; }
    public int    total { get; set; }
    public int    count { get; set; }
    public string template { get; set; }
}

public class Customer
{
    public int id { get; set; }

    public string name { get; set; }
    public string site { get; set; }
    public string addr { get; set; }
    public string country { get; set; }
    public string phone { get; set; }

    public string contact { get; set; }
    public string mobile { get; set; }
    public string email { get; set; }

    public DateTime create_on { get; set; }
    public DateTime update_on { get; set; }

    public string note { get; set; }

    public int state_id { get; set; }
}