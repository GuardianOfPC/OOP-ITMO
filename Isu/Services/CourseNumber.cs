using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        private const int MinCourseNumber = 1;
        private const int MaxCourseNumber = 4;
        private int _number;

        public int Number
        {
            get => _number;
            set
            {
                if (value >= MinCourseNumber | value <= MaxCourseNumber)
                {
                    _number = value;
                }
                else
                {
                    throw new IsuException(message: "You can't insert CourseNumber other then 1 - 4");
                }
            }
        }
    }
}