using System;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class VelRestSensitiveEzDrummerNoteValueMapping : EzDrummerNoteValueMapping
    {
        public override Guid Id { get; } = Guid.Parse("F7900536-18FD-45EA-97B5-B207255B2D3D");

        public override string Name { get; } = "Vel-Time-Sensitive Toontrack (SD / EZD)";
        public override string Description { get; } = "Same as Toontrak (using Superior Drummer / EZDrummer keymapping), but tokens are also velocity-range sensitive and time-sensitive";
        
        public override long GetNoteHash(Note current, Note next)
        {
            var hash = base.GetNoteHash(current, next);

            hash <<= 4;

            hash += (current.Velocity) / 18;    //Split in 7 sections

            hash <<= 4;

            var delta = next?.Time - current.Time;

            delta = Math.Max(delta??0, current.Length);

            hash |= delta.Value;

            return hash;
        }
    }
}