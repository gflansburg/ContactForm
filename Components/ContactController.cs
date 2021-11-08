using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DotNetNuke.Common.Utilities;
using Gafware.Modules.ContactForm.Data;

namespace Gafware.Modules.ContactForm.Components
{
    public class ContactController
    {
        public static string GetContact(string queryCode, int portalId)
        {
            IDataReader reader = DataProvider.Instance().GetContact(queryCode, portalId);
            if (reader != null)
            {
                if (reader.Read())
                {
                    return reader[0] as string;
                }
            }
            return String.Empty;
        }

        public static Contact GetContact(int contactId)
        {
            return CBO.FillObject<Contact>(DataProvider.Instance().GetContact(contactId));
        }

        public static List<Contact> GetContacts(int portalId)
        {
            return CBO.FillCollection<Contact>(DataProvider.Instance().GetContacts(portalId));
        }

        public static void DeleteContact(int contactId)
        {
            DataProvider.Instance().DeleteContact(contactId);
        }

        public static int SaveContact(Contact contact)
        {
            if (contact.ContactID > 0)
            {
                DataProvider.Instance().UpdateContact(contact);
            }
            else
            {
                contact.ContactID = DataProvider.Instance().AddContact(contact);
            }
            return contact.ContactID;
        }
    }
}