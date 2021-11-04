namespace IsuExtra.Models
{
    public class Ognp
    {
        private Ognp(string name, MegaFaculty megaFaculty)
        {
            Name = name;
            MegaFaculty = megaFaculty;
        }

        public string Name { get; }

        public MegaFaculty MegaFaculty { get; }

        public OgnpBuilder ToBuilder()
        {
            OgnpBuilder newOgnp = new ();
            newOgnp
                .WithName(Name)
                .WithMegaFaculty(MegaFaculty);
            return newOgnp;
        }

        public class OgnpBuilder
        {
            private string _name;
            private MegaFaculty _megaFaculty;

            public OgnpBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public OgnpBuilder WithMegaFaculty(MegaFaculty megaFaculty)
            {
                _megaFaculty = megaFaculty;
                return this;
            }

            public Ognp Build()
            {
                Ognp finalOgnp = new (_name, _megaFaculty);
                return finalOgnp;
            }
        }
    }
}