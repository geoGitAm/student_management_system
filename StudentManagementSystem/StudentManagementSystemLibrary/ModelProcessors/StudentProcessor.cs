﻿using StudentManagementSystemLibrary.IdentityMapServices;
using StudentManagementSystemLibrary.Models;
using StudentManagementSystemLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagementSystemLibrary.ModelProcessors
{
    public class StudentProcessor
    {
        private IRepository _database;
       // public StudentIdentityMap IdentityMap = new StudentIdentityMap(); 

        public StudentProcessor(IRepository repository)
        {
            _database = repository;
        }

        /// <summary>
        /// Gets all student information from the database.
        /// </summary>
        /// <returns>A list of student information.</returns>
        public List<StudentModel> GetStudents_All()
        {
            string sql = "exec dbo.spStudents_GetAll;";

            var output = _database.GetData_All<StudentModel>(sql);

            return output;
        }

        // TODO - Delete later. UpdateStudentName I mooved it to unit of work

        /// <summary>
        /// Updates first and second names for the student specified by id.
        /// </summary>
        /// <param name="studentId">Student id.</param>
        /// <param name="updatedFirstName">Updated (new) first name for the student.</param>
        /// <param name="updatedLastName">Updated (new) last name for the student.</param>
        public void UpdateStudent(int studentId, string updatedFirstName, string updatedLastName)
        {
            string sql = "exec dbo.spStudent_UpdateNameById " +
                "@StudentId = STUDENT_ID, " +
                "@UpdatedFirstName = UPDATED_FIRST_NAME, " +
                "@UpdatedLastName = UPDATED_LAST_NAME ;";

            sql = sql.Replace("STUDENT_ID", $"{ studentId }");
            sql = sql.Replace("UPDATED_FIRST_NAME", $"'{ updatedFirstName }'");
            sql = sql.Replace("UPDATED_LAST_NAME", $"'{ updatedLastName }'");

            _database.UpdateData<StudentModel>(sql);
        }

        /// <summary>
        /// Gets student info from the database by id.
        /// </summary>
        /// <param name="studentId">Student id.</param>
        /// <returns>Student info from the database by id.</returns>
        public StudentModel GetStudent_ById(int studentId)
        {
            string sql = "exec dbo.spStudent_GetById @StudentId = STUDENT_ID ;";

            sql = sql.Replace("STUDENT_ID", $"{ studentId }");

            return _database.GetSingleData_ById<StudentModel>(sql);
        }

        /// <summary>
        /// Gets all student information by group from the database.
        /// </summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>A list of student information by groups.</returns>
        public List<StudentModel> GetStudents_ByGroup(int groupId)
        {
            string sql = "exec dbo.spStudents_GetByGroup @GroupId = GROUP_ID ;";
            sql = sql.Replace("GROUP_ID", $"{ groupId }");

            var output = _database.GetListData_ById<StudentModel>(sql);

            foreach (var student in output)
            {
                CacheManager.StudentIdentityMap.AddItem(student);
            }

            return output;

            //return _database.GetListData_ById<StudentModel>(sql);
        }
    }
}
