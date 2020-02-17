using System.Collections.Generic;
using System.Linq;

namespace markov_drummer.Markov.Chiscore.Components
{
    public class LeastPopularUnigramSelector<T> : IUnigramSelector<T>
    {
        public T SelectUnigram(IEnumerable<T> ngrams)
        {
            return ngrams
                .GroupBy(a => a).OrderByDescending(a => a.Count())
                .FirstOrDefault()
                .FirstOrDefault();
        }
    }
}
