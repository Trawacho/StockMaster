namespace StockMaster.BaseClasses
{
    public class Player : TBaseClass
    {
        private string firstname;

        public string FirstName
        {
            get { return firstname; }
            set
            {
                if (firstname == value)
                    return;

                firstname = value;
                RaisePropertyChanged();
            }
        }

        private string lastname;

        public string LastName
        {
            get { return lastname; }
            set
            {
                if (lastname == value)
                    return;

                lastname = value;
                RaisePropertyChanged();
            }
        }

        private string licensenumber;

        public string LicenseNumber
        {
            get { return licensenumber; }
            set
            {
                if (licensenumber == value)
                    return;
                licensenumber = value;
                RaisePropertyChanged();

            }
        }

        public Player(string LastName = "lastname", string FirstName = "firstname")
        {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.LicenseNumber = string.Empty;
        }
    }
}
