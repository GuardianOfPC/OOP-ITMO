using System.Collections.Generic;
using Isu.Models;

namespace IsuExtra.Models
{
    public class GroupWrapper
    {
        private GroupWrapper(Group group)
        {
            StreamStudents = new List<StreamStudent>();
            foreach (Student student in Group.Students)
            {
                StreamStudent tmp = new StreamStudent.StreamStudentBuilder()
                    .WithStudent(student)
                    .Build();
                StreamStudents.Add(tmp);
            }

            Group = group;
        }

        public Group Group { get; } = new Group.GroupBuilder().WithName(" ").Build();
        public List<StreamStudent> StreamStudents { get; }

        public class GroupWrapperBuilder
        {
            private Group _group;
            private List<Lesson> _lessons;

            public GroupWrapperBuilder WithGroup(Group group)
            {
                _group = group;
                return this;
            }

            public GroupWrapperBuilder WithLessons(List<Lesson> lessons)
            {
                _lessons = lessons;
                return this;
            }

            public GroupWrapper Build()
            {
                GroupWrapper final = new (_group);
                return final;
            }
        }
    }
}