using System.Collections.Generic;
using System.Linq;
using JFP.Core.Caching;
using JFP.Core.Data;
using JFP.Core.Domain.Text;
using JFP.Data;

namespace JFP.Services.Textos
{
    public class TextoServices : ITextoServices
    {
        #region Constants

        private const string TextoPattern = "pj.texto";

        private const string LocalstringresourcesAllKey = "pj.texto.all";

        private const string LocalstringresourcesAllKeyObject = "pj.texto.all.object";
        #endregion

        #region Fields
        private readonly IRepository<Texto> _textoRepository;
        private readonly ICacheManager _cacheManager;        
        #endregion

        #region Ctor
        public TextoServices(ICacheManager cacheManager, IRepository<Texto> textoRepository)
        {
            _cacheManager = cacheManager;
            _textoRepository = textoRepository;
        }
        #endregion

        public virtual Texto GetTextoByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new Texto { Text = "PJ.Text.Null", Id = -1, Descripcion = "Texto no existente" };

            var query = _textoRepository.Table;
            query = query.Where(c => c.NomTexto.Equals(name));

            var texto = query.FirstOrDefault();
            return texto;
        }

        public virtual Texto GetTextoById(int id)
        {
            return id <= 0 ? new Texto { Text = "PJ.Text.Null", Id = -1, Descripcion = "Texto no existente" } : _textoRepository.GetById(id);
        }

        public virtual string GetText(string name)
        {
            var texts = GetAllResourceValues();
            var nameLower = name.ToLowerInvariant();

            return !texts.ContainsKey(nameLower) ? name : texts[nameLower];
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <returns>Locale string resources</returns>
        public virtual Dictionary<string, string> GetAllResourceValues()
        {
            const string key = LocalstringresourcesAllKey;

            return _cacheManager.Get(key, () =>
            {

                var query = from l in _textoRepository.Table
                            orderby l.NomTexto
                            select l;
                var locales = query.ToList();
                //format: <name, <id, value>>
                var dictionary = new Dictionary<string, string>();
                foreach (var locale in locales)
                {
                    var resourceName = locale.NomTexto.ToLowerInvariant();
                    if (!dictionary.ContainsKey(resourceName))
                        dictionary.Add(resourceName, locale.Text);
                }
                return dictionary;
            });
        }

        public virtual IList<Texto> GetAllResource()
        {
            const string key = LocalstringresourcesAllKeyObject;

            return _cacheManager.Get(key, () =>
            {

                var query = from l in _textoRepository.Table
                            orderby l.NomTexto
                            select l;
                var locales = query.ToList();
                return locales;
            });
        }

        public virtual void InsertTexto(Texto texto)
        {
            _textoRepository.Insert(texto);
            _cacheManager.RemoveByPattern(TextoPattern);
        }

        public virtual void InsertTexto(IEnumerable<Texto> textos)
        {
            _textoRepository.Insert(textos);
            _cacheManager.RemoveByPattern(TextoPattern);
        }

        public virtual void UpdateTexto(Texto texto)
        {

            _textoRepository.Update(texto);
            _cacheManager.RemoveByPattern(TextoPattern);
        }

        public virtual void DeleteTexto(Texto texto)
        {

            _textoRepository.Delete(texto);
            _cacheManager.RemoveByPattern(TextoPattern);

        }

        public void DeleteAllText()
        {
            _textoRepository.ExecuteProcedureNonQuery("TRUNCATE TABLE [Texto]");
            _cacheManager.RemoveByPattern(TextoPattern);
        }
    }
}
