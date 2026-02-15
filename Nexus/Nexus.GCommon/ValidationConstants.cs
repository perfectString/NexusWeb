using System.ComponentModel.DataAnnotations;

namespace Nexus.GCommon
{
    public class ValidationConstants
    {
        //Profile
        public const int DisplayNameMaxLen = 85;
        public const int DisplayNameMinLen = 1;

        public const int AgeMaxValue = 99;
        public const int AgeMinValue = 18;

        public const int CityMaxLen = 85;
        public const int CityMinLen = 2;

        public const int BioMaxLen = 500;

        //Interest
        public const int InterestMaxName = 100;

        //Quests
        public const int TitleMaxLen = 25;
        public const int TitleMinLen = 2;

        public const int DescriptionMaxLen = 500;
        public const int DescriptionMinLen = 5;

    }
}
