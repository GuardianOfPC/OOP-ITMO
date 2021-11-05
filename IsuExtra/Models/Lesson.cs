using System;

namespace IsuExtra.Models
{
    public class Lesson
    {
        private Lesson(StreamGroup group, string teacher, int classRoomNumber, Ognp ognp, TimeSpan startTime, TimeSpan endTime)
        {
            Group = group;
            Teacher = teacher;
            ClassRoomNumber = classRoomNumber;
            Ognp = ognp;
            StartTime = startTime;
            EndTime = endTime;
        }

        public StreamGroup Group { get; }
        public string Teacher { get; }
        public int ClassRoomNumber { get; }
        public Ognp Ognp { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }

        public static bool IntersectionsCheck(StreamGroup streamGroup, GroupWrapper groupWrapper)
        {
            if (streamGroup == null) throw new Exception("Null object");
            if (groupWrapper == null) throw new Exception("Null object");
            foreach (Lesson groupWrapperLesson in groupWrapper.Lessons)
            {
                foreach (Lesson streamGroupLesson in streamGroup.Lessons)
                {
                    if (groupWrapperLesson.StartTime.Equals(streamGroupLesson.StartTime)) return false;
                }
            }

            return true;
        }
    }
}