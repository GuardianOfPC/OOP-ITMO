using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class StreamGroup
    {
        private StreamGroup(string name, List<StreamStudent> streamStudents, List<Lesson> lessons, Ognp ognp, int maxStudentCount)
        {
            Name = name;
            StreamStudents = new List<StreamStudent>(streamStudents);
            Lessons = new List<Lesson>(lessons);
            Ognp = ognp;
            MaxStudentCount = maxStudentCount;
        }

        public string Name { get; }
        public int MaxStudentCount { get; }
        public IReadOnlyList<Lesson> Lessons { get; }
        public IReadOnlyList<StreamStudent> StreamStudents { get; }
        public Ognp Ognp { get; }

        public StreamGroupBuilder ToBuild()
        {
            StreamGroupBuilder streamGroupBuilder = new ();
            streamGroupBuilder
                .WithLessons((List<Lesson>)Lessons)
                .WithOgnp(Ognp);
            return streamGroupBuilder;
        }

        public class StreamGroupBuilder
        {
            private string _name;
            private List<Lesson> _lessons = new ();
            private List<StreamStudent> _streamStudents = new ();
            private Ognp _ognp;
            private int _maxStudentCount;

            public StreamGroupBuilder WithMaxStudentCount(int val)
            {
                _maxStudentCount = val;
                return this;
            }

            public StreamGroupBuilder WithStreamStudents(List<StreamStudent> streamStudents)
            {
                _streamStudents = streamStudents;
                return this;
            }

            public StreamGroupBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public StreamGroupBuilder WithLessons(List<Lesson> lessons)
            {
                _lessons = lessons;
                return this;
            }

            public StreamGroupBuilder WithOgnp(Ognp ognp)
            {
                _ognp = ognp;
                return this;
            }

            public StreamGroup Build()
            {
                StreamGroup final = new (_name, _streamStudents, _lessons, _ognp, _maxStudentCount);
                return final;
            }
        }
    }
}