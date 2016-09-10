using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace JFP.Core.Config
{
    /// <summary>
    /// Acceso centralizado a los valores de la configuración.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Expresión para detectar archivos xdp.config
        /// </summary>
        static Regex ExpArchivoXDP = new Regex(@"\\xdp(\.\d+)?.config$", RegexOptions.IgnoreCase);

        #region OBTENER LLAVE DEL ARCHIVO XDP

        /// <summary>
        /// Retorna un valor de appSettings como un entero.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static int LlaveInt32(string nombreLlave, int valorDefecto = -1)
        {
            var Item = ObtenerValorLlave(nombreLlave);
            int Resultado = 0;
            if (!int.TryParse(Item, out Resultado))
                Resultado = valorDefecto;
            return Resultado;
        }

        /// <summary>
        /// Retorna un valor de appSettings como un Int64.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static Int64 LlaveInt64(string nombreLlave, Int64 valorDefecto = -1)
        {
            var Item = ObtenerValorLlave(nombreLlave);
            Int64 Resultado = 0;
            if (!Int64.TryParse(Item, out Resultado))
                Resultado = valorDefecto;
            return Resultado;
        }

        /// <summary>
        /// Retorna un valor de appSettings como un Boolean.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static bool LlaveBoolean(string nombreLlave, bool valorDefecto = false)
        {
            var Item = ObtenerValorLlave(nombreLlave);
            bool Resultado = false;
            if (!bool.TryParse(Item, out Resultado))
                Resultado = valorDefecto;
            return Resultado;
        }

        /// <summary>
        /// Retorna un valor de appSettings como un string.
        /// Si no existe se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static string LlaveString(string nombreLlave, string valorDefecto = "")
        {
            string Resultado = ObtenerValorLlave(nombreLlave);
            if (Resultado == null)
                Resultado = valorDefecto;
            return Resultado;
        }

        static XmlDocument _DocumentoXml;
        /// <summary>
        /// El documento XML con las llaves cargadas.
        /// </summary>
        public static XmlDocument DocumentoXml
        {
            get
            {
                if (_DocumentoXml == null)
                    LlaveString("DummyNonExistingKey");
                return _DocumentoXml;
            }
        }

        static StringDictionary _ObtenerComoStringDictionary;
        /// <summary>
        /// Retorna las llaves 'entry' como un StringDictionary.
        /// </summary>
        /// <returns></returns>
        public static StringDictionary ObtenerComoStringDictionary()
        {
            if (_ObtenerComoStringDictionary == null)
            {
                var Lista =
                    from x in
                        JFP.Core.Config.Config.DocumentoXml
                        .SelectNodes("/xdp/parameters/entry").OfType<XmlNode>()
                    select new
                    {
                        id = x.Attributes["name"].Value,
                        valor = x.InnerText
                    };
                _ObtenerComoStringDictionary = new StringDictionary();

                foreach (var item in Lista)
                    _ObtenerComoStringDictionary.Add(item.id, item.valor);
            }
            return _ObtenerComoStringDictionary;
        }

        #endregion

        #region LECTURA DE QUERY STRING

        /// <summary>
        /// Retorna un valor del query string como un entero.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static int QueryStringInt32(string nombreLlave, int valorDefecto = -1)
        {
            var Item = System.Web.HttpContext.Current.Request.QueryString[nombreLlave];
            int Resultado = 0;
            if (!int.TryParse(Item, out Resultado))
                Resultado = valorDefecto;
            return Resultado;
        }

        /// <summary>
        /// Retorna un valor del query string como un Int64.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static Int64 QueryStringInt64(string nombreLlave, Int64 valorDefecto = -1)
        {
            var Item = System.Web.HttpContext.Current.Request.QueryString[nombreLlave];
            Int64 Resultado = 0;
            if (!Int64.TryParse(Item, out Resultado))
                Resultado = valorDefecto;
            return Resultado;
        }

        /// <summary>
        /// Retorna un valor del query string como un Boolean.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static bool? QueryStringBoolean(string nombreLlave, bool? valorDefecto = null)
        {
            var Item = System.Web.HttpContext.Current.Request.QueryString[nombreLlave];

            if (string.IsNullOrWhiteSpace(Item))
                return null;

            bool Resultado = false;
            if (!Boolean.TryParse(Item, out Resultado))
                return valorDefecto;
            else
                return Resultado;
        }

        /// <summary>
        /// Retorna un valor del query string como un Boolean.
        /// Si no existe o no es un entero se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static DateTime? QueryStringDate(string nombreLlave, string formato = "dd/MM/yyyy", DateTime? valorDefecto = null)
        {
            var Item = System.Web.HttpContext.Current.Request.QueryString[nombreLlave];

            if (string.IsNullOrWhiteSpace(Item))
                return null;

            DateTime Resultado = DateTime.Now;

            if (!DateTime.TryParseExact(Item, formato, null,
                System.Globalization.DateTimeStyles.None, out Resultado))
                return valorDefecto;
            else
                return Resultado;
        }

        /// <summary>
        /// Retorna un valor de appSettings como un string.
        /// Si no existe se retorna el valor por defecto.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <param name="valorDefecto"></param>
        /// <returns></returns>
        public static string QueryStringString(string nombreLlave, string valorDefecto = "")
        {
            string Resultado = System.Web.HttpContext.Current.Request.QueryString[nombreLlave];
            if (Resultado == null)
                Resultado = valorDefecto;
            return Resultado;
        }

        #endregion

        #region CARGUE DEL ARCHIVO XDP

        /// <summary>
        /// Retorna un valor de la sección appSettings.
        /// </summary>
        /// <param name="nombreLlave"></param>
        /// <returns></returns>
        static string ObtenerValorLlave(string nombreLlave)
        {
            if (string.IsNullOrWhiteSpace(nombreLlave))
                return null;

            var LlaveMinusc = nombreLlave.ToLower();
            if (Llaves == null)
                return null;
            else if (Llaves.ContainsKey(LlaveMinusc))
                return Llaves[LlaveMinusc];
            else
                return null;
        }

        static Dictionary<string, string> _Llaves = null;
        static object LlavesLock = new object();

        /// <summary>
        /// La lista de las llaves.
        /// </summary>
        static Dictionary<string, string> Llaves
        {

            get
            {
                lock (LlavesLock)
                {
                    if (_Llaves == null || !_Llaves.Any())
                    {
                        XmlDocument NextDoc = null;
                        var xDoc = new XmlDocument();
                        _ObtenerComoStringDictionary = null;
                        var ArchivosXdp = ListaArchivosXDP();

                        if (ArchivosXdp.Any())
                        {
                            _Llaves = new Dictionary<string, string>();
                            foreach (var UnArchivo in ArchivosXdp)
                            {
                                xDoc.Load(UnArchivo);
                                if (NextDoc == null)
                                {
                                    NextDoc = new XmlDocument();
                                    NextDoc.Load(UnArchivo);
                                }

                                var xPathLlaves = xDoc.SelectNodes("/xdp/parameters/entry");
                                if (xPathLlaves.OfType<XmlNode>().Any())
                                {
                                    foreach (var OneItem in xPathLlaves.OfType<XmlNode>())
                                    {
                                        var Nombre = OneItem.Attributes["name"].Value.ToLower();
                                        var Valor = OneItem.InnerText;
                                        if (_Llaves.ContainsKey(Nombre))
                                        {
                                            _Llaves[Nombre] = Valor;
                                            // Fusionar el archivo.
                                            var LlaveExistente = NextDoc.SelectSingleNode(
                                                string.Format("/xdp/parameters/entry [@name = \"{0}\"]", OneItem.Attributes["name"].Value));
                                            if (LlaveExistente != null)
                                            {
                                                // Reemplazarla.
                                                LlaveExistente.InnerText = Valor;
                                            }
                                        }
                                        else
                                            _Llaves.Add(
                                                OneItem.Attributes["name"].Value.ToLower(),
                                                OneItem.InnerText);
                                    }
                                }

                            }
                        }

                        if (ConfigFilesWatcher == null)
                            IniciarMonitoreo();

                        _DocumentoXml = NextDoc;
                    }

                    return _Llaves;
                }
            }
        }

        #endregion

        #region MONITOREO ARCHIVOS

        static FileSystemWatcher ConfigFilesWatcher;

        /// <summary>
        /// Iniciar el monitoreo de cambios a los archivos .config.
        /// </summary>
        static void IniciarMonitoreo()
        {
            ConfigFilesWatcher = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory)
            {
                IncludeSubdirectories = false,
                Filter = "*.config",
                NotifyFilter =
                    NotifyFilters.FileName | NotifyFilters.LastWrite |
                    NotifyFilters.Size | NotifyFilters.CreationTime |
                    NotifyFilters.DirectoryName
            };

            ConfigFilesWatcher.Changed += ConfigFilesWatcher_Detection;
            ConfigFilesWatcher.Created += ConfigFilesWatcher_Detection;
            ConfigFilesWatcher.Deleted += ConfigFilesWatcher_Detection;
            ConfigFilesWatcher.Renamed += ConfigFilesWatcher_Detection;
            ConfigFilesWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Al detectar cambios borrar todas las llaves en memoria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ConfigFilesWatcher_Detection(object sender, FileSystemEventArgs e)
        {
            lock (LlavesLock)
            {
                _Llaves.Clear();
            }
        }

        /// <summary>
        /// Retorna la lista de todos los archivos XDP.
        /// </summary>
        /// <returns></returns>
        static string[] ListaArchivosXDP()
        {
            var ListaArchivos =
                from x in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.config")
                where ExpArchivoXDP.IsMatch(x)
                select x.ToLower();

            if (!ListaArchivos.Any())
                return ListaArchivos.ToArray();
            else
            {
                var Primero = ListaArchivos.Where(x => x.EndsWith("xdp.config"));
                var Otros = ListaArchivos.Where(x => !x.EndsWith("xdp.config")).OrderBy(x => x);
                return Primero.Concat(Otros).ToArray();
            }
        }

        #endregion


        #region Reemplazos generales de texto

        /// <summary>
        /// Hacer reemplazo de cadenas con fines de corregir posibles fallas ortográficas.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SpellingStringReplace(string input)
        {
            if (ReemplazosOrtograficos == null)
            {
                // Cargar la lista de reemplazos.
                ReemplazosOrtograficos = new List<Tuple<Regex, string>>();
                var Parts = Config.LlaveString("ReemplazosOrtograficos").Split('|');
                for (int i = 0; i < Parts.Length; i += 2)
                {
                    ReemplazosOrtograficos.Add(new Tuple<Regex, string>(
                        new Regex(Parts[i], RegexOptions.IgnoreCase | RegexOptions.Singleline),
                        Parts[i + 1]));
                }
            }

            // Hacer los reemplazos.
            ReemplazosOrtograficos.ForEach(x => input = x.Item1.Replace(input, x.Item2));
            return input;
        }

        static List<Tuple<Regex, string>> ReemplazosOrtograficos = null;

        #endregion

        /// <summary>
        /// Expresión que hace match a Request.Browser.Type a los navegadores que NO se soportan.
        /// </summary>
        static Regex ExpUnsupportedBrowserTypes = new Regex(
            Config.LlaveString("BrowserUnsupportedTypes"), RegexOptions.IgnoreCase);

        /// <summary>
        /// Verdadero: El navegador del usuario no es soportado o tiene soporte parcial.
        /// </summary>
        public static bool IsUnsupportedBrowser
        {
            get
            {
                var Browser = System.Web.HttpContext.Current.Request.Browser;
                return ExpUnsupportedBrowserTypes.IsMatch(Browser.Type);
            }
        }


    }

}
