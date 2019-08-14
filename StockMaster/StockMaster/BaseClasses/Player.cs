using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Player
    {
        private string firstname;

        public string FirstName
        {
            get { return firstname; }
            set { firstname = value; }
        }

        private string lastname;

        public string LastName
        {
            get { return lastname; }
            set { lastname = value; }
        }

        private string licencenumber;

        public string LicenceNumber
        {
            get { return licencenumber; }
            set { licencenumber = value; }
        }

        public Player(string LastName, string FirstName)
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.LicenceNumber = string.Empty;
        }
    }
}
