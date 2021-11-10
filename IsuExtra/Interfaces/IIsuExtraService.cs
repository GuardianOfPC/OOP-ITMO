using System.Collections.Generic;
using IsuExtra.Models;

namespace IsuExtra.Interfaces
{
    public interface IIsuExtraService
    {
        public IOgnpRepository OgnpRepository { get; }
        public Ognp AddOgnpToRegister(Ognp ognp);
        public StreamGroup StudentOgnpEntry(StreamStudent streamStudent, StreamGroup streamGroup);
        public StreamGroup StudentOgnpRemoval(StreamStudent streamStudent, StreamGroup streamGroup);
        public List<StreamGroup> GetStreamGroupsFromConcreteOgnp(List<StreamGroup> streamGroups, Ognp ognp);
        public List<StreamStudent> GetStudentsFromStreamGroup(StreamGroup streamGroup);
        public List<StreamStudent> GetStudentsWithoutOgnpFromGroup(GroupWrapper groupWrapper);
    }
}