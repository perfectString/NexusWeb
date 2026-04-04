
namespace Nexus.GCommon
{
    public static class OutputMessages
    {
        //Error Messages
        public const string UnexpectedErrorMessage = "An unexpected error occured. Please reach out to site administrator.";
        public const string UnauthorizedErrorMessage = "You are not authrorised to access this content.";
        public const string BadRequestErrorMessage = "Bad request. Check site url or try again from home page.";
        public const string NotFoundErrorMessage = "The content you are looking for was not found.";
        public const string ServerErrorMessage = "There was an error on our end. Try again later.";
        public const string SavingChangesFailMessage = "Saving changes failed. Please try again later.";


        /* View Models */

        //Profile - Display Name
        public const string DisplayNameRequiredMessage = "Display name is required.";
        public const string DisplayNameMaxLenMessage = "Display name cannot exceed {1} characters.";
        public const string DisplayNameMinLenMessage = "Display name must be at least {1} characters long.";

        //Profile - Age
        public const string AgeRangeMessage = "Age must be between {1} and {2} years.";

        //Profile - City
        public const string CityMissingMessage = "City name is required.";
        public const string CityMaxLenMessage = "City name cannot exceed {1} characters.";
        public const string CityMinLenMessage = "City name must be at least {1} characters long.";

        //Profile - Bio
        public const string BioMaxLenMessage = "Bio cannot exceed {1} characters.";

        //Interest - Name
        public const string InterestNameRequiredMessage = "Interest name is required.";
        public const string InterestNameMaxLenMessage = "Interest name cannot exceed {1} characters.";

        //Quest - Title
        public const string QuestTitleRequiredMessage = "Quest title is required.";
        public const string QuestTitleMaxLenMessage = "Quest title cannot exceed {1} characters.";
        public const string QuestTitleMinLenMessage = "Quest title must be at least {1} characters long.";

        //Quest - Description
        public const string QuestDescriptionMaxLenMessage = "Quest description cannot exceed {1} characters.";
        public const string QuestDescriptionMinLenMessage = "Quest description must be at least {1} characters long.";

        /* Controllers */



        public const string AddQuestFailedMessage = "An error occured while adding a new quest. Please try again later.";
        public const string CompletedQuestFailedMessage = "Quest is already completed.";

        public const string ProfileNotFoundMessage = "This profile was not found.";
        public const string CrudExceptionMessage = "An error occured while trying to {0}!";

    }
}
