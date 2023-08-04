using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smartTechAuthenticator.ViewModels
{
    public class ResponseModel
    {
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public string QrStatus { get; set; }
        public Status Status { get; set; }
        public Guid OrderId { get; set; }
        public ResponseModel()
        {
            Status = new Status();
        }
    }
    public enum Status
    { 
        Failure,
        Success
    }


    public static class RolesConvention
    {
        public const string Administrator = "Administrator";
        public const string Guest = "Guest";
    }
    public class Response
    {
        public int Status { get; set; }
        public string Msg { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Roles { get; set; }
        public string OTP { get; set; }
        public DateTime OTPExpTime { get; set; }
    }
}