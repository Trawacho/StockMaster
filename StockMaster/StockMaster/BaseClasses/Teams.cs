using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class Teams: IEnumerable<Team>
    {
        List<Team> teams;
        public Teams()
        {
            this.teams = new List<Team>();
        }

        public void Add(Team team)
        {
            if (teams.Any(x => x.StartNumber == team.StartNumber))
            {
                throw new Exception("Startnumber is already existing. Team not added to list of teams");
            }
            this.teams.Add(team);


        }
        public void Insert(int index, Team team)
        {
            teams.Insert(index, team);
        }
        public void AddVirtualTeam()
        {
            Add(new Team(teams.Count + 1, "Virtual Team", 4)
            {
                IsVirtual = true
            });
        }

        public Team Last
        {
            get
            {
                return teams.Last<Team>();
            }
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
