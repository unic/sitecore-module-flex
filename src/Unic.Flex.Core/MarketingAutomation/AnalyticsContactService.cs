namespace Unic.Flex.Core.MarketingAutomation
{
    using System;
    using System.Reflection;
    using Glass.Mapper.Sc;
    using Model.MarketingAutomation;
    using Sitecore.Analytics.Model.Entities;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Reflection;
    using Utilities;

    public class MarketingAutomationContactService : IMarketingAutomationContactService
    {
        private readonly ISitecoreContext sitecoreContext;
        private readonly ITrackerWrapper trackerWrapper;

        public MarketingAutomationContactService(ISitecoreContext sitecoreContext, ITrackerWrapper trackerWrapper)
        {
            this.sitecoreContext = sitecoreContext;
            this.trackerWrapper = trackerWrapper;
        }

        public object GetContactValue(Guid contactFieldDefinitionId)
        {
            using (new VersionCountDisabler())
            {
                var contactFieldDefinition = this.sitecoreContext.GetItem<ContactFieldDefinition>(contactFieldDefinitionId);

                if (contactFieldDefinition == null) return null;

                var contactFacetDefinition = contactFieldDefinition.Facet;

                if (contactFacetDefinition == null) return null;

                var facetType = ReflectionUtil.GetTypeInfo(contactFacetDefinition.Type);
                var facet = this.GetFacet(facetType, contactFacetDefinition);

                var fieldProperty = facetType.GetProperty(contactFieldDefinition.FieldName, BindingFlags.Public | BindingFlags.Instance);

                if (fieldProperty == null) return null;

                return fieldProperty.GetValue(facet);
            }
        }

        public string GetEmailAddress(string name)
        {
            return this.GetEmailAddressEntry(name)?.SmtpAddress;
        }

        private IEmailAddress GetEmailAddressEntry(string name)
        {
            var contact = this.trackerWrapper.GetCurrentTracker().Contact;
            var entries = this.GetEmailAdresses(contact).Entries;

            if (entries.Contains(name))
            {
                return entries[name];
            }

            return null;
        }

        public void SetEmailAddress(string name, string email)
        {
            var contact = this.trackerWrapper.GetCurrentTracker().Contact;

            var entry = this.GetEmailAddressEntry(name);
            if (entry == null)
            {
                entry = this.GetEmailAdresses(contact).Entries.Create(name);
            }
            entry.SmtpAddress = email;
        }

        public void IdentifyContact(string identifier)
        {
            var hash = SecurityUtil.GenerateHash(identifier);

            var tracker = this.trackerWrapper.GetCurrentTracker();
            tracker.Session.Identify(hash);
        }

        private IContactEmailAddresses GetEmailAdresses(Contact contact)
        {
            return contact.GetFacet<IContactEmailAddresses>("Emails");
        }

        public void SetContactValue(Guid contactFieldDefinitionId, object value)
        {
            using (new VersionCountDisabler())
            {
                var contactFieldDefinition = this.sitecoreContext.GetItem<ContactFieldDefinition>(contactFieldDefinitionId);

                if (contactFieldDefinition == null) return;

                var contactFacetDefinition = contactFieldDefinition.Facet;

                if (contactFacetDefinition == null) return;

                var facetType = ReflectionUtil.GetTypeInfo(contactFacetDefinition.Type);
                var facet = this.GetFacet(facetType, contactFacetDefinition);

                var fieldProperty = facetType.GetProperty(contactFieldDefinition.FieldName, BindingFlags.Public | BindingFlags.Instance);

                if (fieldProperty == null) return;

                fieldProperty.SetValue(facet, value);
            }
        }

        public void SetContactValue(string contactFieldName, string value)
        {
            var contact = this.trackerWrapper.GetCurrentTracker().Contact;
            contact.Extensions.SimpleValues[contactFieldName] = value;
        }

        public object GetContactValue(string simpleFieldName)
        {
            var contact = this.trackerWrapper.GetCurrentTracker().Contact;
            return contact.Extensions.SimpleValues[simpleFieldName];
        }

        private object GetFacet(Type facetType,  ContactFacetDefinition contactFacetDefinition)
        {
            var contact = this.trackerWrapper.GetCurrentTracker().Contact;

            if (contact == null) return null;

            var getFacet = typeof(Contact).GetMethod(nameof(contact.GetFacet));
            var getFacetGeneric = getFacet.MakeGenericMethod(facetType);
            return getFacetGeneric.Invoke(contact, new object[] {contactFacetDefinition.FacetName});
        }
    }
}