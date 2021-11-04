using System;

namespace IsuExtra.Models
{
    public class Lesson
    {
        private Lesson(StreamGroup group, string teacher, int classRoomNumber, Ognp ognp, TimeSpan timeSpan)
        {
            Group = group;
            Teacher = teacher;
            ClassRoomNumber = classRoomNumber;
            Ognp = ognp;
            Time = timeSpan;
        }

        public StreamGroup Group { get; }
        public string Teacher { get; }
        public int ClassRoomNumber { get; }
        public Ognp Ognp { get; }
        public TimeSpan Time { get; }
    }
}