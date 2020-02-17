using System;
using markov_drummer.Markov;
using Melanchall.DryWetMidi.Interaction;

namespace markov_drummer.Vm.NoteMappers
{
    public class VelRestSensitiveEzDrummerNoteValueMapping : EzDrummerNoteValueMapping
    {
        public override Guid Id { get; } = Guid.Parse("F7900536-18FD-45EA-97B5-B207255B2D3D");

        public override string Name { get; } = "Vel-Time-Sensitive Toontrack (SD / EZD)";
        public override string Description { get; } = "Same as Toontrak (using Superior Drummer / EZDrummer keymapping), but tokens are also velocity-range sensitive and note length-sensitive";

        private long _quantizeDuration = 0;

        public override long GetNoteHash(MarkovNoteToken sourceToken, Note current, Note next)
        {
            if (0 == _quantizeDuration)
                _quantizeDuration = TimeConverter.ConvertFrom(new MusicalTimeSpan(1, 32), sourceToken.TargetTempoMap);

            var hash = base.GetNoteHash(sourceToken, current, next);

            hash <<= 4;

            hash += (current.Velocity) / 18;    //Split in 7 sections

            hash <<= 4;

            var delta = sourceToken.DurationWithSilence;

            delta = Math.Max(delta, current.Length);

            delta /= _quantizeDuration;

            hash |= delta;
            

            return hash;
        }
    }
}