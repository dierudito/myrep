using System.ComponentModel.DataAnnotations;

namespace moreno.myrep.api.Models.Base
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; private set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
