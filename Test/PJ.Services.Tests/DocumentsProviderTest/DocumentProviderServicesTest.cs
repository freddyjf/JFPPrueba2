using LEGIS.PJ.Core.Data;
using LEGIS.PJ.Core.Domain.DocumentProvider;
using LEGIS.PJ.Data;
using LEGIS.PJ.Services.DocumentsProvider;
using NUnit.Framework;

namespace LEGIS.PJ.Tests.Services.DocumentsProviderTest
{
    public class DocumentProviderServicesTest : ServicesTests
    {
        IRepository<DocumentProvider> _documentRepository;

        IDocumentProviderServices _documentProviderServices;

        public void InitCaseTest()
        {
            _documentRepository = new EfRepository<DocumentProvider>(context);
            _documentProviderServices = new DocumentProviderServices(_documentRepository);
        }

        [Test]
        public void CanInsertDocument()
        {
            InitCaseTest();

            var document = new DocumentProvider
            {
                alias = "jurcol",
                contexto = "jurcol_bd5a5da12cf6022ae0430a010151022a",
                server = "1",
                estado = 0
            };

            _documentProviderServices.insertDocument(document);
        }
    }
}
