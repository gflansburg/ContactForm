using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DotNetNuke.Common.Utilities;
using Gafware.Modules.ContactForm.Data;

namespace Gafware.Modules.ContactForm.Components
{
    public class EmailController
    {
        public static EmailObject GetEmail(int emailId)
        {
            return CBO.FillObject<EmailObject>(DataProvider.Instance().GetEmail(emailId));
        }

        public static List<EmailObject> GetEmails(int portalId)
        {
            return CBO.FillCollection<EmailObject>(DataProvider.Instance().GetEmails(portalId));
        }

        public static void DeleteEmail(int emailId)
        {
            DataProvider.Instance().DeleteEmail(emailId);
        }

        public static int SaveEmail(EmailObject email)
        {
            if (email.EmailID > 0)
            {
                DataProvider.Instance().UpdateEmail(email);
            }
            else
            {
                email.EmailID = DataProvider.Instance().AddEmail(email);
            }
            return email.EmailID;
        }
    }
}