using System.Text;

namespace LegacyRenewalApp
{
    internal class PricingNotes
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public void Add(string note)
        {
            _builder.Append(note);
            _builder.Append("; ");
        }

        public override string ToString()
        {
            return _builder.ToString().Trim();
        }
    }
}
