using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;

namespace Gafware.Modules.ContactForm.Components
{
    public class QuestionType : ContentItem
    {
        public int QuestionTypeID { get; set; }
        public int PortalID { get; set; }
        public string QueryCode { get; set; }
        public string Topic { get; set; }

        public override void Fill(System.Data.IDataReader dr)
        {
            QuestionTypeID = Null.SetNullInteger(dr["QuestionTypeID"]);
            PortalID = Null.SetNullInteger(dr["PortalID"]);
            QueryCode = Null.SetNullString(dr["QueryCode"]);
            Topic = Null.SetNullString(dr["Topic"]);
        }

        public override int KeyID
        {
            get
            {
                return QuestionTypeID;
            }
            set
            {
                QuestionTypeID = value;
            }
        }
    }
}