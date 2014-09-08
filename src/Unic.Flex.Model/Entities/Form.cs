namespace Unic.Flex.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Form
    {
        public Form()
        {
            this.Sessions = new HashSet<Session>();
        }
        
        [Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual Guid ItemId { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
