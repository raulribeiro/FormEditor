using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace FormEditor.Fields
{
	public class MemberAttributeField : FieldWithPlaceholder
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
            // gather member data for index
            SubmittedValue = (string)member.Properties[AttributeName].Value;
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
	}
}
