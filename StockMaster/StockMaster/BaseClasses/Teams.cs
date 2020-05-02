using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Teams : TBaseClass, IEnumerable<Team> //, IQueryable<Team>
    {
        readonly List<Team> teams;
        public Teams()
        {
            this.teams = new List<Team>();
        }

        /// <summary>
        /// Hinzufügen eines Teams, es werden alle Virtuellen Teams gelöscht, es werden alle Spiele der Teams gelöscht
        /// </summary>
        /// <param name="team"></param>
        public void Add(Team team)
        {
            if (team.StartNumber == 0)
            {
                teams.RemoveAll(t => t.IsVirtual);
            }
            Parallel.ForEach(teams, (t) => t.Games.Clear());

            //Re-Organize Startnumbers
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].StartNumber = i + 1;
            }

            team.StartNumber = teams.Count + 1;

            this.teams.Add(team);
            team.PropertyChanged += Team_PropertyChanged;
            RaisePropertyChanged(nameof(Teams));

        }

        private void Team_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Teams));
        }

        public void Insert(int index, Team team)
        {
            teams.Insert(index, team);
            RaisePropertyChanged(nameof(Teams));

        }

        public void Remove(Team team)
        {
            teams.Remove(team);
            //Re-Organize Startnumbers
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].StartNumber = i + 1;
            }
            RaisePropertyChanged(nameof(Teams));
        }


        public void AddVirtualTeam()
        {
            Add(new Team("Virtual Team")
            {
                IsVirtual = true
            });
            RaisePropertyChanged(nameof(Teams));
        }

        public Team GetByStartnummer(int StartNumber)
        {
            return teams.First(x => x.StartNumber == StartNumber);
        }

        public Team GetTeamWithHighestStartNumber()
        {
            var startnumbers = teams.Select(x => x.StartNumber);
            int highest = startnumbers.Max();
            return teams.First(x => x.StartNumber == highest);
        }

        public IEnumerator<Team> GetEnumerator()
        {
            return ((IEnumerable<Team>)teams).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Team>)teams).GetEnumerator();
        }

        public Team this[int index]
        {
            get
            {
                return teams[index];
            }
            set
            {
                teams[index] = value;
                RaisePropertyChanged(nameof(Teams));

            }
        }

        public int Count
        {
            get
            {
                return this.teams.Count;
            }
        }


    }
}
