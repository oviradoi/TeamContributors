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
    /// Interaction logic for TeamContributorsView.xaml
    /// </summary>
    public partial class TeamContributorsView : UserControl
    {
        public static readonly DependencyProperty ParentSectionProperty = DependencyProperty.Register("ParentSection", typeof(TeamContributorsPage), typeof(TeamContributorsView));

        public TeamContributorsView()
        {
            InitializeComponent();
        }

        public TeamContributorsPage ParentSection
        {
            get { return (TeamContributorsPage)GetValue(ParentSectionProperty); }

            set { SetValue(ParentSectionProperty, value); }
        }
    }
}
