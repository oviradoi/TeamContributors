using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamContributors.Base;
using TeamContributors.Pages;

namespace TeamContributors
{
    [TeamExplorerNavigationItem(TeamContributorsNavigationItem.LinkId, 200)]
    public class TeamContributorsNavigationItem : TeamExplorerBaseNavigationItem
    {
        public const string LinkId = "2099FE28-F9AB-4FAB-988C-FEE769CE921E";

        [ImportingConstructor]
        public TeamContributorsNavigationItem([Import(typeof(SVsServiceProvider))]IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = "Team Contributors";
            if (this.CurrentContext != null && this.CurrentContext.HasCollection && this.CurrentContext.HasTeamProject)
            {
                this.IsVisible = true;
            }
            //System.Drawing.Image bmp = System.Drawing.Image.FromStream(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Microsoft.ALMRangers.Samples.MyHistory.Resources.MyHistory.png"));
            this.Image = null;
        }

        public override void Execute()
        {
            try
            {
                ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
                if (teamExplorer != null)
                {
                    teamExplorer.NavigateToPage(new Guid(TeamContributorsPage.PageId), null);
                }
            }
            catch (Exception ex)
            {
                this.ShowNotification(ex.Message, NotificationType.Error);
            }
        }

        public override void Invalidate()
        {
            base.Invalidate();
            if (this.CurrentContext != null && this.CurrentContext.HasCollection && this.CurrentContext.HasTeamProject)
            {
                this.IsVisible = true;
            }
            else
            {
                this.IsVisible = false;
            }
        }
    }
}
