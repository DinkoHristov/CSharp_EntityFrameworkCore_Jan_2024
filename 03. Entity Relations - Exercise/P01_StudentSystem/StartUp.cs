﻿using P01_StudentSystem.Data;

namespace P01_StudentSystem
{
    public class StartUp
    {
        private static void Main(string[] args)
        {
            var dbContext = new StudentSystemContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}