using StockMaster.BaseClasses;
using StockMaster.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace StockMaster.Models
{
    public class TournamentModel : TBaseClass, INotifyPropertyChanged
    {
       

       

        public TournamentModel()
        {
            //CreateNewTournament();
            
            
        }

       

        public Tournament Tournament { get; set; }
        

        

        
    }
}
