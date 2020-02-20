using System.Threading.Tasks;
using Svbase.Core.Models;

namespace Svbase.Service.Interfaces
{
    public interface IDashboardService
    {
        DashboardManagementViewModel GetDashboardModel();
        Task<DashboardViewModel> GetDashboardViewModel();
    }
}
