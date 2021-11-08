using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;

namespace Gafware.Modules.ContactForm.Components
{
    public class EmailObject : ContentItem
    {
        public int EmailID { get; set; }
        public int PortalID { get; set; }
        public string FromAddress { get; set; }
        public string BccAddress { get; set; }
        public string Message { get; set; }
        public string ContactNumber { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public DateTime DateSent { get; set; }
    
        public override void Fill(System.Data.IDataReader dr)
        {
            EmailID = Null.SetNullInteger(dr["EmailID"]);
            PortalID = Null.SetNullInteger(dr["PortalID"]);
            FromAddress = Null.SetNullString(dr["FromAddress"]);
            BccAddress = Null.SetNullString(dr["BccAddress"]);
            Message = Null.SetNullString(dr["Message"]);
            ContactNumber = Null.SetNullString(dr["ContactNumber"]);
            Name = Null.SetNullString(dr["Name"]);
            Area = Null.SetNullString(dr["Area"]);
            DateSent = Null.SetNullDateTime(dr["DateSent"]);
        }

        public override int KeyID
        {
            get
            {
                return EmailID;
            }
            set
            {
                EmailID = value;
            }
        }

    }
}