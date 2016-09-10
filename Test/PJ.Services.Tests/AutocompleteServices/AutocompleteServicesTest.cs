using NUnit.Framework;
using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.AutoComplete;
using LEGIS.PJ.Core.Domain.LineasJurisprudenciales;
using LEGIS.PJ.Services.Autocomplete;
using LEGIS.PJ.Data;


namespace LEGIS.PJ.Tests.Services.AutoCompleteServicesTest
{
    [TestFixture]
    public partial class AutocompleteServicesTest : ServicesTests
    {

        private IRepository<AutoComplete> _repository;
        private IRepository<LineaJurisprudencialSuscriptor> _repositoryLinea;
        private IAutoCompleteServices AutocompleteServices;

        private void InitCaseTest()
        {

            _repository = new EfRepository<AutoComplete>(context);
            _repositoryLinea = new EfRepository<LineaJurisprudencialSuscriptor>(context);
            AutocompleteServices = new AutoCompleteServices(_repository, _repositoryLinea, _cacheManager);
        }

        [Test]
        public void Can_AutoCompleteGen()
        {

            InitCaseTest();
            var lst= AutocompleteServices.getAutocompleteBySuscriptor(8);
            lst.ShouldNotBeNull();
        }
    }
}
