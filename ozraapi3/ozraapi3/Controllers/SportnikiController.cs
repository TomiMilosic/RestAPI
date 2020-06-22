using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ozraapi3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SportnikiController : ControllerBase
    {
        public static string cs = @"server=localhost;userid=root;database=bazaozra";
        public MySqlConnection con = new MySqlConnection(cs);

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public Sportnik ParseSportnik(MySqlDataReader beripodatke)
        {

            Sportnik sportnik = new Sportnik();
            sportnik.id = Convert.ToInt32(beripodatke["id"]);
            sportnik.Name = beripodatke["Name"].ToString();
            sportnik.Rank = Convert.ToInt32(beripodatke["Rank"]);
            sportnik.GenderRank = beripodatke["GenderRank"].ToString();
            sportnik.DivRank = Convert.ToInt32(beripodatke["DivRank"]);
            sportnik.OverallRank = beripodatke["OveralRank"].ToString();
            sportnik.Bib = Convert.ToInt32(beripodatke["Bib"]);
            sportnik.Division = beripodatke["Division"].ToString();
            sportnik.Age = Convert.ToInt32(beripodatke["Age"]);
            sportnik.AgeCategory = beripodatke["AgeCategory"].ToString();
            sportnik.State = beripodatke["State"].ToString();
            sportnik.Country = beripodatke["Country"].ToString();
            sportnik.Profession = beripodatke["Profession"].ToString();
            sportnik.Points = Convert.ToInt32(beripodatke["Points"]);
            sportnik.Swim = TimeSpan.Parse(beripodatke["Swim"].ToString());
            sportnik.SwimDistance = float.Parse(beripodatke["SwimDistance"].ToString());
            sportnik.T1 = TimeSpan.Parse(beripodatke["T1"].ToString());
            sportnik.Bike = TimeSpan.Parse(beripodatke["Bike"].ToString());
            sportnik.BikeDistance = float.Parse(beripodatke["BikeDistance"].ToString());
            sportnik.T2 = TimeSpan.Parse(beripodatke["T2"].ToString());
            sportnik.Run = TimeSpan.Parse(beripodatke["Run"].ToString());
            sportnik.RunDistance = float.Parse(beripodatke["RunDistance"].ToString());
            sportnik.Overall = beripodatke["Overall"].ToString();
            sportnik.Finish = TimeSpan.Parse(beripodatke["Finish"].ToString());
            sportnik.OverAllTri = Convert.ToInt32(beripodatke["OverAllTri"]);
            sportnik.Comment = beripodatke["Comment"].ToString();

            return sportnik;


        }



        /// <summary>
        /// Pridobivanje vseh sportnikov iz baze
        /// </summary>
        /// <returns>vrne prvih 5 sportnikov</returns>
        [HttpGet]
        public ObservableCollection<Sportnik> Get()//branje vseh sportnikov 
        {
            ObservableCollection<Sportnik> sportniki = new ObservableCollection<Sportnik>();
            var zazeni = new MySqlCommand("SELECT * FROM sportniki LIMIT 5", con);//Limit je nastavljen na 5
            con.Open();
            zazeni.ExecuteNonQuery();
            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            while (beripodatke.Read())
            {
                sportniki.Add(ParseSportnik(beripodatke));
            }

            beripodatke.Close();
            return sportniki;//OK
        }


        /// <summary>
        /// Pridobivanje sportnikov po id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>vrne posemeznuga sportnika glede na vnesen id</returns>
        [HttpGet("{id}")]
        public ActionResult<Sportnik> Get(int id) //pridobitev sportnika po id
        {
            var zazeni = new MySqlCommand("SELECT * FROM sportniki where id=" + id, con);
            con.Open();
            zazeni.ExecuteNonQuery();

            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            Sportnik sportnik = new Sportnik();
            while (beripodatke.Read())
            {
                sportnik = ParseSportnik(beripodatke);
            }
            beripodatke.Close();


            return sportnik;

        }



        /// <summary>
        /// Doda sportnika v bazo
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPost]
        public bool DodajanjeSportnika(string Rank, string Name, string GenderRank, string DivRank, string OverallRank, string Bib, string Division, string Age, string AgeCategory, string State, string Country, string Profession, string Points, string Swim, string SwimDistance, string T1, string Bike, string BikeDistance, string T2, string Run, string RunDistance, string Overall, string Finish, string OverAllTri, string Comment)//dodajanje spoertnika
        {
            int stevilo = 0;
            TimeSpan cas;
            float float1;
            Sportnik sportnik = new Sportnik();

            if (int.TryParse(Rank, out stevilo)) sportnik.Rank = stevilo;
            sportnik.Name = Name;
            sportnik.GenderRank = GenderRank;
            if (int.TryParse(DivRank, out stevilo)) sportnik.DivRank = stevilo;
            sportnik.OverallRank = OverallRank;
            if (int.TryParse(Bib, out stevilo)) sportnik.Bib = stevilo;
            sportnik.Division = Division;
            if (int.TryParse(Age, out stevilo)) sportnik.Age = stevilo;
            sportnik.AgeCategory = AgeCategory;
            sportnik.State = State;
            sportnik.Country = Country;
            sportnik.Profession = Profession;
            if (int.TryParse(Points, out stevilo)) sportnik.Points = stevilo;
            if (TimeSpan.TryParse(Swim, out cas)) sportnik.Swim = cas;
            if (float.TryParse(SwimDistance, out float1)) sportnik.SwimDistance = float1;
            if (TimeSpan.TryParse(T1, out cas)) sportnik.T1 = cas;
            if (TimeSpan.TryParse(Bike, out cas)) sportnik.Bike = cas;
            if (float.TryParse(BikeDistance, out float1)) sportnik.BikeDistance = float1;
            if (TimeSpan.TryParse(T2, out cas)) sportnik.T2 = cas;
            if (TimeSpan.TryParse(Run, out cas)) sportnik.Run = cas;
            if (float.TryParse(RunDistance, out float1)) sportnik.RunDistance = float1;
            sportnik.Overall = Overall;
            if (TimeSpan.TryParse(Finish, out cas)) sportnik.Finish = cas;
            if (int.TryParse(OverAllTri, out stevilo)) sportnik.OverAllTri = stevilo;
            sportnik.Comment = Comment;
            var zazeni = new MySqlCommand($"INSERT INTO sportniki VALUES(null,'{sportnik.Rank}','{sportnik.Name}','{sportnik.GenderRank}','{sportnik.DivRank}','{sportnik.OverallRank}','{sportnik.Bib}','{sportnik.Division}','{sportnik.Age}','{sportnik.AgeCategory}','{sportnik.State}','{sportnik.Country}','{sportnik.Profession}','{sportnik.Points}','{sportnik.Swim}','{sportnik.SwimDistance}','{sportnik.T1}','{sportnik.Bike}','{sportnik.BikeDistance}','{sportnik.T2}','{sportnik.Run}','{sportnik.RunDistance}','{sportnik.Overall}','{sportnik.Finish}','{sportnik.OverAllTri}','{sportnik.Comment}')", con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();

            return true;

        }


        /// <summary>
        /// Urejanje sportnika glede na id na nove vrednosti vnesene preko parametra
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPut]//tun tudi pazi id
        public bool UrejanjeSportnika(string Rank, string id, string Name, string GenderRank, string DivRank, string OverallRank, string Bib, string Division, string Age, string AgeCategory, string State, string Country, string Profession, string Points, string Swim, string SwimDistance, string T1, string Bike, string BikeDistance, string T2, string Run, string RunDistance, string Overall, string Finish, string OverAllTri, string Comment) //urejanje sportnika po id
        {
            int stevilo = 0;
            TimeSpan cas;
            float float1;
            Sportnik sportnik = new Sportnik();

            if (int.TryParse(Rank, out stevilo)) sportnik.Rank = stevilo;
            if (int.TryParse(id, out stevilo)) sportnik.id = stevilo;
            sportnik.Name = Name;
            sportnik.GenderRank = GenderRank;
            if (int.TryParse(DivRank, out stevilo)) sportnik.DivRank = stevilo;
            sportnik.OverallRank = OverallRank;
            if (int.TryParse(Bib, out stevilo)) sportnik.Bib = stevilo;
            sportnik.Division = Division;
            if (int.TryParse(Age, out stevilo)) sportnik.Age = stevilo;
            sportnik.AgeCategory = AgeCategory;
            sportnik.State = State;
            sportnik.Country = Country;
            sportnik.Profession = Profession;
            if (int.TryParse(Points, out stevilo)) sportnik.Points = stevilo;
            if (TimeSpan.TryParse(Swim, out cas)) sportnik.Swim = cas;
            if (float.TryParse(SwimDistance, out float1)) sportnik.SwimDistance = float1;
            if (TimeSpan.TryParse(T1, out cas)) sportnik.T1 = cas;
            if (TimeSpan.TryParse(Bike, out cas)) sportnik.Bike = cas;
            if (float.TryParse(BikeDistance, out float1)) sportnik.BikeDistance = float1;
            if (TimeSpan.TryParse(T2, out cas)) sportnik.T2 = cas;
            if (TimeSpan.TryParse(Run, out cas)) sportnik.Run = cas;
            if (float.TryParse(RunDistance, out float1)) sportnik.RunDistance = float1;
            sportnik.Overall = Overall;
            if (TimeSpan.TryParse(Finish, out cas)) sportnik.Finish = cas;
            if (int.TryParse(OverAllTri, out stevilo)) sportnik.OverAllTri = stevilo;
            sportnik.Comment = Comment;
            var zazeni = new MySqlCommand($"UPDATE sportniki SET Rank='{sportnik.Rank}',Name='{sportnik.Name}',GenderRank='{sportnik.GenderRank}',DivRank='{sportnik.DivRank}',OveralRank='{sportnik.OverallRank}',Bib='{sportnik.Bib}',Division='{sportnik.Division}',Age='{sportnik.Age}',AgeCategory='{sportnik.AgeCategory}',State='{sportnik.State}',Country='{sportnik.Country}',Profession='{sportnik.Profession}',Points='{sportnik.Points}',Swim='{sportnik.Swim}',SwimDistance='{sportnik.SwimDistance}',T1='{sportnik.T1}',Bike='{sportnik.Bike}',BikeDistance='{sportnik.BikeDistance}',T2='{sportnik.T2}',Run='{sportnik.Run}',RunDistance='{sportnik.RunDistance}',Overall='{sportnik.Overall}',Finish='{sportnik.Finish}',OverAllTri='{sportnik.OverAllTri}',Comment='{sportnik.Comment}' WHERE id=" + sportnik.id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();

            return true;//se ni ok
        }


        /// <summary>
        /// Izbrise sportnika glede na podan id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpDelete("sportniki/{id}")]
        public bool SportnikDelete(int id)//Brisanje sportnika po id
        {
            var zazeni = new MySqlCommand("DELETE FROM sportniki WHERE id=" + id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            beripodatke.Close();

            return true;//ok
        }


        /// <summary>
        /// Pridobivanje vseh adminov
        /// </summary>
        /// <returns>Vrne seznam adminov</returns>
        [HttpGet("admin")]
        public ActionResult<Admin> GetAllAdmin()//pridobitev vseh adminov
        {

            var zazeni = new MySqlCommand("SELECT * FROM admin", con);//Limit je nastavljen na 5
            con.Open();
            zazeni.ExecuteNonQuery();

            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            Admin admin = new Admin();
            while (beripodatke.Read())
            {
                admin.id = Convert.ToInt32(beripodatke["id"]);
                admin.UporabniskoIme = beripodatke["UporabniskoIme"].ToString();
                admin.Geslo = beripodatke["Geslo"].ToString();
            }
            beripodatke.Close();


            return admin;
        }

        /// <summary>
        /// Dodajanje novega admina
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPost("admin/{UporabniskoIme}/{geslo}")]
        public bool DodajanjeAdmina(string UporabniskoIme, string geslo)//Dodajanje admina
        {
            Admin admin = new Admin() { UporabniskoIme=UporabniskoIme, Geslo=geslo};

            var zazeni = new MySqlCommand($"INSERT INTO admin(id, UporabniskoIme, Geslo) VALUES(null,'{admin.UporabniskoIme}','{admin.Geslo}')", con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();

            return true; //dela zdaj
        }


        /// <summary>
        /// Brisanje admina glede na podan id
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpDelete("admin/{id}")]
        public bool BrisanjeAdmina(int id)//Brisanje admina po id
        {
            var zazeni = new MySqlCommand("DELETE FROM admin WHERE id=" + id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            beripodatke.Close();

            return true; //tudi dela!
        }

        /// <summary>
        /// Urejevanje Admina glede na podane parametre
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPut("admin/{id}/{name}/{geslo}")]
        public bool PoosodablanjeAdmina(int id, string name, string geslo)//poosodabljanje admina po ID
        {
            var zazeni = new MySqlCommand($"UPDATE admin SET UporabniskoIme='{name}', Geslo='{geslo}' where id=" + id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();
            return true;
        }


        /// <summary>
        /// Pridobivanje vseh dogotkov
        /// </summary>
        /// <returns>Vrne seznam vseh dogodkov</returns>
        [HttpGet("dogodek")]
        public List<Dogodek> GetDogodek()
        {

            List<Dogodek> dogodki = new List<Dogodek>();
            int time ;
            var zazeni = new MySqlCommand("SELECT * FROM dogodki LIMIT 5", con);//Limit je nastavljen na 5
            con.Open();
            zazeni.ExecuteNonQuery();
            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            while (beripodatke.Read())
            {
                Dogodek dogodek = new Dogodek();
                dogodek.id = Convert.ToInt32(beripodatke["id"]);
                dogodek.naziv = beripodatke["naziv"].ToString();
                if (int.TryParse(beripodatke["cas"].ToString(), out time))
                {
                    dogodek.cas = time;
                }

                dogodki.Add(dogodek);
                
            }

            beripodatke.Close();
            return dogodki;

           
        }//pridobitev vseh dogodtkov

        /// <summary>
        /// Pridobivanje dogodka po id
        /// </summary>
        /// <returns>Vrne posamezen dogodek</returns>

        [HttpGet("dogodek/{id}")]
        public Dogodek GetDogodekPoId(int id)
        {
            var zazeni = new MySqlCommand("SELECT * FROM dogodki where id=" + id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            MySqlDataReader beripodatke = zazeni.ExecuteReader();
            Dogodek dogodek = new Dogodek();
            while (beripodatke.Read())
            {
                dogodek.id = Convert.ToInt32(beripodatke["id"]);
                dogodek.naziv = beripodatke["naziv"].ToString();
                dogodek.cas = Convert.ToInt32(beripodatke["cas"]);
            }
            beripodatke.Close();
            return dogodek;
        }//pridobivanje id po ID


        /// <summary>
        /// Dodajanje dogodka v bazo
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPost("dogodek/{naziv}/{cas}")]
        public bool DodajDogodek(string naziv, string cas)
        {
            int leto;
            int.TryParse(cas, out leto);
            var zazeni = new MySqlCommand($"INSERT INTO dogodek VALUES(null,'{naziv}',' {leto}')", con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();
            return true;
        }//Dodajanje dogodka

        /// <summary>
        /// Urejevanje dogodka po id in glede na parametre
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPut("dogodek/{id}/{naziv}/{cas}")]
        public bool PoosodobiDogodek(int id, string naziv, string cas)
        {
            int leto;
            int.TryParse(cas, out leto);
            var zazeni = new MySqlCommand($"UPDATE dogodek SET naziv='{naziv}', cas='{leto}' WHERE id="+id , con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();
            return true;
            //return true;// cist ok!
           


        }// Urejevanje dogodka

        /// <summary>
        /// Brisanje dogodka glede na id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpDelete("dogodek/{id}")]
        public bool DogodekDelete(int id)//odtranjevanje dogodka po id
        {
            var zazeni = new MySqlCommand("DELETE FROM dogodek WHERE id="+id, con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();

            return true;
        } //Odstranjevanje dogodka

        /// <summary>
        /// Pisanje v evidenco glede prijave v sistem
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPost("evidenca/{id}")]
        public bool EvidencaPrijave(int id)
        {
            var zazeni = new MySqlCommand($"INSERT INTO evidenca VALUES(null,'{id}','{DateTime.Now.TimeOfDay}','{DateTime.Today}')", con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();


            return true;
        }//spremljanje logina adminov

        /// <summary>
        /// Dodajanje sportnika na dogodek
        /// </summary>
        /// <returns>Vrne potrdilo o izvedeni akcija/true</returns>
        [HttpPost("prijava/{idDogodka}/{idSportnika}")]
        public bool PrijavaNaDogodek(int idDogodka, int idSportnika)
        {
            var zazeni = new MySqlCommand($"INSERT INTO PrijavaNaDogodek VALUES(null,'{idDogodka}','{idSportnika}')", con);
            con.Open();
            zazeni.ExecuteNonQuery();
            con.Close();
            return true;

        }


    }
}
