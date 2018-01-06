using System;

namespace Assets.Skrypty.Tests
{
    internal class TestClassAttribute : Attribute
    {
        string info;

        public override string ToString()
        {
            return Info;
        }

        public TestClassAttribute()
        {
            Info = "Klasa testowa";
        }

        public TestClassAttribute(string info)
        {
            Info = info;
        }

        public string Info
        {
            get
            {
                return info;
            }

            set
            {
                info = value;
            }
        }
    }
}