using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Mapping
{
    using System.Web;

    public class UserDataRepository : IUserDataRepository
    {
        private const string SessionKey = "FLEX_FORM_USERDATA";

        private IDictionary<string, IDictionary<string, object>> FormSession
        {
            get
            {
                var session = HttpContext.Current.Session[SessionKey] as IDictionary<string, IDictionary<string, object>>;
                if (session == null)
                {
                    session = new Dictionary<string, IDictionary<string, object>>();
                    this.FormSession = session;
                }

                return session;
            }
            set
            {
                HttpContext.Current.Session[SessionKey] = value;
            }
        }
        
        public object GetValue(string formId, string fieldId)
        {
            var formSession = this.GetFormValues(formId);
            if (formSession == null) return null;
            return formSession.ContainsKey(fieldId) ? formSession[fieldId] : null;
        }

        public void SetValue(string formId, string fieldId, object value)
        {
            var session = this.FormSession;
            if (!session.ContainsKey(formId))
            {
                session[formId] = new Dictionary<string, object>();
            }

            session[formId][fieldId] = value;
            this.FormSession = session;
        }

        private IDictionary<string, object> GetFormValues(string formId)
        {
            var session = this.FormSession;
            return session.ContainsKey(formId) ? session[formId] : null;
        }
    }
}
