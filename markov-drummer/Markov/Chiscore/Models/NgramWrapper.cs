using System.Linq;

namespace markov_drummer.Markov.Chiscore.Models
{
    public class NgramContainer<T>
    {
        public NgramContainer(params T[] args)
        {
            Ngrams = args;
        }

        internal T[] Ngrams { get; }

        public override bool Equals(object o)
        {
            if (!(o is NgramContainer<T> testObj))
                return false;

            return Ngrams.OrderBy(a => a).SequenceEqual(testObj.Ngrams.OrderBy(a => a));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                var defaultVal = default(T);

                foreach (var member in Ngrams.Where(a => a != null && !a.Equals(defaultVal)))
                    hash = hash * 23 + member.GetHashCode();

                return hash;
            }
        }
    }
}