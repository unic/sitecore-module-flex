namespace Unic.Flex.Model.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Session
    {
        public Session()
        {
            this.Fields = new HashSet<Field>();
        }
        
        [Required]
        public virtual int Id { get; set; }

        [Required]
        public virtual Form Form { get; set; }

        [Required]
        public virtual string Language { get; set; }

        [Required]
        public virtual DateTime Timestamp { get; set; }

        public virtual ICollection<Field> Fields { get; set; }
    }
}
