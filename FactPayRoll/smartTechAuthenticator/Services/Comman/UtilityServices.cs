using smartTechAuthenticator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace smartTechAuthenticator.Services.Comman
{
    public static class UtilityServices
    {
        public static ToastMessage ShowMessage(this Controller controller,string Title,string Message,ToastType toastType)
        {  
            ToastModel toastr = controller.TempData["toastmessage"] as ToastModel;
            toastr = toastr ?? new ToastModel(); 
            var toastMessage = toastr.AddToastMessage(Title, Message, toastType);
            controller.TempData["toastmessage"] = toastr;
            return toastMessage;
        }
        public static  void SetUserId(this Controller controller, Guid UserId,string name ,string userName,string Role)
        {
            controller.Session["UserId"] = UserId;
            controller.Session["Name"] = name;
            controller.Session["userName"] = userName;         
            controller.Session["Role"] = Role;         
        }
        public static Guid GetUserId(this Controller controller)
        {
            return (Guid)controller.Session["UserId"];
        }
    }
}