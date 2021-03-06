﻿using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudentManagementSystemLibrary.ModelProcessors;
using StudentManagementSystemLibrary.Models;
using StudentManagementSystemLibrary.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagementSystemLibrary.UnitTests.ModelProcessors
{
    [TestClass]
    public class Test_GroupRepository
    {
        [DataRow("exec dbo.spGroups_GetAll ;")]
        [DataTestMethod]
        public void GetGroups_All_ValidCall(string sql)
        {
            var sampleGroups = new DataConnectionSampleData().GetSampleGroups();

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataConnection>()
                    .Setup(x => x.GetData_All<GroupModel>(sql))
                    .Returns(sampleGroups);

                var groupRepository = mock.Create<GroupRepository>();

                var expected = sampleGroups;

                var actual = groupRepository.GetGroups_All();

                Assert.IsTrue(actual != null);
                Assert.AreEqual(expected.Count, actual.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].GroupId, actual[i].GroupId);
                    Assert.AreEqual(expected[i].CourseId, actual[i].CourseId);
                    Assert.AreEqual(expected[i].Name, actual[i].Name);
                }
            }
        }

        [DataRow("exec dbo.spStudents_GetByGroup @GroupId = 1 ;", 1)]
        [DataTestMethod]
        public void GetStudents_ByGroup_ValidCall(string sql, int groupId)
        {
            var sampleStudents = new DataConnectionSampleData().GetSampleStudents();
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataConnection>()
                    .Setup(x => x.GetListData_ById<StudentModel>(sql))
                    .Returns(sampleStudents);

                var groupRepository = mock.Create<GroupRepository>();

                var expected = sampleStudents;
                var actual = groupRepository.GetStudents_ByGroup(groupId);

                Assert.IsTrue(actual != null);
                Assert.AreEqual(expected.Count, actual.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].FirstName, actual[i].FirstName);
                    Assert.AreEqual(expected[i].LastName, actual[i].LastName);
                    Assert.AreEqual(expected[i].GroupId, actual[i].GroupId);
                    Assert.AreEqual(expected[i].StudentId, actual[i].StudentId);
                }
            }
        }

        [DataRow("exec dbo.spGroup_UpdateNameById @GroupId = 1, @UpdatedName = 'TestName' ; ", 1, "TestName")]
        [DataTestMethod]
        public void UpdateGroupName_ValidCall(string sql, int groupId, string updatedGroupName)
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataConnection>().Setup(x => x.UpdateData<GroupModel>(sql));

                var groupRepository = mock.Create<GroupRepository>();

                groupRepository.UpdateGroupName(groupId, updatedGroupName);

                mock.Mock<IDataConnection>().Verify(x => x.UpdateData<GroupModel>(sql), Times.Exactly(1));
            }
        }

        [DataRow("exec dbo.spGroup_DeleteById @GroupId = 1 ;")]
        [DataTestMethod]
        public void DeleteGroup_ValidCall(string sql)
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IDataConnection>().Setup(x => x.DeleteData<GroupModel>(sql));

                var groupRepository = mock.Create<GroupRepository>();

                groupRepository.DeleteGroup(1);

                mock.Mock<IDataConnection>().Verify(x => x.DeleteData<GroupModel>(sql), Times.Exactly(1));
            }
        }
    }
}
