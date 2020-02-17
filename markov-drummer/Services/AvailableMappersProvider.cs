using System.Collections.Generic;
using markov_drummer.Vm.NoteMappers;

namespace markov_drummer.Services
{
    public static class AvailableMappersProvider
    {
        public static IEnumerable<NoteMappingBase> GetAllMappings()
        {
            yield return new VelRestSensitiveEzDrummerNoteValueMapping();
            yield return new VelSensitiveEzDrummerNoteValueMapping();
            yield return new EzDrummerNoteValueMapping();
            yield return new NoteValueMapping();
            yield return new OnlyVelocityMapping();
            yield return new OnlyDurationMapping();
        }
    }
}
