using Microsoft.EntityFrameworkCore;
using smartTechAuthenticator.Models;
using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace smartTechAuthenticator.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationDbContext context;
        public TicketService(ApplicationDbContext _context)
        {
            context = _context;
        }
        public async Task<ResponseModel> CreateTicket(TicketViewModel tick)
        {
            ResponseModel model = new ResponseModel();
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Ticket data = new Ticket();
                        data.Id = Guid.NewGuid();
                        data.Title= tick.Title; 
                        data.Message = tick.Message;    
                        data.CustomerId = tick.CustomerId;
                        data.CreatedDate = DateTime.Now;
                        data.CreatedBy =tick.CustomerId;
                        data.Photo = tick.Photo;
                        data.Status = "Unsolved";
                        data.NotificationAdmin = true;
                        data.NotificationCust = false;
                        context.Ticket.Add(data);
                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        model.Status = Status.Success;
                    }
                    catch (Exception ex)
                    {
                        model.Status = Status.Failure;
                        await transaction.RollbackAsync();
                    }

                }
            return model;
        }


        public (int TotalCount, int FilteredCount, dynamic Categorys) GetTickets(int skip, int take, string sortColumn, string sortColumnDir, string searchValue,string UserId)
        {
            var BannerCarousels = context.Ticket.Where(x=>x.CustomerId== UserId).Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Photos = x.Photo,
                Description = x.Message,
                Answer=x.Answer,
                Status=x.Status
            });
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        BannerCarousels = sortColumnDir == "asc" ? BannerCarousels.OrderBy(x => x.Title) : BannerCarousels.OrderByDescending(x => x.Title);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                BannerCarousels = BannerCarousels.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = BannerCarousels.Count();
            var data = BannerCarousels.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Photos = x.Photos,
                Description = x.Description,
                Answer = x.Answer ,
                Status = x.Status
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public async Task<TicketViewModel> GetTicketlDetail(string Id)
        {
            var TicketInfo = (from a in context.Ticket
                              where a.Id == new Guid(Id)
                              select new TicketViewModel
                              {
                                  Id = a.Id.ToString(),
                                  Message = a.Message,
                                  Photo = a.Photo,
                                  Title = a.Title,
                                  Status=a.Status
                              }).FirstOrDefault();
            return TicketInfo;
        }

        public async Task<ResponseModel> UpdateTicketsData(TicketViewModel prod)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.Ticket.FirstOrDefault(x => x.Id == new Guid(prod.Id));
                    if (Info != null)
                    {

                        Info.Title = prod.Title;
                        Info.Message = prod.Message;
                        if (prod.Photo != null)
                        {
                            Info.Photo = prod.Photo;
                        }
                        Info.UpdatedBy = prod.UpdatedBy;
                        Info.UpdatedDate = DateTime.Now;
                        Info.NotificationAdmin = true;
                        Info.NotificationCust = false;
                        context.Ticket.Update(Info);

                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                   // logger.Error(ex);
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return model;
        }
        public async Task<ResponseModel> DeleteTicketDetails(TicketViewModel model1)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var Info = context.Ticket.FirstOrDefault(x => x.Id == new Guid(model1.Id));
                if (Info != null)
                {
                    var data = context.Ticket.Find(Info.Id);
                    if (data != null)
                    {
                        context.Ticket.Remove(data);
                    }
                    await context.SaveChangesAsync();
                    model.Status = Status.Success;
                }
            }
            catch (Exception ex)
            {
                //logger.Error(ex);
                model.Status = Status.Failure;
            }
            return model;
        }
        public async Task<TicketViewModel> TicketDetails(Guid? id)
        {
            TicketViewModel model = new TicketViewModel();
            try
            {
                var a = await context.Ticket.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (a!=null)
                {
                    model.Title = a.Title;
                    model.Message = a.Message;
                    model.Id = a.Id.ToString();
                    model.Photo = a.Photo;
                    model.CustomerId = a.CustomerId;
                    model.Id = id.ToString();
                    model.Answer = a.Answer;
                    model.Status = a.Status;
                }
           
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }

        public async Task<TicketViewModel> GetAllTickets(string UserId,int StatusId)
        {
            TicketViewModel model = new TicketViewModel();
            try
            {
                if (UserId!=null)
                {
                    model.TicketViewmodelList = (from tic in context.Ticket.Where(x => x.CustomerId == UserId)
                                                 select new TicketViewModel
                                                 {
                                                     Id = tic.Id.ToString(),
                                                     Title = tic.Title,
                                                     Photo = tic.Photo,
                                                     Message = tic.Message,
                                                     Answer = tic.Answer,
                                                     Status = tic.Status,
                                                     CreatedDate=tic.CreatedDate,
                                                     Rating = tic.Rating
                                                 }).OrderByDescending(x=>x.CreatedDate).ToList();
                    if(StatusId > 0)
                    {
                        model.TicketViewmodelList = (from tic in context.Ticket.Where(x => x.CustomerId == UserId && x.LebelStatus == StatusId)
                                                     select new TicketViewModel
                                                     {
                                                         Id = tic.Id.ToString(),
                                                         Title = tic.Title,
                                                         Photo = tic.Photo,
                                                         Message = tic.Message,
                                                         Answer = tic.Answer,
                                                         Status = tic.Status,
                                                         LabelStatus=tic.LebelStatus,
                                                         CreatedDate = tic.CreatedDate,
                                                         Rating = tic.Rating
                                                     }).OrderByDescending(x => x.CreatedDate).ToList();
                    }
                    else
                    {
                        model.TicketViewmodelList = (from tic in context.Ticket.Where(x => x.CustomerId == UserId)
                                                     select new TicketViewModel
                                                     {
                                                         Id = tic.Id.ToString(),
                                                         Title = tic.Title,
                                                         Photo = tic.Photo,
                                                         Message = tic.Message,
                                                         Answer = tic.Answer,
                                                         Status = tic.Status,
                                                         LabelStatus = tic.LebelStatus,
                                                         CreatedDate = tic.CreatedDate,
                                                         Rating = tic.Rating
                                                     }).OrderByDescending(x => x.CreatedDate).ToList();
                    }

            }

            }
            catch (Exception ex)
            {

                throw;
            }
            return model;
        }
        public (int TotalCount, int FilteredCount, dynamic Categorys) GetSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var solveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "solved" && x.CreatedBy!=null)
                               join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                               select new TicketViewModel
                               {
                                  Id = Convert.ToString(a.Id),
                                   Message = a.Message,
                                   Title = a.Title,
                                   CreatedDate = a.CreatedDate,
                                   Status = a.Status,
                                   Answer = a.Answer,
                                   Photo = a.Photo,
                                   LabelStatus=a.LebelStatus,
                                   CreatedBy=product.Name
                               }) ;
//solveticket = solveticket.OrderByDescending(x => x.CreatedDate);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Title) : solveticket.OrderByDescending(x => x.Title);
                        break;
                    case "Message":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Message) : solveticket.OrderByDescending(x => x.Message);
                        break;
                    case "CreatedDate":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.CreatedDate) : solveticket.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "Status":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Status) : solveticket.OrderByDescending(x => x.Status);
                        break;
                    case "LabelStatus":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.LabelStatus) : solveticket.OrderByDescending(x => x.LabelStatus);
                        break;
                    case "CreatedBy":
                        solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.CreatedBy) : solveticket.OrderByDescending(x => x.CreatedBy);
                        break;
                }
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                solveticket = solveticket.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = solveticket.Count();
     //       solveticket = solveticket.OrderByDescending(x=>x.CreatedDate);
            var data = solveticket.Select(x => new
            {
                Id=x.Id,
                Title = x.Title,
                Status = x.Status,
                Message = x.Message,
                Answer = x.Answer,
                Photo = x.Photo,
                LabelStatus=x.LabelStatus,
                CreatedBy = x.CreatedBy,
                CreatedDate=x.CreatedDate.ToString("dd-MM-yyyy")
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) GetSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string startDate, string endDate, int status)
        {
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                DateTime startDate1 = Convert.ToDateTime(startDate).Date;
                var aa = endDate.Trim();
                DateTime endDate1 = Convert.ToDateTime(aa).Date;
                var solveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "solved" && x.CreatedBy != null && x.CreatedDate.Date >= startDate1 && x.CreatedDate.Date <= endDate1)
                                   join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                                   select new TicketViewModel
                                   {
                                       Id = Convert.ToString(a.Id),
                                       Message = a.Message,
                                       Title = a.Title,
                                       CreatedDate = a.CreatedDate,
                                       Status = a.Status,
                                       Answer = a.Answer,
                                       Photo = a.Photo,
                                       LabelStatus = a.LebelStatus,
                                       CreatedBy = product.Name
                                   });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Title) : solveticket.OrderByDescending(x => x.Title);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    solveticket = solveticket.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = solveticket.Count();
 //               solveticket = solveticket.OrderByDescending(x => x.CreatedDate);
                var data = solveticket.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = x.Status,
                    Message = x.Message,
                    Answer = x.Answer,
                    Photo = x.Photo,
                    LabelStatus = x.LabelStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
            else
            {
                var solveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "solved" && x.LebelStatus==status)
                                   join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                                   select new TicketViewModel
                                   {
                                       Id = Convert.ToString(a.Id),
                                       Message = a.Message,
                                       Title = a.Title,
                                       CreatedDate = a.CreatedDate,
                                       Status = a.Status,
                                       Answer = a.Answer,
                                       Photo = a.Photo,
                                       LabelStatus = a.LebelStatus,
                                       CreatedBy = product.Name
                                   });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Title) : solveticket.OrderByDescending(x => x.Title);
                            break;
                        case "Message":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Message) : solveticket.OrderByDescending(x => x.Message);
                            break;
                        case "CreatedDate":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.CreatedDate) : solveticket.OrderByDescending(x => x.CreatedDate);
                            break;
                        case "Status":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.Status) : solveticket.OrderByDescending(x => x.Status);
                            break;
                        case "LabelStatus":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.LabelStatus) : solveticket.OrderByDescending(x => x.LabelStatus);
                            break;
                        case "CreatedBy":
                            solveticket = sortColumnDir == "asc" ? solveticket.OrderBy(x => x.CreatedBy) : solveticket.OrderByDescending(x => x.CreatedBy);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    solveticket = solveticket.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = solveticket.Count();
             //  solveticket = solveticket.OrderByDescending(x => x.CreatedDate);
                var data = solveticket.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = x.Status,
                    Message = x.Message,
                    Answer = x.Answer,
                    Photo = x.Photo,
                    LabelStatus = x.LabelStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
           
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) GetUnSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            var unsolveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "unsolved" && x.CreatedBy != null)
                                 join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                                 select new TicketViewModel
                               {
                                   Id = Convert.ToString(a.Id),
                                   Message = a.Message,
                                   Title = a.Title,
                                   CreatedDate = a.CreatedDate,
                                   Status = a.Status,
                                   Answer = a.Answer,
                                   Photo = a.Photo,
                                   LabelStatus=a.LebelStatus,
                                   CreatedBy=product.Name
                               });
         //   unsolveticket = unsolveticket.OrderByDescending(x => x.CreatedDate);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                switch (sortColumn)
                {
                    case "Title":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Title) : unsolveticket.OrderByDescending(x => x.Title);
                        break;
                    case "Message":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Message) : unsolveticket.OrderByDescending(x => x.Message);
                        break;
                    case "CreatedDate":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedDate) : unsolveticket.OrderByDescending(x => x.CreatedDate);
                        break;
                    case "Status":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Status) : unsolveticket.OrderByDescending(x => x.Status);
                        break;
                    case "LabelStatus":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.LabelStatus) : unsolveticket.OrderByDescending(x => x.LabelStatus);
                        break;
                    case "CreatedBy":
                        unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedBy) : unsolveticket.OrderByDescending(x => x.CreatedBy);
                        break;

                }

            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                unsolveticket = unsolveticket.Where(m => m.Title.Contains(searchValue));
            }
            int recordsTotal = unsolveticket.Count();
        //    unsolveticket = unsolveticket.OrderByDescending(x => x.CreatedDate);
            var data = unsolveticket.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                Status = x.Status,
                Message = x.Message,
                Answer = x.Answer,
                Photo = x.Photo,
                LabelStatus=x.LabelStatus,
                CreatedBy = x.CreatedBy,
                CreatedDate=x.CreatedDate.ToString("dd-MM-yyyy")
            }).Skip(skip).Take(take).ToList();
            return (recordsTotal, recordsTotal, data);
        }

        public (int TotalCount, int FilteredCount, dynamic Categorys) GetUnSolvedTicket(int skip, int take, string sortColumn, string sortColumnDir, string searchValue, string startDate, string endDate, int status)
        {
            if (startDate != null && startDate != "" && endDate != null && endDate != "")
            {
                DateTime startDate1 = Convert.ToDateTime(startDate).Date;
                var aa = endDate.Trim();
                DateTime endDate1 = Convert.ToDateTime(aa).Date;
                var  unsolveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "unsolved" &&  x.CreatedBy != null && x.CreatedDate.Date >= startDate1 && x.CreatedDate.Date <= endDate1)
                                     join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                                     select new TicketViewModel
                                     {
                                         Id = Convert.ToString(a.Id),
                                         Message = a.Message,
                                         Title = a.Title,
                                         CreatedDate = a.CreatedDate,
                                         Status = a.Status,
                                         Answer = a.Answer,
                                         Photo = a.Photo,
                                         LabelStatus = a.LebelStatus,
                                         CreatedBy = product.Name
                                     });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Title) : unsolveticket.OrderByDescending(x => x.Title);
                            break;
                        case "Message":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Message) : unsolveticket.OrderByDescending(x => x.Message);
                            break;
                        case "CreatedDate":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedDate) : unsolveticket.OrderByDescending(x => x.CreatedDate);
                            break;
                        case "Status":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Status) : unsolveticket.OrderByDescending(x => x.Status);
                            break;
                        case "LabelStatus":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.LabelStatus) : unsolveticket.OrderByDescending(x => x.LabelStatus);
                            break;
                        case "CreatedBy":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedBy) : unsolveticket.OrderByDescending(x => x.CreatedBy);
                            break;
                    }

                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    unsolveticket = unsolveticket.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = unsolveticket.Count();
          //      unsolveticket = unsolveticket.OrderByDescending(x => x.CreatedDate);
                var data = unsolveticket.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = x.Status,
                    Message = x.Message,
                    Answer = x.Answer,
                    Photo = x.Photo,
                    LabelStatus = x.LabelStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
            else
            {
               var unsolveticket = (from a in context.Ticket.Where(x => x.Status.ToLower() == "unsolved" && x.LebelStatus==status)
                                    join product in context.CustomerInfo on a.CreatedBy equals product.Id.ToString()
                                    select new TicketViewModel
                                     {
                                         Id = Convert.ToString(a.Id),
                                         Message = a.Message,
                                         Title = a.Title,
                                         CreatedDate = a.CreatedDate,
                                         Status = a.Status,
                                         Answer = a.Answer,
                                         Photo = a.Photo,
                                         LabelStatus = a.LebelStatus,
                                        CreatedBy = product.Name
                                    });
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    switch (sortColumn)
                    {
                        case "Title":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Title) : unsolveticket.OrderByDescending(x => x.Title);
                            break;
                        case "Message":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Message) : unsolveticket.OrderByDescending(x => x.Message);
                            break;
                        case "CreatedDate":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedDate) : unsolveticket.OrderByDescending(x => x.CreatedDate);
                            break;
                        case "Status":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.Status) : unsolveticket.OrderByDescending(x => x.Status);
                            break;
                        case "LabelStatus":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.LabelStatus) : unsolveticket.OrderByDescending(x => x.LabelStatus);
                            break;
                        case "CreatedBy":
                            unsolveticket = sortColumnDir == "asc" ? unsolveticket.OrderBy(x => x.CreatedBy) : unsolveticket.OrderByDescending(x => x.CreatedBy);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    unsolveticket = unsolveticket.Where(m => m.Title.Contains(searchValue));
                }
                int recordsTotal = unsolveticket.Count();
           //     unsolveticket = unsolveticket.OrderByDescending(x => x.CreatedDate);
                var data = unsolveticket.Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    Status = x.Status,
                    Message = x.Message,
                    Answer = x.Answer,
                    Photo = x.Photo,
                    LabelStatus = x.LabelStatus,
                    CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate.ToString("dd-MM-yyyy")
                }).Skip(skip).Take(take).ToList();
                return (recordsTotal, recordsTotal, data);
            }
        }

        public TicketViewModel GetTickNotification(string UserId)
        {
            TicketViewModel model = new TicketViewModel();
            TicketViewModel model1 = new TicketViewModel();
            TicketViewModel model2 = new TicketViewModel();
            try
            {
                var Userid = new Guid(UserId);
                if (UserId != null)
                {
                    model1.TicketViewmodelList = (from tic in context.TicketMessageSystem.Where(x=>x.NotificationCust==true && x.CustomerId==Userid)
                                                 select new TicketViewModel
                                                 {
                                                     Id = tic.Id.ToString(),
                                                     Title =context.Ticket.Where(x=>x.Id==tic.TicketId).FirstOrDefault().Title,
                                                     CreatedDate = tic.CreatedDate,
                                                     CustomerId=tic.CustomerId.ToString()
                                                 }).ToList();

                    model2.TicketViewmodelList = (from bb in context.Ticket.Where(x =>x.NotificationCust == true && x.CustomerId==UserId)
                               select new TicketViewModel
                               {
                                   Id=bb.Id.ToString(),
                                   Title=bb.Title,
                                   CreatedDate=bb.CreatedDate,
                                   CustomerId=bb.CustomerId
                               }).ToList();
                    model.TicketViewmodelList = model1.TicketViewmodelList.Concat(model2.TicketViewmodelList).OrderByDescending(x=>x.CreatedDate).ToList();
                    model.Notification = model.TicketViewmodelList.Count();
                }

            }
            catch (Exception ex)
            {

              //  throw;
            }
            return model;
        }

        public async Task<ResponseModel> notificaationview(string Id)
        {
            ResponseModel res = new ResponseModel();
            try
            {
                var data = context.Ticket.Where(x => x.Id == new Guid(Id)).FirstOrDefault();
                var data1 = context.TicketMessageSystem.Where(x => x.Id == new Guid(Id)).FirstOrDefault();
                if (data!=null)
                {
                    data.NotificationCust = false;
                    context.Ticket.Update(data);
                   await context.SaveChangesAsync();
                    res.OrderId = data.Id;
                    res.Status = Status.Success;
                }
                else if (data1!=null)
                {
                    data1.NotificationCust = false;
                    context.TicketMessageSystem.Update(data1);
                    await context.SaveChangesAsync();
                    res.OrderId = data1.TicketId;
                    res.Status = Status.Success;
                }
                else
                {
                    res.Status = Status.Failure;
                }
            }
            catch (Exception)
            {
                res.Status = Status.Failure;
            }
            return res;
        }

        public async Task<ResponseModel> TicketMessage(TicketMessageSystemViewModels TicketMessage)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    TicketMessageSystem data = new TicketMessageSystem();
                    data.Id = Guid.NewGuid();
                    data.Description = TicketMessage.Description;
                    data.CustomerId = TicketMessage.CustomerId;
                    data.TicketId = TicketMessage.TicketId;
                    data.CreatedDate = DateTime.Now;
                    data.NotificationAd = true;
                    data.NotificationCust = false;
                    context.TicketMessageSystem.Add(data);
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }

            }
            return model;
        }

        public List<TicketMessageSystemViewModels> ViewUserMessageList(Guid UserId, Guid? TicketId)
        {
            List<TicketMessageSystemViewModels> model = new List<TicketMessageSystemViewModels>();
            try
            {
                if (UserId != null)
                {
                    var userdata = Guid.Empty;
                    var username = "";
                    var data = context.TicketMessageSystem.Where(x => x.TicketId == TicketId && x.UserId!=Guid.Empty).ToList();
                    if (data!=null)
                    {
                        userdata = data[0].UserId;
                    }
                    var Name1 = context.CustomerInfo.Where(x => x.Id == userdata).FirstOrDefault();
                    if (Name1!=null)
                    {
                        username = Name1.Name;
                    }
                    var Name = context.CustomerInfo.Where(x => x.Id == UserId).FirstOrDefault();
                    
                    var UserName = Name.Name;
                    
                    model = (from a in context.TicketMessageSystem.Where(x => x.TicketId== TicketId)
                                         select new TicketMessageSystemViewModels
                                         {
                                             Id = a.Id,
                                             Description = a.Description,
                                             CreatedDate = a.CreatedDate,
                                             CustomerId=a.CustomerId,
                                             UserId=a.UserId,
                                             CustomerName = UserName,
                                             UserName= username
                                         }).OrderBy(z=>z.CreatedDate).ToList();
                }
                return model;
            }
            catch (Exception ex)
            {

            }
            return model;
        }
        public async Task<ResponseModel> UpdateRating(string TicketId, string Rating)
        {
            ResponseModel model = new ResponseModel();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var Info = context.Ticket.FirstOrDefault(x => x.Id == new Guid(TicketId));
                    if (Info != null)
                    {

                        Info.Rating =Convert.ToInt32(Rating);                
                        Info.UpdatedDate = DateTime.Now;                      
                        context.Ticket.Update(Info);
                    }
                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    model.Status = Status.Success;
                }
                catch (Exception ex)
                {
                    // logger.Error(ex);
                    model.Status = Status.Failure;
                    await transaction.RollbackAsync();
                }
            }
            return model;
        }

    }
}