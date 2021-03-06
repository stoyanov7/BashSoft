﻿namespace BashSoft.Repositories.Contracts
{
    public interface IDatabase : IRequester, IFilteredTaker, IOrderedTaker
    {
        void LoadData(string fileName = null);
        void UnloadData();
    }
}