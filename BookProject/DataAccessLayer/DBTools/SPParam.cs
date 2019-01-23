using System;

namespace DataAccessLayer.DBTools
{
    public class SPParam
    {
        public string Name { get; private set; }
        public object Value { get; private set; }

        public SPParam(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
