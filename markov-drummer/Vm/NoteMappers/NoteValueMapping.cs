using System;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class NoteValueMapping : NoteMappingBase
    {
        public override Guid Id { get; } = Guid.Parse("FDC43590-505A-4CE0-B3B9-DBE5A6F70987");

        public override string Name { get; } = "Note value";
        public override string Description { get; } = "use note value as tokens";

        public override long GetNoteHash(Note current, Note next)
        {
            return current.NoteNumber;
        }
    }
}