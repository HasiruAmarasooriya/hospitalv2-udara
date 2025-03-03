namespace HospitalMgrSystem.Model.Enums
{
    public enum PaymentStatus
    {
        PAID, // If Balence == 0
        PARTIAL_PAID,// If Balence > 0
        NEED_TO_PAY,// If Balence < 0
        OPD,// If payment done by OPD and still not it add on cashier
        NOT_PAID, // Newly created invoiced
        ALL, // All
        ADM, // Admission
        Refund
    }
}
