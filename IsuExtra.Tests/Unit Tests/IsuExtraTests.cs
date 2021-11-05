using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Services;
using IsuExtra.Models;
using IsuExtra.Services;
using NUnit.Framework;

namespace IsuExtra.Tests.Unit_Tests
{
    public class IsuExtraTests
    {
        private IsuService _isuService = new ();
        private IsuExtraService _isuExtra = new ();

        [Test]
        public void AddOgnpToRegister_OgnpIsInTheRegister()
        {
            Ognp ognp = _isuExtra.AddOgnpToRegister("Data Science", MegaFaculty.Tint );
            Assert.True(_isuExtra.OgnpRegister.Contains(ognp));
        }

        [Test]
        public void StudentEntryToOgnp_StudentIsInStreamGroup()
        {
            Ognp ognp = _isuExtra.AddOgnpToRegister("CyberSec", MegaFaculty.Ctu );
            Student student = new Student.StudentBuilder().WithName("Alex").Build();
            GroupWrapper groupWrapper = new GroupWrapper.GroupWrapperBuilder().Build();
            StreamStudent streamStudent = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithGroupWrapper(groupWrapper)
                .Build();
            StreamGroup streamGroup = new StreamGroup.StreamGroupBuilder().WithOgnp(ognp).Build();
            _isuExtra.StudentOgnpEntry(streamStudent, streamGroup);
            Assert.True(streamGroup.StreamStudents.Contains(streamStudent));
        }
        
        [Test]
        public void StudentRemovalFromOgnp_StudentRemovedFromOgnp()
        {
            Ognp ognp = _isuExtra.AddOgnpToRegister("CyberSec", MegaFaculty.Ctu );
            Student student = new Student.StudentBuilder().WithName("Alex").Build();
            GroupWrapper groupWrapper = new GroupWrapper.GroupWrapperBuilder().Build();
            StreamStudent streamStudent = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithGroupWrapper(groupWrapper)
                .Build();
            StreamGroup streamGroup = new StreamGroup.StreamGroupBuilder().WithOgnp(ognp).Build();
            _isuExtra.StudentOgnpEntry(streamStudent, streamGroup);
            Assert.True(streamGroup.StreamStudents.Contains(streamStudent));
            _isuExtra.StudentOgnpRemoval(streamStudent, streamGroup);
            Assert.True(!streamGroup.StreamStudents.Contains(streamStudent));
            
        }
        
        [Test]
        public void GetStreamGroupsByOgnp_StreamGroupsFound()
        {
            Ognp ognp1 = _isuExtra.AddOgnpToRegister("CyberSec", MegaFaculty.Ctu );
            Ognp ognp2 = _isuExtra.AddOgnpToRegister("Data Science", MegaFaculty.Tint );
            StreamGroup streamGroup1 = new StreamGroup.StreamGroupBuilder().WithOgnp(ognp1).Build();
            StreamGroup streamGroup2 = new StreamGroup.StreamGroupBuilder().WithOgnp(ognp1).Build();
            StreamGroup streamGroup3 = new StreamGroup.StreamGroupBuilder().WithOgnp(ognp2).Build();
            var streamGroupsList = new List<StreamGroup> {streamGroup1, streamGroup2, streamGroup3};
            List<StreamGroup> result = _isuExtra.GetStreamGroupsFromConcreteOgnp(streamGroupsList, ognp1);
            Assert.True(result.Count == 2);
        }
        [Test]
        public void GetStreamStudentsListFromStreamGroup_StreamStudentsListReturned()
        {
            Ognp ognp = _isuExtra.AddOgnpToRegister("CyberSec", MegaFaculty.Ctu );
            GroupWrapper groupWrapper = new GroupWrapper.GroupWrapperBuilder().Build();
            Student student1 = new Student.StudentBuilder().WithName("Alex").Build();
            StreamStudent streamStudent1 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student1)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithGroupWrapper(groupWrapper)
                .Build();
            Student student2 = new Student.StudentBuilder().WithName("Bob").Build();
            StreamStudent streamStudent2 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student2)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithGroupWrapper(groupWrapper)
                .Build();
            Student student3 = new Student.StudentBuilder().WithName("Robin").Build();
            StreamStudent streamStudent3 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student3)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithGroupWrapper(groupWrapper)
                .Build();
            StreamGroup streamGroup = new StreamGroup.StreamGroupBuilder()
                .WithOgnp(ognp)
                .Build();
            _isuExtra.StudentOgnpEntry(streamStudent1, streamGroup);
            _isuExtra.StudentOgnpEntry(streamStudent2, streamGroup);
            _isuExtra.StudentOgnpEntry(streamStudent3, streamGroup);
            List<StreamStudent> result = _isuExtra.GetStudentsFromStreamGroup(streamGroup);
            Assert.True(result.Count == 3);
        }

        [Test]
        public void GetStreamStudentsWithoutOgnp_StreamStudentsListReturned()
        {
            Student student1 = new Student.StudentBuilder().WithName("Alex").Build();
            StreamStudent streamStudent1 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student1)
                .WithMegaFaculty(MegaFaculty.Tint)
                .Build();
            Student student2 = new Student.StudentBuilder().WithName("Bob").Build();
            StreamStudent streamStudent2 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student2)
                .WithMegaFaculty(MegaFaculty.Tint)
                .Build();
            Student student3 = new Student.StudentBuilder().WithName("Robin").Build();
            StreamStudent streamStudent3 = new StreamStudent.StreamStudentBuilder()
                .WithStudent(student3)
                .WithMegaFaculty(MegaFaculty.Tint)
                .WithOgnpCount(1)
                .Build();
            var tmp = new List<StreamStudent> {streamStudent1, streamStudent2, streamStudent3};
            GroupWrapper groupWrapper = new GroupWrapper.GroupWrapperBuilder().WithStudents(tmp).Build();
            List<StreamStudent> result = _isuExtra.GetStudentsWithoutOgnpFromGroup(groupWrapper);
            Assert.True(result.Count == 2);
        }
    }
}