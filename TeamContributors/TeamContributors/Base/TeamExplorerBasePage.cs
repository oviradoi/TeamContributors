using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamContributors.Base
{
    /// <summary>
    /// Team Explorer page base class.
    /// </summary>
    public class TeamExplorerBasePage : TeamExplorerBase, ITeamExplorerPage
    {
        private string title;
        private bool isBusy;
        private object pageContent;

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        public object PageContent
        {
            get
            {
                return this.pageContent;
            }

            set
            {
                this.pageContent = value;
                this.RaisePropertyChanged("PageContent");
            }
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }

            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged("IsBusy");
            }
        }

        public virtual void Initialize(object sender, PageInitializeEventArgs e)
        {
            this.ServiceProvider = e.ServiceProvider;
        }

        public virtual void Loaded(object sender, PageLoadedEventArgs e)
        {
        }

        public virtual void SaveContext(object sender, PageSaveContextEventArgs e)
        {
        }

        public virtual void Refresh()
        {
        }

        public virtual void Cancel()
        {
        }

        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }
    }
}
