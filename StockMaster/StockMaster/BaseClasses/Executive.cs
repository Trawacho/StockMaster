using StockMaster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class ExecutiveBaseClass : TBaseClass, IExecutive
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

    public class Referee : ExecutiveBaseClass
    {
    }

    public class ComputingOfficer : ExecutiveBaseClass
    {
    }

    public class CompetitionManager : ExecutiveBaseClass
    {

    }
}
