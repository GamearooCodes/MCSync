﻿using ClussPro.ObjectBasedFramework;
using ClussPro.ObjectBasedFramework.DataSearch;
using ClussPro.ObjectBasedFramework.Validation.Conditions;

namespace WebModels.invoicing.Validations
{
    public class IsValidForSentStatusCondition : Condition
    {
        public override bool Evaluate(DataObject dataObject)
        {
            if (!(dataObject is Invoice invoice))
            {
                return false;
            }

            if (invoice.Status != Invoice.Statuses.Sent)
            {
                return true;
            }

            Search<InvoiceLine> invoiceLineSearch = new Search<InvoiceLine>(new LongSearchCondition<InvoiceLine>()
            {
                Field = "InvoiceID",
                SearchConditionType = SearchCondition.SearchConditionTypes.Equals,
                Value = invoice.InvoiceID
            });

            if (!invoiceLineSearch.ExecuteExists(null))
            {
                return false;
            }

            return (invoice.LocationIDTo != null || invoice.GovernmentIDTo != null) &&
                    !string.IsNullOrEmpty(invoice.InvoiceNumber) &&
                    invoice.InvoiceDate != null &&
                    invoice.DueDate != null &&
                    !string.IsNullOrEmpty(invoice.Description) &&
                    invoice.AccountIDTo != null;
        }
    }
}
