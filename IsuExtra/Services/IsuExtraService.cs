using System;
using System.Collections.Generic;
using IsuExtra.Interfaces;
using IsuExtra.Models;

namespace IsuExtra.Services
{
    public class IsuExtraService : IIsuExtraService
    {
        public IsuExtraService(IOgnpRepository ognpRepository) => OgnpRepository = ognpRepository;
        public IOgnpRepository OgnpRepository { get; }

        public Ognp AddOgnpToRegister(Ognp ognp)
        {
            OgnpRepository.Add(ognp);
            return ognp;
        }

        public StreamGroup AddStudentToStreamGroup(StreamStudent streamStudent, StreamGroup streamGroup)
        {
            if (streamStudent.OgnpCount > 2) throw new Exception("Ognp overflow");
            if (streamStudent.MegaFaculty == streamGroup.Ognp.MegaFaculty) throw new Exception("Same faculty");
            if (!Lesson.IntersectionsCheck(streamGroup, streamStudent.GroupWrapper))
                throw new Exception("Lesson intersection");
            int ognpCount = streamStudent.OgnpCount;
            ognpCount++;
            streamStudent.ToBuild().WithOgnpCount(ognpCount).Build();
            var studentList = (List<StreamStudent>)streamGroup.StreamStudents;
            studentList.Add(streamStudent);
            streamGroup.ToBuild().WithStreamStudents(studentList).Build();
            return streamGroup;
        }

        public StreamGroup StudentOgnpRemoval(StreamStudent streamStudent, StreamGroup streamGroup)
        {
            int ognpCount = streamStudent.OgnpCount;
            ognpCount--;
            streamStudent.ToBuild().WithOgnpCount(ognpCount).Build();
            var studentList = (List<StreamStudent>)streamGroup.StreamStudents;
            studentList.Remove(streamStudent);
            streamGroup.ToBuild().WithStreamStudents(studentList).Build();
            return streamGroup;
        }

        public List<StreamGroup> GetStreamGroupsFromConcreteOgnp(List<StreamGroup> streamGroups, Ognp ognp)
        {
            var finalList = new List<StreamGroup>();
            foreach (StreamGroup streamGroup in streamGroups)
            {
                if (streamGroup.Ognp.Equals(ognp)) finalList.Add(streamGroup);
            }

            return finalList;
        }

        public List<StreamStudent> GetStudentsFromStreamGroup(StreamGroup streamGroup)
        {
            var result = new List<StreamStudent>(streamGroup.StreamStudents);
            return result;
        }

        public List<StreamStudent> GetStudentsWithoutOgnpFromGroup(GroupWrapper groupWrapper)
        {
            var result = new List<StreamStudent>();
            foreach (StreamStudent streamStudent in groupWrapper.StreamStudents)
            {
                if (streamStudent.OgnpCount == 0) result.Add(streamStudent);
            }

            return result;
        }
    }
}