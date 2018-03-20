<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" Debug="true" %>
<%@ Import Namespace="Sitecore.Sites" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Flush session</title>
    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckSecurity();
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

        private void btnFlushSession_OnClick(object sender, EventArgs e)
        {
            Session.Abandon();        
            Response.Write("Abandoned");
        }

    </script>
</head>
<body>
    <form runat=server>
        <asp:Button ID="btnFlushSession" runat="server" Text="Flush session" OnClick="btnFlushSession_OnClick" />
    </form>
</body>
</html>