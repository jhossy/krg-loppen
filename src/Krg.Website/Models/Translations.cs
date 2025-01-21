namespace Krg.Website.Models;
public struct Translations
{
    public struct Events
    {
        public const string EventDetailsHeader = "Detaljer";
        
        public const string EventOverview = "Oversigt";
        
        public const string DateLabel = "Dato";
        
        public const string NameLabel = "Navn";
        
        public const string PhoneLabel = "Telefon";
        
        public const string EmailLabel = "Email";

        public const string SaveButtonText = "Gem";
        
        public const string CancelButtonText = "Annuller";
        
        public const string CreateButtonText = "Opret";
        
        public const string EditButtonText = "Rediger";
        
        public const string DeleteButtonText = "Slet";
        
        public const string SelectYear = "Vælg år";
        
        public const string AreYouSure = "Er du sikker på at du vil slette?";
    }

    public struct Profile 
    {
        public const string ProfileHeader = "Bruger profil";

        public const string PlaceholderCurrentPassword = "Nuværende password";

        public const string PlaceholderNewPassword = "Nyt password";

        public const string PlaceholderRepeatPassword = "Gentag password";

        public const string ButtonUpdateText = "Opdater";
    }

    public struct Registrations 
    {
        public const string RegistrationsHeader = "Tilmeldinger";

        public const string ButtonExportText = "Eksporter";

        public const string ButtonExportGroupedText = "Grupperet eksport";

        public const string ButtonFetchRegistrationsText = "Hent";

        public const string ButtonDeleteRegistrationsText = "Slet";

        public const string AreYouSure = "Er du sikker på at du vil slette?";
    }

    public struct User
    {
        public const string CreateUserHeader = "Opret system bruger";

        public const string PlaceholderRepeatPassword = "Gentag nyt password";

        public const string OverviewHeader = "Oversigt";

        public const string AreYouSure = "Er du sikker?";

        public const string ButtonDeleteText = "Slet";

        public const string ButtonResetPasswordText = "Nulstil password";

        public const string ButtonCreateUserText = "Opret";
    }
}