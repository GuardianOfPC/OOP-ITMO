using System;
using System.Collections.Generic;
using System.Linq;
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
            streamStudent = streamStudent.ToBuild().WithOgnpCount(ognpCount).Build();
            var studentList = (List<StreamStudent>)streamGroup.StreamStudents;
            studentList.Add(streamStudent);
            streamGroup = streamGroup.ToBuild().WithStreamStudents(studentList).Build();
            return streamGroup;
        }

        public StreamGroup StudentOgnpRemoval(StreamStudent streamStudent, StreamGroup streamGroup)
        {
            int ognpCount = streamStudent.OgnpCount;
            ognpCount--;
            streamStudent = streamStudent.ToBuild().WithOgnpCount(ognpCount).Build();
            var studentList = (List<StreamStudent>)streamGroup.StreamStudents;
            StreamStudent studentToDelete = null;
            foreach (StreamStudent student in studentList.Where(student => student.Student.Name == streamStudent.Student.Name))
            {
                studentToDelete = student;
            }

            studentList.Remove(studentToDelete);
            streamGroup = streamGroup.ToBuild().WithStreamStudents(studentList).Build();
            return streamGroup;
        }

        public List<StreamGroup> GetStreamGroupsFromConcreteOgnp(List<StreamGroup> streamGroups, Ognp ognp)
        {
            return streamGroups.Where(streamGroup => streamGroup.Ognp.Equals(ognp)).ToList();
        }

        public List<StreamStudent> GetStudentsFromStreamGroup(StreamGroup streamGroup)
        {
            var result = new List<StreamStudent>(streamGroup.StreamStudents);
            return result;
        }

        public List<StreamStudent> GetStudentsWithoutOgnpFromGroup(GroupWrapper groupWrapper)
        {
            return groupWrapper.StreamStudents.Where(streamStudent => streamStudent.OgnpCount == 0).ToList();
        }
    }
}