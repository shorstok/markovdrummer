using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public abstract class NoteMappingBase : IEquatable<NoteMappingBase>
    {
        public bool Equals(NoteMappingBase other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NoteMappingBase) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(NoteMappingBase left, NoteMappingBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NoteMappingBase left, NoteMappingBase right)
        {
            return !Equals(left, right);
        }

        public virtual Guid Id { get; }

        public abstract string Name { get; }
        public abstract string Description { get; }

        public abstract long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next);
    }
}
