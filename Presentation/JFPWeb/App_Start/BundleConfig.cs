using System.Web.Optimization;

namespace Precedente
{
    public static class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Areas

            #region Banner

            bundles.Add(new ScriptBundle("~/area/banner/create-edit").Include(
                "~/Areas/Admin/Scripts/Shared/hide-image-recurso.js"
            ));

            #endregion

            #region Idioma

            bundles.Add(new ScriptBundle("~/area/idioma/index").Include(
                "~/Areas/Admin/Scripts/Idioma/jsonUploader.js"
            ));

            #endregion

            #region Shared

            bundles.Add(new ScriptBundle("~/area/edit-shared").Include(
                "~/Areas/Admin/Scripts/Shared/edit-global.js"
            ));

            #endregion

            #endregion

            #region Angular

            bundles.Add(new ScriptBundle("~/js/angular").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-sanitize.js",
                    "~/Scripts/angular-locale_es-co.js",
                    "~/Scripts/angular-tree-repeat.js",
                    "~/Scripts/angular-chosen.js",
                    "~/Scripts/angular-ui/ui-bootstrap-custom-{version}.js",
                    "~/Scripts/angular.utilities.js",
                    "~/Scripts/misc/Shared/app.js"
                    ));

            #endregion

            #region blueimp Gallery

            bundles.Add(new ScriptBundle("~/js/blueimp.gallery").Include(
                    "~/Scripts/blueimp/Gallery/blueimp-gallery.js",
                    "~/Scripts/blueimp/Gallery/blueimp-gallery-fullscreen.js",
                    "~/Scripts/blueimp/Gallery/blueimp-gallery-indicator.js",
                    "~/Scripts/blueimp/Gallery/blueimp-gallery-video.js",
                    "~/Scripts/blueimp/Gallery/blueimp-gallery-youtube.js",
                    "~/Scripts/blueimp/Gallery/blueimp-gallery-vimeo.js",
                    "~/Scripts/blueimp/Gallery/jquery.blueimp-gallery.js"
            ));

            #endregion

            #region BootBox

            bundles.Add(new ScriptBundle("~/js/bootbox").Include(
                    "~/Scripts/bootbox.js"));

            #endregion

            #region Bootstrap

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/Content/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));


            bundles.Add(new ScriptBundle("~/js/bootstrap-table").Include(
                   "~/Scripts/bootstrap-table.js",
                   "~/Scripts/bootstrap-table-es-ES.js"
                   ));

            bundles.Add(new StyleBundle("~/Content/bootstrap-table-css").Include(
                 "~/Content/bootstrap-table.css"));

            #endregion

            #region Bootstrap TabCollapse

            bundles.Add(new ScriptBundle("~/js/bootstrap-tapcollapse").Include(
                      "~/Scripts/bootstrap-tabcollapse.js"));

            #endregion

            #region Bootstrap DropDown Enhacements

            bundles.Add(new ScriptBundle("~/js/bootstrap-dropdownenhacements").Include(
                      "~/Scripts/dropdowns-enhancement.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap-dropdownenhacements").Include(
                      "~/Content/dropdowns-enhancement.css"));

            #endregion

            #region Bootstrap TabDrop

            bundles.Add(new ScriptBundle("~/js/bootstrap-td").Include(
                   "~/Scripts/bootstrap-tabdrop.js"));

            #endregion

            #region Bootstrap Tour

            bundles.Add(new StyleBundle("~/css/bootstrap-tour").Include(
                    "~/Content/bootstrap-tour.min.css"
                   ));

            bundles.Add(new ScriptBundle("~/js/bootstrap-tour").Include(
                    "~/Scripts/bootstrap-tour.min.js"
                   ));

            #endregion

            #region Chosen

            bundles.Add(new ScriptBundle("~/js/chosen").Include(
                    "~/Scripts/chosen.js"
                    ));
            #endregion

            #region TypeaHead
            bundles.Add(new ScriptBundle("~/js/typeahead").Include(
             "~/Scripts/bootstrap3-typeahead.js"
             ));

            #endregion

            #region Custom Scripts

            #region global

            bundles.Add(new ScriptBundle("~/js/components").Include(
                 "~/Scripts/spin.js",
                 "~/Scripts/utils.js"
            ));

            bundles.Add(new ScriptBundle("~/js/global").Include(
                    "~/Scripts/misc/Shared/global.js"
            ));

            #endregion

            #region Perfil

            bundles.Add(new ScriptBundle("~/js/perfil").Include(
                    "~/Scripts/misc/Perfil/Controllers/perfilController.js",
                    "~/Scripts/misc/Perfil/Controllers/notificacionesController.js",
                    "~/Scripts/misc/Perfil/Factories/perfilFactory.js"
                     ));

            #endregion

            #region AnalisisJuridico

            bundles.Add(new ScriptBundle("~/js/analisisjuridico-favoritos").Include(
                      "~/Scripts/misc/Shared/Factories/favoritosFactory.js",
                      "~/Scripts/misc/AnalisisJuridico/Index/Directives/agregarFavoritosDirective.js"
                      ));

            bundles.Add(new ScriptBundle("~/js/analisisjuridico").Include(
                      "~/Scripts/misc/AnalisisJuridico/Index/main.js"
                      ));

            #region GraficoBurbuja

            bundles.Add(new ScriptBundle("~/js/analisisjuridicogb").Include(
                      "~/Scripts/misc/AnalisisJuridico/GraficoBurbuja/main.js"
                      ));

            #endregion

            #region Lineas

            bundles.Add(new ScriptBundle("~/js/lineas").Include(
                      "~/Scripts/misc/AnalisisJuridico/Linea/Controllers/lineasController.js",
                      "~/Scripts/misc/Shared/Factories/busquedasFactory.js"
                      ));

            #endregion

            #endregion

            #region AnalisisConsultados

            bundles.Add(new ScriptBundle("~/js/analisisconsultados").Include(
                      "~/Scripts/misc/Shared/Factories/analisisConsultadosFactory.js",
                      "~/Scripts/misc/Home/Index/Directives/analisisConsultadosDirective.js"
                      ));

            #endregion

            #region Sentencias

            bundles.Add(new ScriptBundle("~/js/SentenciaScripts").Include(
                        "~/Scripts/jquery.blast.js",
                        "~/Scripts/misc/Sentencia/LegisResaltado.js",
                        "~/Scripts/misc/Sentencia/resaltados-init.js",
                        "~/Scripts/misc/Sentencia/barra-documento.js",
                        "~/Scripts/misc/Sentencia/SearchInDocument.js"
                        ));

            #endregion

            #region Contactenos

            bundles.Add(new ScriptBundle("~/js/contactenos").Include(
                        "~/Scripts/misc/Home/Contactenos/main.js"
                        ));

            #endregion

            #region Autocompletar
            bundles.Add(new ScriptBundle("~/js/autocompletar").Include(
             "~/Scripts/misc/Autocompletar/Autocompletar.js"
             ));

            #endregion

            #region Resultados de busqueda

            bundles.Add(new ScriptBundle("~/js/resultados").Include(
                      "~/Scripts/misc/Shared/Factories/busquedasFactory.js",
                      "~/Scripts/misc/Busquedas/Controllers/busquedasController.js"
                      ));

            #endregion

            #region Notificaciones

            bundles.Add(new ScriptBundle("~/js/notificacionesGlobal").Include(
                    "~/Scripts/misc/Notificaciones/Controllers/notificacionesMiniController.js",
                    "~/Scripts/misc/Notificaciones/Factories/notificacionesFactory.js"
                    )
                );

            bundles.Add(new ScriptBundle("~/js/notificaciones").Include(
                    "~/Scripts/misc/Notificaciones/Alerts.js",
                    "~/Scripts/misc/Notificaciones/Controllers/notificacionesController.js"
                    )
                );



            #endregion

            #region UsuarioIP
            bundles.Add(new ScriptBundle("~/js/UsuarioIp").Include(
                 "~/Scripts/misc/LoginIp/LoginIp.js"
                ));

            #endregion

            #region Favoritos

            bundles.Add(new ScriptBundle("~/js/favoritos").Include(
                      "~/Scripts/misc/Favoritos/Alerts.js",
                      "~/Scripts/misc/Favoritos/Controllers/FavoritosController.js",
                      "~/Scripts/misc/Shared/Factories/favoritosFactory.js"
                      ));

            #endregion

            #region LineaJurisprudencial

            bundles.Add(new StyleBundle("~/css/linea-jurisprudencial-grafico").Include(
                  "~/Content/lineaJurisprudencialGraph.css"
            ));

            bundles.Add(new ScriptBundle("~/js/linea-jurisprudencial-grafico").Include(
                  "~/Scripts/misc/LineaJurisprudencial/Grafico/main.js"
            ));

            #endregion

            #endregion

            #region Custom Styles

            bundles.Add(new StyleBundle("~/css/styles").Include(
                      "~/Content/main.css",
                      "~/Content/custom-styles.css"
                      ));

            // Estilos para los documentos de jurcol.
            bundles.Add(new StyleBundle("~/css/SentenciaStyles").Include(
                      "~/Content/DocumentMarkerStyles.css",
                      "~/Content/DocumentStyles.css"
                      ));

            // Estilos para el gráfico de burburjas.
            bundles.Add(new StyleBundle("~/css/BubbleGraphStyles").Include(
                      "~/Content/BubbleGraph.css"
                      ));

            #region Admin

            bundles.Add(new StyleBundle("~/css/admin").Include(
                      "~/Content/admin.css"));

            #endregion

            #endregion

            #region Jasny Bootstrap

            bundles.Add(new StyleBundle("~/css/jasny").Include(
                      "~/Content/jasny-bootstrap.css"
                      ));

            bundles.Add(new ScriptBundle("~/js/jasny").Include(
                   "~/Scripts/jasny-bootstrap.js"
                   ));

            #endregion

            #region jQuery

            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                    "~/Scripts/jquery-{version}.js"
                    ));

            #endregion

            #region jQuery Waypoints

            bundles.Add(new ScriptBundle("~/js/jqueryw").Include(
                    "~/Scripts/jquery.waypoints.js",
                    "~/Scripts/sticky.js"
                    ));

            #endregion

            #region jQuery MapHilight

            bundles.Add(new ScriptBundle("~/js/jquery-map-img").Include(
                    "~/Scripts/jquery.maphilight.js"
                    ));

            #endregion

            #region jTip

            bundles.Add(new ScriptBundle("~/js/jtip").Include(
                    "~/Scripts/jTip.js"));

            #endregion

            #region Lodash

            bundles.Add(new ScriptBundle("~/js/lodash").Include(
                    "~/Scripts/lodash.js"));

            #endregion

            #region Logos

            bundles.Add(new ScriptBundle("~/js/logos").Include(
                    "~/Scripts/misc/Statistics/Estadisticas.js",
                    "~/Scripts/misc/Statistics/Eventos.js"
                    ));

            #endregion

            // Para la depuración, establezca EnableOptimizations en false. Para obtener más información,
            // visite http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = false;

            bundles.IgnoreList.Clear();
        }
    }
}
