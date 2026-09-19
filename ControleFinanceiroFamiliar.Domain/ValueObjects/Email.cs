namespace ControleFinanceiroFamiliar.Domain.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; init; }
        private Email(string value) { Value = value; }
        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Email obrigatório");
            value = value.Trim().ToLowerInvariant();
            if(!value.Contains('@') || value.Length < 5) throw new ArgumentException("Email inválido");

            return new Email(value);
        }
        public override string ToString() => Value;

    }
}
