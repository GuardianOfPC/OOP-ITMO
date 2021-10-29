using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class Ognp
    {
        private readonly string _name;

        private Ognp(string name)
        {
            _name = name;
        }

        public OgnpBuilder ToBuilder()
        {
            OgnpBuilder newOgnp = new ();
            newOgnp
                .WithName(_name);
            return newOgnp;
        }

        public class OgnpBuilder
        {
            private string _name;

            public OgnpBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public Ognp Build()
            {
                Ognp finalOgnp = new (_name);
                return finalOgnp;
            }
        }
    }
}