using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FormEditor.Fields
{
	public class MemberAttributeField : FieldWithValue
    {
		public override string Type
		{
			get { return "core.memberattribute"; }
		}

		public override string PrettyName
		{
			get { return "Member Attribute"; }
		}

        public string AttributeName { get; set; }

        protected internal override void CollectSubmittedValue(Dictionary<string, string> allSubmittedValues, IPublishedContent content)
        {
            // get the logged in member (if any)
            var member = CurrentMember();
            if (member == null)
            {
                // no member logged in
                base.CollectSubmittedValue(allSubmittedValues, content);
                return;
            }

            Log.Info("AttributeName =" + AttributeName, null);


            if (!string.IsNullOrEmpty((string)AttributeName)) {
                if (AttributeName == "Username")
                    SubmittedValue = member.Username;
                else if (AttributeName == "Useremail")
                    SubmittedValue = member.Email;
                else if (AttributeName == "Userid")
                    SubmittedValue = member.Id.ToString();
                else if (!string.IsNullOrEmpty((string)member.GetValue(AttributeName)))
                    SubmittedValue = (string)member.GetValue(AttributeName);
                else
                    SubmittedValue = "";
            }

            Log.Info("SubmittedValue =" + SubmittedValue, null);
        }

        private IMember CurrentMember()
		{
			var user = UmbracoContext.Current.HttpContext.User;
			var identity = user != null ? user.Identity : null;
			return identity != null && string.IsNullOrWhiteSpace(identity.Name) == false
				? UmbracoContext.Current.Application.Services.MemberService.GetByUsername(identity.Name)
				: null;			
		}

		public bool IsMemberLoggedIn
		{
			get { return CurrentMember() != null; }
		}

        #region Format value for various display scenarios

        protected internal override string FormatValueForDataView(string value, IContent content, Guid rowId)
        {
            return base.FormatValueForDataView(value, content, rowId);
        }

        protected internal override string FormatValueForCsvExport(string value, IContent content, Guid rowId)
        {
            return base.FormatValueForCsvExport(value, content, rowId);
        }

        protected internal override string FormatValueForFrontend(string value, IPublishedContent content, Guid rowId)
        {
            return base.FormatValueForFrontend(value, content, rowId);
        }
        
        #endregion
    }
}
