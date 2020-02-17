using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace markov_drummer.Vm.NoteMappers
{
    public static class AvailableMappersProvider
    {
        public static IEnumerable<NoteMappingBase> GetAllMappings()
        {
            yield return new VelRestSensitiveEzDrummerNoteValueMapping();
            yield return new VelSensitiveEzDrummerNoteValueMapping();
            yield return new EzDrummerNoteValueMapping();
            yield return new NoteValueMapping();
        }
    }
}
