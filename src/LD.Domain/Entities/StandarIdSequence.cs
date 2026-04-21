using LD.Domain.Common;

namespace LD.Domain.Entities
{
    public class StandarIdSequence : AuditableEntity
    {
        public int StandarIdSequenceId { get; set; }

        public DateTime SequenceDate { get; set; }

        public int LastNumber { get; set; }

        public DateTime LastUpdatedAt { get; set; }
    }
}
