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
            Assert.True(alexey.Group == group);
            Assert.True(group.Students.Contains(alexey));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = _isuService.AddGroup("M3109");
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
            Assert.Catch<IsuException>(() =>
            {
                Group group1 = _isuService.AddGroup("M3109");
                Group group2 = _isuService.AddGroup("M3100");
                Student alexey = _isuService.AddStudent(group1, "Alexey");

                _isuService.ChangeStudentGroup(alexey, group2);
                
                Assert.True(group1.Students.Contains(alexey));
            });
        }
    }
}