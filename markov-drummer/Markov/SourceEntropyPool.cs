using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Markov
{
    public class SourceEntropyPool
    {
        private class PoolEntity
        {
            private const int PoolEntityMax = 1024;

            private int RoundRobinGetter { get; set; } = 0;
            private List<Note> Notes { get; } = new List<Note>();

            private HashSet<int> PooledNotes { get; } = new HashSet<int>();

            public void PoolNote(Note note)
            {
                if(Notes.Count>=PoolEntityMax)
                    return;

                if(PooledNotes.Contains(CalculateSignificantHash(note)))
                    return;

                PooledNotes.Add(CalculateSignificantHash(note));
                Notes.Add(note);
            }

            private int CalculateSignificantHash(Note note)
            {
                unchecked
                {
                    long result = note.Velocity * 13;
                    result += note.NoteNumber * 7;
                    result += note.Octave * 23;
                    result += note.Length * 31;

                    return (int) result;
                }
            }

            public Note GetPooledNote()
            {
                var rz = Notes[RoundRobinGetter].Clone();

                RoundRobinGetter = (RoundRobinGetter + 1) % Notes.Count;

                return rz;
            }
        }

        private readonly Dictionary<long, PoolEntity> _pool = new Dictionary<long, PoolEntity>();

        public void AddTokenToPool(MarkovNoteToken token, Note note)
        {
            if(token.IsEmpty)
                return;

            if (!_pool.TryGetValue(token.SignificantMatchingId, out var list))
            {
                list = new PoolEntity();
                _pool[token.SignificantMatchingId] = list;
            }

            list.PoolNote(note);
        }

        public Note GetFromPool(MarkovNoteToken token)
        {
            if (!_pool.TryGetValue(token.SignificantMatchingId, out var pool))
                return null;

            return pool.GetPooledNote();
        }
    }
}
