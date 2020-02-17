using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class VelSensitiveEzDrummerNoteValueMapping : EzDrummerNoteValueMapping
    {
        public override Guid Id { get; } = Guid.Parse("B16B3927-9F30-463D-97BE-FB7116EEEE9B");

        public override string Name { get; } = "Vel-Sensitive Toontrack (SD / EZD)";
        public override string Description { get; } = "Same as Toontrak (using Superior Drummer / EZDrummer keymapping), but tokens are also velocity-range sensitive";
        
        public override long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next)
        {
            var hash = base.GetNoteHash(sourceToken, current, next);

            hash <<= 4;

            hash += (current.Velocity) / 18;    //Split in 7 sections

            return hash;
        }
    }
}