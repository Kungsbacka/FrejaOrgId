namespace FrejaOrgId.Model.GetOne;

public enum TransactionStatus
{
    Started, // The transaction has been started but not yet delivered to the user's Freja eID application.
    Delivered_To_Mobile, // The Freja eID app has received the transaction.
    Canceled, // The end user declined the 'Add Organisation ID' request.
    Rp_Canceled, // The 'Add Organisation ID' request was sent to the user but cancelled by the Relying Party before the user could respond.
    Expired, // The 'Add Organisation ID' request was not approved by the user within the set time frame.
    Approved // The end user has approved the 'Add Organisation ID' request.
};
