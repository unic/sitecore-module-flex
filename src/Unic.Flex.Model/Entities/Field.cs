namespace Unic.Flex.Model.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Field
    {
        [Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual Session Session { get; set; }

        [Required]
        public virtual Guid ItemId { get; set; }

        [Required]
        public virtual string Value { get; set; }

        //// todo: add blob values for attachments
    }
}
