using System.Configuration;

namespace MP.WindowsServices.Common.ConfigurationHelper
{
    internal class AppConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("fileFilters")]
        public FileFiltersElement FileFilters
        {
            get { return (FileFiltersElement)this["fileFilters"]; }
        }
        

        [ConfigurationProperty("fileNameTemplate")]
        public FileNameTemplateElement FileNameTemplate
        {
            get { return (FileNameTemplateElement)this["fileNameTemplate"]; }
        }

        [ConfigurationCollection(typeof(ObservableFolderElement), AddItemName = "folder")]
        [ConfigurationProperty("observableFolders")]
        public ConfigElementCollection<ObservableFolderElement> ObservableFolders
        {
            get { return (ConfigElementCollection<ObservableFolderElement>)this["observableFolders"]; }
        }
    }
}
