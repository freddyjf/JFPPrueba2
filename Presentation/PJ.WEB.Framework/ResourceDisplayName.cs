using JFP.Core;
using JFP.Core.Infrastructure;
using JFP.Services.Textos;


namespace JFP.WEB.Framework
{
    public class ResourceDisplayName : System.ComponentModel.DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        //private ITextoServices _localizationService;
        //private bool _resourceValueRetrived;

        public ResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }

        public string ResourceKey { get; set; }

        public override string DisplayName
        {
            get
            {



                //var _localizationService = EngineContext.Current.Resolve<ITextoServices>();
                _resourceValue = "null"; //_localizationService.GetText(ResourceKey);
                return _resourceValue;
            }
        }

        public string Name
        {
            get { return "ResourceDisplayName"; }
        }
    }
}
