using System.Collections.Generic;
using markov_drummer.Markov;
using markov_drummer.Markov.Chiscore.Components;

namespace markov_drummer.Services
{
    public static class AvailableUnigramSelectorsProvider
    {
        public static IEnumerable<UnigramSelectorBase> GetUnigramSelectors()
        {
            yield return new WeightedRandomUnigramSelector<MarkovNoteToken>();
            yield return new UnweightedRandomUnigramSelector<MarkovNoteToken>();
            yield return new MostPopularUnigramSelector<MarkovNoteToken>();
            yield return new LeastPopularUnigramSelector<MarkovNoteToken>();
            yield return new AlphabeticUnigramSelector<MarkovNoteToken>();
            yield return new AlphabeticUnigramSelectorDesc<MarkovNoteToken>();
        }
    }
}