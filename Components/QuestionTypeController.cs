using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DotNetNuke.Common.Utilities;
using Gafware.Modules.ContactForm.Data;

namespace Gafware.Modules.ContactForm.Components
{
    public class QuestionTypeController
    {
        public static QuestionType GetQuestionType(int questionTypeId)
        {
            return CBO.FillObject<QuestionType>(DataProvider.Instance().GetQuestionType(questionTypeId));
        }

        public static List<QuestionType> GetQuestionTypes(int portalId)
        {
            return CBO.FillCollection<QuestionType>(DataProvider.Instance().GetQuestionTypes(portalId));
        }

        public static void DeleteQuestionType(int questionTypeId)
        {
            DataProvider.Instance().DeleteQuestionType(questionTypeId);
        }

        public static int SaveQuestionType(QuestionType questionType)
        {
            if (questionType.QuestionTypeID > 0)
            {
                DataProvider.Instance().UpdateQuestionType(questionType);
            }
            else
            {
                questionType.QuestionTypeID = DataProvider.Instance().AddQuestionType(questionType);
            }
            return questionType.QuestionTypeID;
        }
    }
}