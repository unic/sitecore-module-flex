<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" Debug="true" %>
<%@ Import Namespace="Sitecore.Sites" %>
<%@ Import Namespace="Sitecore.Analytics" %>
<%@ Import Namespace="Sitecore.Analytics.Tracking" %>
<%@ Import Namespace="Newtonsoft.Json" %>
<%@ Import Namespace="Newtonsoft.Json.Linq" %>
<%@ Import Namespace="Sitecore.Analytics.Model.Framework" %>
<%@ Import Namespace="Sitecore.Analytics.Automation.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Contact</title>
    <script runat="server">
        public class AutomationStateManagerJsonConverter : JsonConverter
        {
            public override bool CanRead
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var stateManager = (AutomationStateManager)value;

                var token = new JObject();
                foreach(var plan in stateManager.GetAutomationStates())
                {
                    var planToken = new JObject();
                    planToken.Add("StateId", plan.StateId);
                    planToken.Add("Created", plan.Created.ToString());
                    planToken.Add("WakeUpDateTime", plan.WakeUpDateTime.ToString());
                    planToken.Add("LastAccessedDateTime", plan.LastAccessedDateTime.ToString());
                    planToken.Add("EntryDateTime", plan.EntryDateTime.ToString());
                    token.Add(plan.PlanId.ToString(), planToken);
                }

                token.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(AutomationStateManager).IsAssignableFrom(objectType);
            }
        }

        public class VisitProfileJsonConverter : JsonConverter
        {
            public override bool CanRead
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var profiles = (IVisitProfiles)value;

                var token = new JObject();
                foreach(var profileName in profiles.GetProfileNames())
                {
                    var profile = profiles[profileName];
                    var profileToken = new JObject
                    {
                        {"PatternId", JToken.FromObject(profile.PatternId)},
                        {"PatternLabel", JToken.FromObject(profile.PatternLabel)},
                        {"Total", JToken.FromObject(profile.Total)},
                        {"Count", JToken.FromObject(profile.Count)},
                        {"Values", JToken.FromObject(profile.ToDictionary(_ => _.Key, _ => _.Value))}
                    };

                    token.Add(new JProperty(profileName, profileToken));
                }

                token.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(IVisitProfiles).IsAssignableFrom(objectType);
            }
        }

        public class ElementDictionaryJsonConverter : JsonConverter
        {
            public override bool CanRead
            {
                get { return false; }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var dictionary = (IElementDictionary<IElement>)value;

                var token = new JObject();
                foreach(var key in dictionary.Keys)
                {
                    token.Add(new JProperty(key, JToken.FromObject(dictionary[key], serializer)));
                }

                token.WriteTo(writer);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(IElementDictionary<IElement>).IsAssignableFrom(objectType);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CheckSecurity();
            this.LoadData();
        }

        private void LoadData()
        {
            Tracker.Initialize();
            if(!this.IsPostBack)
            {
                var settings = new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Error = (serializer,err) => {
                        err.ErrorContext.Handled = true;
                    },
                    Converters = new JsonConverter[] { new ElementDictionaryJsonConverter(), new VisitProfileJsonConverter(), new AutomationStateManagerJsonConverter() }
                };

                divCurrentInteraction.Attributes.Add("data-json", JsonConvert.SerializeObject(Tracker.Current.Interaction, Formatting.Indented, settings));

                divContact.Attributes.Add("data-json", JsonConvert.SerializeObject(Tracker.Current.Contact, Formatting.Indented, settings));

                divKeyBehaviourCache.Attributes.Add("data-json", JsonConvert.SerializeObject(Tracker.Current.Contact.GetKeyBehaviorCache(), Formatting.Indented, settings));
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

    </script>

    <link href="/sitecore_services/admin/jquery.json-viewer.css" type="text/css" rel="stylesheet" />
</head>
<body>
<form runat=server>
    <h1>Current Interaction</h1>
    <pre runat="server" ID="divCurrentInteraction" class="jsoner"></pre>
        
    <h1>Contact</h1>
    <pre runat="server" ID="divContact" class="jsoner"></pre>
    <h1>Key Bahaviour Cache</h1>
    <pre runat="server" ID="divKeyBehaviourCache" class="jsoner"></pre>
</form>
<script src="/sitecore_services/admin/jquery.min.js"></script>
<script src="/sitecore_services/admin/jquery.json-viewer.js"></script>
<script>
    $(".jsoner").each(function(index, elem) {
        var data = $(elem).data('json');
        $(elem).jsonViewer(data, { collapsed: true });
    })
    
</script>
</body>
</html>