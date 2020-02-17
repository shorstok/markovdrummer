using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class OnlyVelocityMapping : NoteMappingBase
    {
        public override Guid Id { get; } = Guid.Parse("50270F65-504A-4BAF-984E-9C070746AEB6");

        public override string Name { get; } = "Note velocity only";
        public override string Description { get; } = "Use note velocity only as token";

        public override long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next)
        {
            return current.Velocity / 10;
        }
    }
}