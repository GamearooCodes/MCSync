﻿using ClussPro.ObjectBasedFramework;
using ClussPro.ObjectBasedFramework.Schema.Attributes;
using ClussPro.ObjectBasedFramework.Validation.Attributes;
using System.Collections.Generic;
using WebModels.account;
using WebModels.auth;
using WebModels.company;
using WebModels.gov;
using WebModels.tow;

namespace WebModels.security
{
    [Table("554BDF2E-ACA5-46AB-A56E-080E6823267F")]
    public class User : DataObject
    {
        protected User() : base() { }

        private long? _userID;
        [Field("7BD9DFA8-8EC1-4159-AEDF-5A9330AEF3EB")]
        public long? UserID
        {
            get { CheckGet(); return _userID; }
            set { CheckSet(); _userID = value; }
        }

        private string _username;
        [Field("DF59672D-B5FB-43F7-8B42-BFC44097B54E", DataSize = 50)]
        [Required]
        public string Username
        {
            get { CheckGet(); return _username; }
            set { CheckSet(); _username = value; }
        }

        #region Relationships
        #region auth
        private List<Token> _tokens = new List<Token>();
        [RelationshipList("F2685C25-DB7A-4D37-8AB8-9B0C17454B12", "UserID", AutoDeleteReferences = true)]
        public IReadOnlyCollection<Token> Tokens
        {
            get { CheckGet(); return _tokens; }
        }

        private List<PersonalAccessToken> _personalAccessTokens = new List<PersonalAccessToken>();
        [RelationshipList("FA149A61-881D-4FB8-80BB-D27E6DBEAB8D", nameof(UserID), AutoDeleteReferences = true)]
        public IReadOnlyCollection<PersonalAccessToken> PersonalAccessTokens
        {
            get { CheckGet(); return _personalAccessTokens; }
        }
        #endregion
        #region company
        private List<Employee> _employees = new List<Employee>();
        [RelationshipList("FB2B84FE-2069-4ADC-9F0F-5DA46E23DBEA", "UserID")]
        public IReadOnlyCollection<Employee> Employees
        {
            get { CheckGet(); return _employees; }
        }
        #endregion
        #region account
        private List<AccountClearance> _accountClearances = new List<AccountClearance>();
        [RelationshipList("E6F6B024-1F6B-4B1D-BFC2-71C87AD5EA4C", "UserID")]
        public IReadOnlyCollection<AccountClearance> AccountClearances
        {
            get { CheckGet(); return _accountClearances; }
        }
        #endregion
        #region fleet
        private List<fleet.TrainDutyTransaction> _trainDutyTransactions = new List<fleet.TrainDutyTransaction>();
        [RelationshipList("28494AB9-46E0-4DA6-88F6-A826CC3B6342", nameof(fleet.TrainDutyTransaction.UserIDOperator))]
        public IReadOnlyCollection<fleet.TrainDutyTransaction> TrainDutyTransactions
        {
            get { CheckGet(); return _trainDutyTransactions; }
        }

        private List<fleet.LiveLoadSession> _liveLoadSessions = new List<fleet.LiveLoadSession>();
        [RelationshipList("9A3F3912-820E-480C-A488-762ECD624D93", nameof(fleet.LiveLoadSession.UserID))]
        public IReadOnlyCollection<fleet.LiveLoadSession> LiveLoadSessions
        {
            get { CheckGet(); return _liveLoadSessions; }
        }
        #endregion
        #region gov
        private List<Official> _officials = new List<Official>();
        [RelationshipList("59E3FCF3-5FD8-42CE-BA06-0B91E0E40318", "UserID")]
        public IReadOnlyCollection<Official> Officials
        {
            get { CheckGet(); return _officials; }
        }
        #endregion
        #region security
        private List<UserProgram> _userPrograms = new List<UserProgram>();
        [RelationshipList("1714F3DF-C649-4A9D-B7C2-7ED01F676173", "UserID", AutoDeleteReferences = true)]
        public IReadOnlyCollection<UserProgram> UserPrograms
        {
            get { CheckGet(); return _userPrograms; }
        }
        #endregion
        #region tow
        private List<TowTicket> _towTicketsIssued = new List<TowTicket>();
        [RelationshipList("19241116-4D32-42BB-B53C-A243469E1703", nameof(TowTicket.UserIDIssuedTo), AutoRemoveReferences = true)]
        public IReadOnlyCollection<TowTicket> TowTicketsIssued
        {
            get { CheckGet(); return _towTicketsIssued; }
        }

        private List<TowTicket> _towTicketsResponding = new List<TowTicket>();
        [RelationshipList("AA5B7A3F-CEFC-4539-95A0-1A92EE35429E", nameof(TowTicket.UserIDResponding), AutoRemoveReferences = true)]
        public IReadOnlyCollection<TowTicket> TowTicketsResponding
        {
            get { CheckGet(); return _towTicketsResponding; }
        }

        private List<AccessCode> _accessCodes = new List<AccessCode>();
        [RelationshipList("D130873B-8CD5-422E-9613-B5C164ADF0F6", nameof(AccessCode.UserID))]
        public IReadOnlyCollection<AccessCode> AccessCodes
        {
            get { CheckGet(); return _accessCodes; }
        }
        #endregion
        #endregion
    }
}