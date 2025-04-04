﻿namespace Krg.Domain
{
	public class Constants
	{
		public const string NameRequiredMessage = "Navn skal angives";
		public const string EmailRequiredMessage = "Email skal angives";
        public const string EmailInvalidFormat = "Ugyldig email";
        public const string PhoneNoRequiredMessage = "Telefon nr skal angives";
        public const string DepartmentRequiredMessage = "Gren skal angives";
		public const string NoOfAdultsRequiredMessage = "Antal voksne skal angives";
		public const string NoOfChildrenRequiredMessage = "Antal børn skal angives";

		public const string FallBackContactName = "Jette Simonsen";
		public const string FallBackContactPhoneNo = "24448429";
		public const string FallBackContactEmail = "simonsenjette@yahoo.dk";

		public struct Email
		{
			public const string EmailSender = "support@spejderknud.dk";
		}
	}
}
