namespace Unic.Flex.Mapping
{
    using System.Collections.Generic;
    using System.Web;

    /// <summary>
    /// Repository containg data access to user generated form data from the user session
    /// </summary>
    public class UserDataRepository : IUserDataRepository
    {
        /// <summary>
        /// The session key
        /// </summary>
        private const string SessionKey = "FLEX_FORM_USERDATA";

        /// <summary>
        /// Gets or sets the form session.
        /// </summary>
        /// <value>
        /// The form session.
        /// </value>
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

        /// <summary>
        /// Gets the value from the storage.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>
        /// Value loaded from the storage provider
        /// </returns>
        public object GetValue(string formId, string fieldId)
        {
            var formSession = this.GetFormValues(formId);
            if (formSession == null) return null;
            return formSession.ContainsKey(fieldId) ? formSession[fieldId] : null;
        }

        /// <summary>
        /// Sets and stores the value for a field in the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <param name="value">The value.</param>
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

        /// <summary>
        /// Gets the values from the session for a specific form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>Key-Value based dictionary with values</returns>
        private IDictionary<string, object> GetFormValues(string formId)
        {
            var session = this.FormSession;
            return session.ContainsKey(formId) ? session[formId] : null;
        }
    }
}
