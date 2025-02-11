﻿using ClussPro.Base.Data.Query;
using ClussPro.ObjectBasedFramework;
using ClussPro.ObjectBasedFramework.Schema.Attributes;
using ClussPro.ObjectBasedFramework.Validation.Attributes;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebModels.fleet;
using WebModels.invoicing;

namespace WebModels.mesasys
{
    [Table("3A24E2E4-EC2B-4F02-98EA-777B60BDE5C2")]
    [Unique(new[] { nameof(Hash) })]
    public class Item : DataObject
    {
        protected Item() : base() { }

        private long? _itemID;
        [Field("892EFE98-B6F2-42D7-9552-B6ACC2E1A0B9")]
        public long? ItemID
        {
            get { CheckGet(); return _itemID; }
            set { CheckSet(); _itemID = value; }
        }

        private long? _itemNamespaceID;
        [Field("1BFF506C-5CBD-4940-A48A-864C73D7351F")]
        public long? ItemNamespaceID
        {
            get { CheckGet(); return _itemNamespaceID; }
            set { CheckSet(); _itemNamespaceID = value; }
        }

        private ItemNamespace _itemNamespace = null;
        [Relationship("938F34BB-83A0-4BFA-9B40-DF7C44BACBF5")]
        public ItemNamespace ItemNamespace
        {
            get { CheckGet(); return _itemNamespace; }
        }

        private string _name;
        [Field("ECE7AE7B-8D18-4827-AA68-FCD2942BF18D", DataSize = 100)]
        public string Name
        {
            get { CheckGet(); return _name; }
            set { CheckSet(); _name = value; }
        }

        private byte[] _image;
        [Field("71C6EDE5-3597-4E42-BFE9-546084415E17")]
        public byte[] Image
        {
            get { CheckGet(); return _image; }
            set { CheckSet(); _image = value; }
        }

        private byte[] _hash;
        [Field("4BB393D6-D7F3-4548-A285-8352FD8C9D4B")]
        public byte[] Hash
        {
            get { CheckGet(); return _hash; }
            set { CheckSet(); _hash = value; }
        }

        protected override bool PreSave(ITransaction transaction)
        {
            if (IsFieldDirty(nameof(ItemNamespaceID)) || IsFieldDirty(nameof(Image)))
            {
                string hashString = string.Format("{0}", ItemNamespaceID);
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(hashString);

                byte[] prehashBytes = new byte[utf8Bytes.Length + Image.Length];
                Array.Copy(utf8Bytes, prehashBytes, utf8Bytes.Length);
                Array.Copy(Image, 0, prehashBytes, utf8Bytes.Length, Image.Length);

                using (MD5 md5 = MD5.Create())
                {
                    Hash = md5.ComputeHash(prehashBytes);
                }
            }

            return base.PreSave(transaction);
        }

        #region Relationships
        #region fleet
        private List<RailcarLoad> _railcarLoads = new List<RailcarLoad>();
        [RelationshipList("7B69B73F-36B2-4C22-B9DF-8A992415AF26", nameof(RailcarLoad.ItemID))]
        public IReadOnlyCollection<RailcarLoad> RailcarLoads
        {
            get { CheckGet(); return _railcarLoads; }
        }
        #endregion
        #region invoicing
        private List<InvoiceLine> _invoiceLines = new List<InvoiceLine>();
        [RelationshipList("AC6E6BF0-5481-4C13-844C-223161B4A750", nameof(InvoiceLine.ItemID))]
        public IReadOnlyCollection<InvoiceLine> InvoiceLines
        {
            get { CheckGet(); return _invoiceLines; }
        }
        #endregion
        #endregion
    }
}
