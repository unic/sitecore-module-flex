namespace Unic.Flex.Core.MarketingAutomation
{
    using System;
    using System.Reflection;
    using Glass.Mapper.Sc;
    using Model.MarketingAutomation;
    using Sitecore.Analytics;
    using Sitecore.Analytics.Model.Entities;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Reflection;

    public class MarketingAutomationContactService : IMarketingAutomationContactService
    {
        private readonly ISitecoreContext sitecoreContext;

        public MarketingAutomationContactService(ISitecoreContext sitecoreContext)
        {
            this.sitecoreContext = sitecoreContext;
        }

        public object GetContactValue(Guid contactFieldDefinitionId)
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

        public string GetEmailAddress(string name)
        {
            return this.GetEmailAddressEntry(name)?.SmtpAddress;
        }

        private IEmailAddress GetEmailAddressEntry(string name)
        {
            var contact = this.GetCurrentContact();
            var entries = this.GetEmailAdresses(contact).Entries;

            if (entries.Contains(name))
            {
                return entries[name];
            }

            return null;
        }

        public void SetEmailAddress(string name, string email)
        {
            var contact = this.GetCurrentContact();

            var entry = this.GetEmailAddressEntry(name);
            if (entry == null)
            {
                entry = this.GetEmailAdresses(contact).Entries.Create(name);
            }
            entry.SmtpAddress = email;
        }

        private IContactEmailAddresses GetEmailAdresses(Contact contact)
        {
            return contact.GetFacet<IContactEmailAddresses>("Emails");
        }

        public void SetContactValue(Guid contactFieldDefinitionId, object value)
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

        public void SetContactValue(string contactFieldName, string value)
        {
            var contact = this.GetCurrentContact();
            contact.Extensions.SimpleValues[contactFieldName] = value;
        }

        public object GetContactValue(string simpleFieldName)
        {
            var contact = this.GetCurrentContact();
            return contact.Extensions.SimpleValues[simpleFieldName];
        }

        private object GetFacet(Type facetType,  ContactFacetDefinition contactFacetDefinition)
        {
            var contact = this.GetCurrentContact();

            if (contact == null) return null;

            var getFacet = typeof(Contact).GetMethod(nameof(contact.GetFacet));
            var getFacetGeneric = getFacet.MakeGenericMethod(facetType);
            return getFacetGeneric.Invoke(contact, new object[] {contactFacetDefinition.FacetName});
        }

        private Contact GetCurrentContact()
        {
            return Tracker.Current.Contact;
        }
    }
}