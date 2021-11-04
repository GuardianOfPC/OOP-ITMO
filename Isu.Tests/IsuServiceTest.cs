using System.Linq;
using Isu.Models;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Group group = _isuService.AddGroup("M3109");
            Student alexey = _isuService.AddStudent(group, "Alexey");
            Assert.True(alexey.Group.Equals(group));
            Assert.True(group.Students.Contains(alexey));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {   
            Group group = _isuService.AddGroup("M3109");
            Assert.Catch<IsuException>(() =>
            {
                for (int i = 0; i < 40; ++i)
                {
                    _isuService.AddStudent(group, "test");
                }
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("123456789");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            Group group1 = _isuService.AddGroup("M3109");
            Group group2 = _isuService.AddGroup("M3100");
            Student alexey = _isuService.AddStudent(group1, "Alexey");

            _isuService.ChangeStudentGroup(alexey, group2);
            
            Assert.False(group1.Students.Contains(alexey));
            Assert.True(group2.Students.Contains(alexey));
        }
    }
} 