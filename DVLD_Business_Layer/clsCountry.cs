using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsCountry
    {
        public int ID { get; set; }
        public string CountryName { get; set; }

        public clsCountry() 
        {
            this.ID = -1;
            this.CountryName = "";
        }
        private clsCountry(int iD, string countryName)
        {
            this.ID = iD;
            this.CountryName = countryName;
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = "";
            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))
            {
                return new clsCountry(ID, CountryName);
            }
            else
                return null;
        }
        public static clsCountry Find(string CountryName)
        {
            int ID = -1;
            if (clsCountryData.GetCountryInfoByName(ref ID, CountryName))
            {
                return new clsCountry(ID, CountryName);
            }
            else
                return null;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }
    }
}
