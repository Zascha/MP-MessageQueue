using System.Configuration;

namespace MP.WindowsServices.Common.ConfigurationHelper
{
    internal interface IConfigurationCollectionElement
    {
        object Key { get; }
    }

    internal class ObservableFolderElement : ConfigurationElement, IConfigurationCollectionElement
    {
        [ConfigurationProperty("path", IsKey = true, IsRequired = true)]
        public string Path => (string)base["path"];

        public object Key => Path;
    }

    internal class FileFiltersElement : ConfigurationElement
    {
        [ConfigurationProperty("filters")]
        public string Filters
        {
            get { return (string)this["filters"]; }
        }
    }

    internal class FileNameTemplateElement : ConfigurationElement
    {
        [ConfigurationProperty("template")]
        public string Template
        {
            get { return (string)this["template"]; }
        }
    }
}
