/*
' Copyright (c) 2021 Gafware
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Microsoft.ApplicationBlocks.Data;

namespace Gafware.Modules.ContactForm.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// 
    /// This concreted data provider class provides the implementation of the abstract methods 
    /// from data dataprovider.cs
    /// 
    /// In most cases you will only modify the Public methods region below.
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "Gafware_ContactForm_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        public override System.Data.IDataReader GetContact(string querycode, int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetContact", new SqlParameter("@QueryCode", querycode), new SqlParameter("@PoralID", portalId));
        }

        public override System.Data.IDataReader GetContacts(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetContacts", new SqlParameter("@PortalID", portalId));
        }

        public override System.Data.IDataReader GetContact(int contactId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetContactById", new SqlParameter("@ContactID", contactId));
        }

        public override void DeleteContact(int contactId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteContact", new SqlParameter("@ContactID", contactId));
        }

        public override int AddContact(Components.Contact contact)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "AddContact",
                new SqlParameter("@PortalID", contact.PortalID),
                new SqlParameter("@QueryCode", contact.QueryCode),
                new SqlParameter("@EmailAddress", contact.EmailAddress)
            ));
        }

        public override void UpdateContact(Components.Contact contact)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "UpdateContact",
                new SqlParameter("@ContactID", contact.ContactID),
                new SqlParameter("@PortalID", contact.PortalID),
                new SqlParameter("@QueryCode", contact.QueryCode),
                new SqlParameter("@EmailAddress", contact.EmailAddress)
            );
        }

        public override System.Data.IDataReader GetQuestionTypes(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetQuestionTypes", new SqlParameter("@PortalID", portalId));
        }

        public override System.Data.IDataReader GetQuestionType(int questionTypeId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetQuestionType", new SqlParameter("@QuestionTypeID", questionTypeId));
        }

        public override void DeleteQuestionType(int questionTypeId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteQuestionType", new SqlParameter("@QuestionTypeID", questionTypeId));
        }

        public override int AddQuestionType(Components.QuestionType questionType)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "AddQuestionType",
                new SqlParameter("@PortalID", questionType.PortalID),
                new SqlParameter("@QueryCode", questionType.QueryCode),
                new SqlParameter("@Topic", questionType.Topic)
            ));
        }

        public override void UpdateQuestionType(Components.QuestionType questionType)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "UpdateQuestionType",
                new SqlParameter("@QuestionTypeID", questionType.QuestionTypeID),
                new SqlParameter("@PortalID", questionType.PortalID),
                new SqlParameter("@QueryCode", questionType.QueryCode),
                new SqlParameter("@Topic", questionType.Topic)
            );
        }



        public override System.Data.IDataReader GetEmails(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetEmails", new SqlParameter("@PortalID", portalId));
        }

        public override System.Data.IDataReader GetEmail(int emailId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetEmail", new SqlParameter("@EmailID", emailId));
        }

        public override void DeleteEmail(int emailId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteEmail", new SqlParameter("@EmailID", emailId));
        }

        public override int AddEmail(Components.EmailObject email)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "AddEmail",
                new SqlParameter("@PortalID", email.PortalID),
                new SqlParameter("@FromAddress", email.FromAddress),
                new SqlParameter("@BccAddress", email.BccAddress),
                new SqlParameter("@Message", email.Message),
                new SqlParameter("@ContactNumber", email.ContactNumber),
                new SqlParameter("@Name", email.Name),
                new SqlParameter("@Area", email.Area)
            ));
        }

        public override void UpdateEmail(Components.EmailObject email)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, System.Data.CommandType.StoredProcedure, NamePrefix + "UpdateEmail",
                new SqlParameter("@EmailID", email.EmailID),
                new SqlParameter("@PortalID", email.PortalID),
                new SqlParameter("@FromAddress", email.FromAddress),
                new SqlParameter("@BccAddress", email.BccAddress),
                new SqlParameter("@Message", email.Message),
                new SqlParameter("@ContactNumber", email.ContactNumber),
                new SqlParameter("@Name", email.Name),
                new SqlParameter("@Area", email.Area)
            );
        }
        #endregion

    }

}