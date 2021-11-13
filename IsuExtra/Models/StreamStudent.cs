using System;
using Isu.Models;

namespace IsuExtra.Models
{
    public class StreamStudent
    {
        private StreamStudent(Student student, int ognpCount, MegaFaculty megaFaculty, GroupWrapper groupWrapper)
        {
            Student = new Student.StudentBuilder()
                .WithName(student.Name)
                .WithGroup(student.Group)
                .Build();
            OgnpCount = ognpCount;
            MegaFaculty = megaFaculty;
            GroupWrapper = groupWrapper;
        }

        public int OgnpCount { get; }
        public MegaFaculty MegaFaculty { get; }
        public Student Student { get; }
        public GroupWrapper GroupWrapper { get; }

        public StreamStudentBuilder ToBuild()
        {
            StreamStudentBuilder builder = new ();
            builder.WithStudent(Student)
                .WithMegaFaculty(MegaFaculty)
                .WithOgnpCount(OgnpCount)
                .WithGroupWrapper(GroupWrapper)
                .Build();
            return builder;
        }

        public class StreamStudentBuilder
        {
            private int _ognpCount;
            private MegaFaculty _megaFaculty;
            private Student _student;
            private GroupWrapper _groupWrapper;

            public StreamStudentBuilder WithGroupWrapper(GroupWrapper groupWrapper)
            {
                _groupWrapper = groupWrapper;
                return this;
            }

            public StreamStudentBuilder WithStudent(Student student)
            {
                _student = student;
                return this;
            }

            public StreamStudentBuilder WithOgnpCount(int ognpCount)
            {
                _ognpCount = ognpCount;
                return this;
            }

            public StreamStudentBuilder WithMegaFaculty(MegaFaculty megaFaculty)
            {
                _megaFaculty = megaFaculty;
                return this;
            }

            public StreamStudent Build()
            {
                StreamStudent final = new (_student, _ognpCount, _megaFaculty, _groupWrapper);
                return final;
            }
        }
    }
}
