using System.Collections.Generic;

namespace markov_drummer.Markov.Chiscore.Components
{
    public interface IUnigramSelector <TUnigram>
    {
        TUnigram SelectUnigram(IEnumerable<TUnigram> ngrams);
    }
}
