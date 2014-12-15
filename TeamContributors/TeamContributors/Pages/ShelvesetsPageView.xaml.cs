using System;
using System.Collections.Generic;
using System.Linq;
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

namespace TeamContributors.Pages
{
    /// <summary>
    /// Interaction logic for ShelvesetsPageView.xaml
    /// </summary>
    public partial class ShelvesetsPageView : UserControl
    {
        public static readonly DependencyProperty ParentSectionProperty = DependencyProperty.Register("ParentSection", typeof(ShelvesetsPage), typeof(ShelvesetsPageView));

        public ShelvesetsPageView()
        {
            InitializeComponent();
        }

        public ShelvesetsPage ParentSection
        {
            get { return (ShelvesetsPage)GetValue(ParentSectionProperty); }

            set { SetValue(ParentSectionProperty, value); }
        }
    }
}
