using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContributors.Base;

namespace TeamContributors.Pages
{
    [TeamExplorerPage(ShelvesetsPage.PageId)]
    public class ShelvesetsPage : TeamExplorerBasePage
    {
        public const string PageId = "0A2645B0-A26E-4566-A40E-B67B189DB6AE";

        public ShelvesetsPage()
        {
            this.Title = "View Shelvesets";
            this.PageContent = new ShelvesetsPageView();
            this.View.ParentSection = this;
        }

        protected ShelvesetsPageView View
        {
            get { return this.PageContent as ShelvesetsPageView; }
        }
    }
}
