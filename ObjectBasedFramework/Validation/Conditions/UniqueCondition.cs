﻿using ClussPro.ObjectBasedFramework.DataSearch;
using ClussPro.ObjectBasedFramework.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClussPro.ObjectBasedFramework.Validation.Conditions
{
    public class UniqueCondition : Condition
    {
        private string[] uniqueFields;
        public UniqueCondition(string[] uniqueFields)
        {
            this.uniqueFields = uniqueFields;
        }

        public override bool Evaluate(DataObject dataObject)
        {
            bool shouldRunRule = false;
            foreach(string uniqueField in uniqueFields)
            {
                if (dataObject.IsFieldDirty(uniqueField))
                {
                    shouldRunRule = true;
                    break;
                }
            }

            if (!shouldRunRule)
            {
                return true;
            }

            Search search = new Search(dataObject.GetType());
            SearchConditionGroup conditionGroup = new SearchConditionGroup(SearchConditionGroup.SearchConditionGroupTypes.And);
            foreach(string field in uniqueFields)
            {
                object fieldValue = dataObject.GetField(field).GetValue(dataObject);

                if (fieldValue != null)
                {
                    conditionGroup.SearchConditions.Add(new GenericSearchCondition(dataObject.GetType())
                    {
                        Field = field,
                        SearchConditionType = SearchCondition.SearchConditionTypes.Equals,
                        Value = fieldValue
                    });
                }
                else
                {
                    conditionGroup.SearchConditions.Add(new GenericSearchCondition(dataObject.GetType())
                    {
                        Field = field,
                        SearchConditionType = SearchCondition.SearchConditionTypes.Null
                    });
                }
            }

            if (dataObject.PrimaryKeyField.GetValue(dataObject) != null)
            {
                long? primaryKey = ConvertUtility.GetNullableLong(dataObject.PrimaryKeyField.GetValue(dataObject));

                conditionGroup.SearchConditions.Add(new LongSearchCondition(dataObject.GetType())
                {
                    Field = dataObject.PrimaryKeyField.FieldName,
                    SearchConditionType = SearchCondition.SearchConditionTypes.NotEquals,
                    Value = primaryKey
                });
            }
            search.SearchCondition = conditionGroup;

            return !search.ExecuteExists(null);
        }
    }
}
