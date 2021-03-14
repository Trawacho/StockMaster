using StockMaster.Interfaces;

namespace StockMaster.BaseClasses
{
    public class TExecutiveClass : TBaseClass, IExecutive
    {

        private string _Name;
        private string _ClubName;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name == value) return;

                _Name = value;
                RaisePropertyChanged();
            }
        }
        public string ClubName
        {
            get
            {
                return _ClubName;
            }
            set
            {
                if (_ClubName == value) return;

                _ClubName = value;
                RaisePropertyChanged();
            }
        }
    }
}
