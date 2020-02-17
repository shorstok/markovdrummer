using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class AlphabeticUnigramSelectorDesc<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .OrderByDescending(a => a)
                .FirstOrDefault();
        }
    }
}
