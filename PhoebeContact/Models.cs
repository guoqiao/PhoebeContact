using System;

namespace PhoebeContact
{

    [PetaPoco.TableName("Country")]
    [PetaPoco.PrimaryKey("id")]
    public class Country
    {
        public int id { get; set; }

        public string abbr { get; set; }
        public string chinese { get; set; }
        public string english { get; set; }

        public string area { get; set; }
        public string code { get; set; }

        public string language { get; set; }
        public string google { get; set; }

        public float hourdiff { get; set; }

        public override string ToString()
        {
            return abbr;
        }
    }

    [PetaPoco.TableName("State")]
    [PetaPoco.PrimaryKey("id")]
    public class State
    {
        public int id { get; set; }

        public string name { get; set; }
        public int period { get; set; }
        public int total { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    [PetaPoco.TableName("Customer")]
    [PetaPoco.PrimaryKey("id")]
    public class Customer
    {
        public int id { get; set; }

        public string company { get; set; }
        public string site { get; set; }
        public string addr { get; set; }
        public string country { get; set; }
        public string phone { get; set; }

        public string name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string skype { get; set; }
        public int browse { get; set; }
        public int inquiry { get; set; }

        public DateTime create_on { get; set; }
        public DateTime update_on { get; set; }

        public int state_id { get; set; }
        public int count { get; set; }
        public string note { get; set; }

        public override string ToString()
        {
            return company;
        }
    }
}