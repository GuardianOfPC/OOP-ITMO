using System;
using System.Collections.Generic;
using Isu.Models;

namespace IsuExtra.Models
{
    public class GroupWrapper
    {
        private readonly List<StreamStudent> _streamStudents;
        private readonly List<Lesson> _lessons;
        private GroupWrapper(List<StreamStudent> streamStudents, Group group, List<Lesson> lessons)
        {
            _streamStudents = new List<StreamStudent>(streamStudents);
            _lessons = new List<Lesson>(lessons);
            Group = group;
            if (group != null) MegaFaculty = DefineMegaFaculty(group.GroupName);
        }

        public Group Group { get; }
        public MegaFaculty MegaFaculty { get; private set; }
        public IReadOnlyList<StreamStudent> StreamStudents => _streamStudents;
        public IReadOnlyList<Lesson> Lessons => _lessons;

        private MegaFaculty DefineMegaFaculty(string name)
        {
            if (Group.GroupName[0] == 'K') MegaFaculty = MegaFaculty.Ctu;
            if (Group.GroupName[0] == 'M') MegaFaculty = MegaFaculty.Tint;
            if (Group.GroupName[0] == 'U') MegaFaculty = MegaFaculty.Ftmi;
            if (Group.GroupName[0] == 'T') MegaFaculty = MegaFaculty.Btins;
            if (Group.GroupName[0] == 'V') MegaFaculty = MegaFaculty.Ft;
            throw new Exception("No name");
        }

        public class GroupWrapperBuilder
        {
            private Group _group;
            private List<Lesson> _lessons = new ();
            private List<StreamStudent> _streamStudents = new ();

            public GroupWrapperBuilder WithStudents(List<StreamStudent> streamStudents)
            {
                _streamStudents = streamStudents;
                return this;
            }

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
                GroupWrapper final = new (_streamStudents, _group, _lessons);
                return final;
            }
        }
    }
}