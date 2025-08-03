namespace Krg.Website.Models;
public struct Translations
{
    public struct Menu
    {
        public const string HomeLabel = "Forside";
        
        public const string EventDatesLabel = "Datoer";

        public const string RegistrationsLabel = "Tilmeldinger";

        public const string UsersLabel = "System brugere";
        
        public const string ProfileLabel = "Profil";
        
        public const string SignOutLabel = "Log af";
    }
    
    public struct Home
    {
        public const string Welcome = "Velkommen, {user}";
    }
    
    public struct Events
    {
        public const string EventDetailsHeader = "Detaljer";
        
        public const string EventOverview = "Oversigt";
        
        public const string DateLabel = "Dato";
        
        public const string NameLabel = "Navn";
        
        public const string PhoneLabel = "Telefon";
        
        public const string EmailLabel = "Email";
        
        public const string NoteLabel = "Kommentar";

        public const string SaveButtonText = "Gem";
        
        public const string CancelButtonText = "Annuller";
        
        public const string CreateButtonText = "Opret";
        
        public const string EditButtonText = "Rediger";
        
        public const string DeleteButtonText = "Slet";
        
        public const string SelectYear = "Vælg år";
        
        public const string AreYouSure = "Er du sikker?";
    }

    public struct Profile 
    {
        public const string ProfileHeader = "Bruger profil";

        public const string PlaceholderPassword = "Adgangskode";
        
        public const string PlaceholderCurrentPassword = "Nuværende adgangskode";

        public const string PlaceholderNewPassword = "Ny adgangskode";

        public const string PlaceholderRepeatPassword = "Gentag adgangskode";

        public const string ButtonUpdateText = "Opdater";

        public const string UserDoesNotExist = "Ingen bruger findes med den indtastede email";

        public const string InvalidPassword = "Ugyldig adgangskode - indtast brugerens nuværende adgangskode";

        public const string NewAndRepeatPasswordMismatch = "'Nuværende' og 'ny' adgangskode er ikke ens";

        public const string PasswordUpdated = "Adgangskoden er opdateret!";
    }

    public struct Registrations 
    {
        public const string RegistrationsHeader = "Tilmeldinger";

        public const string ButtonExportText = "Eksporter";

        public const string ButtonExportGroupedText = "Grupperet eksport";

        public const string ButtonFetchRegistrationsText = "Hent";

        public const string ButtonDeleteRegistrationsText = "Slet";

        public const string AreYouSure = "Er du sikker?";

        public const string EndDateValidationMessage = "'Slutdato' skal være efter 'startdato'";
    }

    public struct User
    {
        public const string CreateUserHeader = "Opret system bruger";

        public const string PlaceholderPassword = "Adgangskode";

        public const string PlaceholderRepeatPassword = "Gentag ny adgangskode";

        public const string OverviewHeader = "Oversigt";

        public const string AreYouSure = "Er du sikker?";

        public const string ButtonDeleteText = "Slet";

        public const string ButtonResetPasswordText = "Nulstil adgangskode";

        public const string ButtonCreateUserText = "Opret";

        public const string UserAlreadyExists = "En bruger med {email} findes allerede i systemet";

        public const string PasswordRepeatPasswordMismatch = "'Adgangskode' og 'gentag adgangskode' er ikke ens";

        public const string UserWithIdNotFound = "Brugeren kan ikke findes - prøv igen";

        public const string ResetPasswordSuccess = "Adgangskoden blev ændret til: {newPassword} for brugeren med email: {email}";

        public const string ResetPasswordFailed = "Adgangskoden kunne ikke nulstilles for brugeren med id: {id}";
        
        public const string Activate = "Aktiver";
        
        public const string Deactivate = "De-aktiver";
    }
}