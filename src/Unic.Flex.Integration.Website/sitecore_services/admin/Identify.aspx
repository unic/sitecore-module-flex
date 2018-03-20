<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" Debug="true" %>
<%@ Import Namespace="Sitecore.Sites" %>
<%@ Import Namespace="Sitecore.Analytics" %>
<%@ Import Namespace="Sitecore.Analytics.Model.Entities" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Identify</title>
    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckSecurity();
            this.LoadData();
        }

        private void LoadData() 
        {
            Tracker.Initialize();
            if(!this.IsPostBack) {
                var contact = Tracker.Current.Contact;
                txtIdentifier.Text = contact.Identifiers.Identifier;

                var personalInfo = contact.GetFacet<IContactPersonalInfo>("Personal");
                txtFirstname.Text = personalInfo.FirstName;
                txtLastname.Text = personalInfo.Surname;
                txtGender.Text = personalInfo.Gender;
                
                var emails = contact.GetFacet<IContactEmailAddresses>("Emails");
                if(emails.Entries.Contains("Default")) {
                    txtEmail.Text = emails.Entries["Default"].SmtpAddress;
                }
            }
        }

        private void CheckSecurity()
        {
            if (Sitecore.Context.User.IsAdministrator) return;

            // show login page if the user does not have enough privileges
            SiteContext site = Sitecore.Context.Site;
            if (site != null)
            {
                base.Response.Redirect(string.Format("{0}?returnUrl={1}", site.LoginPage, HttpUtility.UrlEncode(base.Request.Url.PathAndQuery)));
            }
        }

        private void btnIdentify_Clicked(object sender, EventArgs e)
        {
            Tracker.Current.Session.Identify(txtIdentifier.Text);
        }

        private void btnSave_Clicked(object sender, EventArgs e) 
        {
            var contact = Tracker.Current.Contact;
            var emails = contact.GetFacet<IContactEmailAddresses>("Emails");

            if (!emails.Entries.Contains("Default"))
            {
                emails.Preferred = "Default";
                emails.Entries.Create("Default");
            }

            var personalEmail = emails.Entries["Default"];
            personalEmail.SmtpAddress = txtEmail.Text;

            var personalInfo = contact.GetFacet<IContactPersonalInfo>("Personal");
            personalInfo.FirstName = txtFirstname.Text;
            personalInfo.Surname = txtLastname.Text;
            personalInfo.Gender = txtGender.Text;
        }

    </script>
</head>
<body>
    <form runat=server>
        <h1>Identify</h1>
        <asp:TextBox ID="txtIdentifier" runat="server" Text="" />
        <asp:Button ID="btnIdentify" OnClick="btnIdentify_Clicked" runat="server" Text="Identify" />

        <h1>Contact Data</h1>
        <div>
            Firstname:
        <asp:TextBox ID="txtFirstname" runat="server" Text="" />
        <div>
            Lastname: 
        <asp:TextBox ID="txtLastname" runat="server" Text="" />
        <div>
            Email: 
        <asp:TextBox ID="txtEmail" runat="server" Text="" />
        <div>
            Gender:
        <asp:TextBox ID="txtGender" runat="server" Text="" />
        <div>
        <asp:Button ID="btnSave" OnClick="btnSave_Clicked" runat="server" Text="Save" />

    </form>
</body>
</html>