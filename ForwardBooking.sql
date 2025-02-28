SELECT 
    O.PatientId,              
    O.ConsultantId,                     
	CS.Id as Channeling,
	I.CustomerName,
	I.Id as Invoice,
	P.CashAmount,          
    I.CreateUser,             
    I.ModifiedUser,            
    O.Status AS PaymentStatus, 
    I.InvoiceType,
	O.Id as OPD,
	O.schedularId as schOPD,
	O.CreateDate,
	O.ModifiedDate,
	O.CreatedUser,
	O.ModifiedUser,
	O.Description as OPDDiscription,
	I.InvoiceType
FROM 
    Invoices I  
INNER JOIN 
    InvoiceItems II ON II.InvoiceId = I.Id  
INNER JOIN 
    OPD O ON O.Id = I.ServiceID  
INNER JOIN 
    ChannelingSchedule CS ON CS.Id = O.schedularId  
INNER JOIN 
    Consultants C ON C.Id = CS.ConsultantId  
INNER JOIN 
    Payments P ON P.InvoiceID = I.Id  

WHERE 
    CS.Id = 2571  --channeling schadule id
    AND P.sessionID = 2509 --cashier session id
    AND CAST(I.CreateDate AS DATE) < CAST(CS.DateTime AS DATE)
	--AND O.invoiceType=2
GROUP BY 
    O.PatientId, O.ConsultantId,CS.Id,I.CustomerName,I.Id,P.CashAmount,I.CreateUser, 
    I.ModifiedUser, O.Status, I.InvoiceType,O.ID,O.schedularId,O.CreateDate,O.ModifiedDate,O.CreatedUser,O.ModifiedUser,O.Description,I.InvoiceType;
