using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class NoteValueMapping : NoteMappingBase
    {
        public override Guid Id { get; } = Guid.Parse("FDC43590-505A-4CE0-B3B9-DBE5A6F70987");

        public override string Name { get; } = "Note value";
        public override string Description { get; } = "Use note value as tokens";

        public override long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next)
        {
            return current.NoteNumber;
        }
    }
}