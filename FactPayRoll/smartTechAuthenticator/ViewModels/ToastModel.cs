using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ToastModel
    {
        public ToastType Type { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }
        public ToastModel()
        {
            ToastMessages = new List<ToastMessage>();
        }

        public ToastMessage AddToastMessage(string Title, string Message, ToastType Type)
        {
            ToastMessage toastMessage = new ToastMessage()
            {
                Message = Message,
                Title = Title,
                Type = Type
            };
            ToastMessages.Add(toastMessage);
            return toastMessage;
        }
    }
    public class ToastMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ToastType Type { get; set; }
    }

    public enum ToastType
    {
        success, info, error
    }

    public enum UserType
    {
        Admin,
        Customer,
        SubAdmin,
    }
}