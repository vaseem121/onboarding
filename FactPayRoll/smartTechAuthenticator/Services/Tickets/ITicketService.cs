using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace smartTechAuthenticator.Services.Tickets
{
    public interface ITicketService
    {
        Task<ResponseModel> CreateTicket(TicketViewModel tick);
        Task<ResponseModel> TicketMessage(TicketMessageSystemViewModels TicketMessage);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetTickets(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string UserId);
        Task<TicketViewModel> GetTicketlDetail(string Id);
        Task<ResponseModel> UpdateTicketsData(TicketViewModel prod);
        Task<ResponseModel> UpdateRating(string Id, string RatingId);
        Task<ResponseModel> DeleteTicketDetails(TicketViewModel model1);
        Task<TicketViewModel> TicketDetails(Guid? id);
        Task<TicketViewModel> GetAllTickets(string UserId,int StatusId);
        List<TicketMessageSystemViewModels> ViewUserMessageList(Guid UserId,Guid? TicketId);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string startDate, string endDate, int status);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetUnSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue);
        (int TotalCount, int FilteredCount, dynamic Categorys) GetUnSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string startDate, string endDate, int status);
        TicketViewModel GetTickNotification(string UserId);
        Task<ResponseModel> notificaationview(string Id);
    }
}