using System.ComponentModel.DataAnnotations;

namespace Nexus.Common
{
    public class ValidationConstants
    {
        //Profile
        public const int NameMaxLen = 85;
        public const int NameMinLen = 1;

        public const int AgeMaxValue = 99;
        public const int AgeMinValue = 18;

        public const int CityMaxLen = 85;
        public const int CityMinLen = 2;

        public const int BioMaxLen = 500;

        //Interest
        public const int InterestMaxName = 100;

    }
}
