using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;

namespace Gafware.Modules.ContactForm.Components
{
    public class Contact : ContentItem
    {
        public int ContactID { get; set; }
        public int PortalID { get; set; }
        public string QueryCode { get; set; }
        public string EmailAddress { get; set; }

        public override void Fill(System.Data.IDataReader dr)
        {
            ContactID = Null.SetNullInteger(dr["ContactID"]);
            PortalID = Null.SetNullInteger(dr["PortalID"]);
            QueryCode = Null.SetNullString(dr["QueryCode"]);
            EmailAddress = Null.SetNullString(dr["EmailAddress"]);
        }

        public override int KeyID
        {
            get
            {
                return ContactID;
            }
            set
            {
                ContactID = value;
            }
        }
    }
}