﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IssueTracker.Models
{
    interface IIssueManager
    {
        void Create(Issue issue);
        void Update(Issue issue);
        List<Issue> GetAll();
        List<Issue> GetAll(Project project, int? idIssue, Type? type, State? state);
        List<Issue> GetAllFromProject(Project project);
        List<Issue> GetAllByType(Type type);
        List<Issue> GetAllByState(State state);
    }
}