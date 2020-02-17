using System;
using System.Collections.Generic;
using markov_drummer.Vm.NoteMappers;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Markov
{
    public class MarkovNoteToken : IEquatable<MarkovNoteToken>, IComparable<MarkovNoteToken>, IComparable
    {
        public TempoMap TargetTempoMap { get; }

        public long SignificantMatchingId { get; }

        public static MarkovNoteToken Empty { get; } = new MarkovNoteToken(null, null, null, null){ IsEmpty = true};
        
        public bool IsEmpty { get; private set; }
        public long DurationWithSilence { get;  }

        public MarkovNoteToken(Note current, Note next, NoteMappingBase noteMapping, TempoMap targetTempoMap)
        {
            TargetTempoMap = targetTempoMap;

            if (next != null && current != null)
            {
                DurationWithSilence = next.Time - current.Time;

                DurationWithSilence = Math.Max(DurationWithSilence, current.Length);

                SignificantMatchingId = noteMapping.GetNoteHash(this, current, next);
            }
        }

        public int CompareTo(MarkovNoteToken other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return SignificantMatchingId.CompareTo(other.SignificantMatchingId);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            if (!(obj is MarkovNoteToken)) throw new ArgumentException($"Object must be of type {nameof(MarkovNoteToken)}");
            return CompareTo((MarkovNoteToken) obj);
        }
        
        public static bool operator <(MarkovNoteToken left, MarkovNoteToken right) => 
            Comparer<MarkovNoteToken>.Default.Compare(left, right) < 0;

        public static bool operator >(MarkovNoteToken left, MarkovNoteToken right) => 
            Comparer<MarkovNoteToken>.Default.Compare(left, right) > 0;

        public static bool operator <=(MarkovNoteToken left, MarkovNoteToken right) => 
            Comparer<MarkovNoteToken>.Default.Compare(left, right) <= 0;

        public static bool operator >=(MarkovNoteToken left, MarkovNoteToken right) => 
            Comparer<MarkovNoteToken>.Default.Compare(left, right) >= 0;

        public bool Equals(MarkovNoteToken other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return SignificantMatchingId == other.SignificantMatchingId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MarkovNoteToken) obj);
        }

        public override int GetHashCode() => SignificantMatchingId.GetHashCode();

        public static bool operator ==(MarkovNoteToken left, MarkovNoteToken right) => Equals(left, right);
        public static bool operator !=(MarkovNoteToken left, MarkovNoteToken right) => !Equals(left, right);

    }
}
