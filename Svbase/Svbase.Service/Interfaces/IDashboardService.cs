using System.Collections.Generic;
using Svbase.Core.Models;

namespace Svbase.Service.Interfaces
{
    public interface IDashboardService
    {
        DashboardManagementViewModel GetDashboardModel();
        DashboardViewModel GetDashboardViewModel();

    }
}
