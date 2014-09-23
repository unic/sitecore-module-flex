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
        /// The competed steps key
        /// </summary>
        private const string CompetedStepsKey = "COMPLETED_STEPS";

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
        public virtual object GetValue(string formId, string fieldId)
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
        public virtual void SetValue(string formId, string fieldId, object value)
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
        /// Determines whether a form is stored in the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>
        ///   <c>true</c> if the form is currently stored in the storage provider, <c>false</c> otherwise.
        /// </returns>
        public virtual bool IsFormStored(string formId)
        {
            return this.FormSession.ContainsKey(formId);
        }

        /// <summary>
        /// Determines whether a specific field from a form is actually stored in the storage provider or not.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="fieldId">The field identifier.</param>
        /// <returns>
        ///   <c>true</c> if the field is curretly stored in the storage provider, <c>false</c> otherwise.
        /// </returns>
        public virtual bool IsFieldStored(string formId, string fieldId)
        {
            var formSession = this.GetFormValues(formId);
            return formSession != null && formSession.ContainsKey(fieldId);
        }

        /// <summary>
        /// Clears the form values completely out of the storage provider.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        public virtual void ClearForm(string formId)
        {
            var session = this.FormSession;
            if (!session.ContainsKey(formId)) return;

            session.Remove(formId);
            this.FormSession = session;
        }

        /// <summary>
        /// Completes the step.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        public virtual void CompleteStep(string formId, int stepNumber)
        {
            var completedSteps = this.GetValue(formId, CompetedStepsKey) as List<int> ?? new List<int>();
            completedSteps.Add(stepNumber);
            this.SetValue(formId, CompetedStepsKey, completedSteps);
        }

        /// <summary>
        /// Reverts to step and invaldiate all completed step until the give one.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        public virtual void RevertToStep(string formId, int stepNumber)
        {
            var completedSteps = this.GetValue(formId, CompetedStepsKey) as List<int>;
            if (completedSteps == null) return;

            completedSteps.RemoveAll(step => step >= stepNumber);
            this.SetValue(formId, CompetedStepsKey, completedSteps);
        }

        /// <summary>
        /// Determines whether a step number has been completed.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <param name="stepNumber">The step number.</param>
        /// <returns>
        /// Boolean value if the given step has been completed or not
        /// </returns>
        public virtual bool IsStepCompleted(string formId, int stepNumber)
        {
            var completedSteps = this.GetValue(formId, CompetedStepsKey) as List<int>;
            return completedSteps != null && completedSteps.Contains(stepNumber);
        }

        /// <summary>
        /// Gets the values from the session for a specific form.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>Key-Value based dictionary with values</returns>
        public virtual IDictionary<string, object> GetFormValues(string formId)
        {
            var session = this.FormSession;
            return session.ContainsKey(formId) ? session[formId] : null;
        }
    }
}
