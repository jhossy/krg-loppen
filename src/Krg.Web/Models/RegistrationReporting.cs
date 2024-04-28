using Umbraco.Cms.Core.PropertyEditors;

namespace Krg.Web.Models
{
	[DataEditor(
    alias: "Registrations editor",
    name: "RegistrationsReporting",
    view: "/App_Plugins/RegistrationReporting/Reporting.html",
    Group = "Common",
    Icon = "icon-list")]
    public class RegistrationReporting : DataEditor
    {
        public RegistrationReporting(IDataValueEditorFactory dataValueEditorFactory)
            : base(dataValueEditorFactory)
        {
        }
    }
}
