namespace BashSoft.Exceptions
{
    using System;

    public class DuplicateEntryInStructureException : Exception
    {
        private const string DuplicateEntry = "This {0} is already enrolled in {1}.";

        public DuplicateEntryInStructureException(string message)
            : base(message)
        {
        }

        public DuplicateEntryInStructureException(string entry, string structure)
            : base(string.Format(DuplicateEntry, entry, structure))
        {
        }
    }
}