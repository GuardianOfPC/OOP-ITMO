using Isu.Services;

namespace IsuExtra.Models
{
    public class Lesson
    {
        private readonly Group _group;
        private readonly string _teacher;
        private readonly int _classRoomNumber;
        private readonly Ognp _ognp;

        private Lesson(Group group, string teacher, int classRoomNumber, Ognp ognp)
        {
            _group = group;
            _teacher = teacher;
            _classRoomNumber = classRoomNumber;
            _ognp = ognp;
        }
    }
}