using EnvDTE80;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.VersionControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TeamContributors.Base;

namespace TeamContributors.Pages
{
    [TeamExplorerPage(TeamContributorsPage.PageId)]
    public class TeamContributorsPage : TeamExplorerBasePage
    {
        public const string PageId = "45DA439D-7415-4D02-A571-6695AE6F5974";

        private ObservableCollection<TeamFoundationIdentity> users = new ObservableCollection<TeamFoundationIdentity>();

        public ObservableCollection<TeamFoundationIdentity> Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged("Users"); }
        }

        public ICommand ViewChangesets { get; private set; }
        public ICommand ViewShelvesets { get; private set; }

        public TeamContributorsPage()
        {
            this.Title = "Team Contributors";
            this.PageContent = new TeamContributorsView();
            this.View.ParentSection = this;
            ViewChangesets = new RelayCommand(id => ViewChangesetsCommand((TeamFoundationIdentity)id));
            ViewShelvesets = new RelayCommand(id => ViewShelvesetsCommand((TeamFoundationIdentity)id));
        }

        protected TeamContributorsView View
        {
            get { return this.PageContent as TeamContributorsView; }
        }

        private void ViewShelvesetsCommand(TeamFoundationIdentity item)
        {
            ITeamExplorer teamExplorer = (ITeamExplorer)ServiceProvider.GetService(typeof(ITeamExplorer));
            if (teamExplorer != null)
            {
                teamExplorer.NavigateToPage(new Guid(ShelvesetsPage.PageId), null);
            }
        }

        private void ViewChangesetsCommand(TeamFoundationIdentity item)
        {
            ITeamFoundationContext context = this.CurrentContext;
            if (context != null && context.HasCollection && context.HasTeamProject)
            {
                VersionControlServer vcs = context.TeamProjectCollection.GetService<VersionControlServer>();
                if (vcs != null)
                {
                    string path = "$/" + context.TeamProjectName;
                    string user = item.UniqueName;
                    VersionControlExt vc = GetVersionControlExt(this.ServiceProvider);
                    if (vc != null)
                    {
                        vc.History.Show(path, VersionSpec.Latest, 0, RecursionType.Full, user, null, null, int.MaxValue, true);
                    }
                }
            }
        }

        public static VersionControlExt GetVersionControlExt(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                DTE2 dte = serviceProvider.GetService(typeof(SDTE)) as DTE2;
                if (dte != null)
                {
                    return dte.GetObject("Microsoft.VisualStudio.TeamFoundation.VersionControl.VersionControlExt") as VersionControlExt;
                }
            }

            return null;
        }

        public override async void Refresh()
        {
            base.Refresh();
            await RefreshUsersAsync();
        }

        private async Task RefreshUsersAsync()
        {
            try
            {
                this.IsBusy = true;
                this.Users.Clear();

                ObservableCollection<TeamFoundationIdentity> users = new ObservableCollection<TeamFoundationIdentity>();

                await Task.Run(() =>
                    {
                        ITeamFoundationContext context = this.CurrentContext;
                        IIdentityManagementService ims = context.TeamProjectCollection.GetService<IIdentityManagementService>();

                        TeamFoundationIdentity[][] sids = ims.ReadIdentities(IdentitySearchFactor.AccountName, new string[] { "Contributors" }, MembershipQuery.Expanded, ReadIdentityOptions.None);
                        if (sids.Length > 0 && sids[0].Length > 0)
                        {
                            var ids = ims.ReadIdentities(sids[0][0].Members, MembershipQuery.Expanded, ReadIdentityOptions.None);
                            foreach (TeamFoundationIdentity id in ids)
                            {
                                users.Add(id);
                            }
                        }
                    });
                this.Users = users;
            }
            catch (Exception ex)
            {
                this.ShowNotification(ex.Message, NotificationType.Error);
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        public override async void Initialize(object sender, PageInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            // Kick off the refresh
            await this.RefreshUsersAsync();
        }
    }
}
