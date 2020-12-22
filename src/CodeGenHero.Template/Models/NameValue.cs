namespace CodeGenHero.Template.Models
{
    public class NameValue : BaseNameValue<string>
    {
        public NameValue()
        {
        }

        public NameValue(string name, string value)
        {
            if (name == null)
                throw new System.ArgumentNullException(nameof(name));

            Name = name;
            Value = value;
        }
    }

    public class NameValue<T> : BaseNameValue<T>
    {
        public NameValue()
        {
        }

        public NameValue(string name, T value)
        {
            if (name == null)
                throw new System.ArgumentNullException(nameof(name));

            Name = name;
            Value = value;
        }
    }
}