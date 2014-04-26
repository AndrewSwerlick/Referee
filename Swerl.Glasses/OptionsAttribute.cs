using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Swerl.Glasses.Providers;

namespace Swerl.Glasses
{
    public class OptionsAttribute : Attribute, IMetadataAware
    {
        public Type ViewOptionsProviderType { get; set; }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var provider = (IViewOptionsProvider) DependencyResolver.Current.GetService(ViewOptionsProviderType);
            metadata.AdditionalValues["Options"] = provider.BuildOptions();
        }
    }
}
