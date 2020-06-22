using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ozraapi3
{
    public class Sportnik
    {
        public int Rank { get; set; }
        public int id { get; set; }
        public string Name { get; set; }
        public string GenderRank { get; set; }
        public int DivRank { get; set; }
        public string OverallRank { get; set; }
        public int Bib { get; set; }
        public string Division { get; set; }//"30-34"
        public int Age { get; set; }
        public string AgeCategory { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Profession { get; set; }
        public int Points { get; set; }
        public TimeSpan Swim { get; set; }
        public float SwimDistance { get; set; }
        public TimeSpan T1 { get; set; }
        public TimeSpan Bike { get; set; }
        public float BikeDistance { get; set; }
        public TimeSpan T2 { get; set; }
        public TimeSpan Run { get; set; }
        public float RunDistance { get; set; }
        public string Overall { get; set; }

        public TimeSpan Finish { get; set; }

        public int OverAllTri { get; set; }
        public string Comment { get; set; }


        public Sportnik() { }
    }
}
