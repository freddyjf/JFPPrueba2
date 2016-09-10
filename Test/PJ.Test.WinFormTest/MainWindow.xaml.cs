using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NUnit.Framework;


namespace PJ.Test.WinFormTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var assembly = Assembly.LoadFile(txtfile.Text);
            //get testfixture classes in assembly.
            var testTypes = from t in assembly.GetTypes()
                            let attributes = t.GetCustomAttributes(typeof(LEGIS.PJ.Tests.Services.), true)
                            where attributes != null && attributes.Length > 0
                            orderby t.Name
                            select t;
            foreach (var type in testTypes)
            {
                //get test method in class.
                var testMethods = from m in type.GetMethods()
                                  let attributes = m.GetCustomAttributes(typeof(NUnit.Framework.TestAttribute), true)
                                  where attributes != null && attributes.Length > 0
                                  orderby m.Name
                                  select m;
                foreach (var method in testMethods)
                {
                    txtresult.Text +=method.Name;
                }
            }

        }
    }
}
