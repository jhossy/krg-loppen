using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Core.PropertyEditors;

namespace Krg.Web.Composers
{
    public class RegistrationReportingComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.ManifestFilters().Append<RegistrationFilter>();
        }
    }

    public class RegistrationFilter : IManifestFilter
    {
        private readonly IDataValueEditorFactory _dataValueEditorFactory;

        public RegistrationFilter(IDataValueEditorFactory dataValueEditorFactory)
        {
            _dataValueEditorFactory = dataValueEditorFactory;
        }

        public void Filter(List<PackageManifest> manifests)
        {
            manifests.Add(new PackageManifest
            {
                PackageName = "Registrations",
                Scripts = new[]
                {
                    "/App_Plugins/RegistrationReporting/reporting.controller.js"
                },
                Stylesheets = new[] { "/App_Plugins/RegistrationReporting/reporting.css" },
                PackageView = "/App_Plugins/RegistrationReporting/reporting.html",
                Version = "1.0.0"
            });
        }
    }
}
