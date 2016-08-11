USE [MaaAahwanam]
GO
/****** Object:  StoredProcedure [dbo].[sp_Servicedetails]    Script Date: 10/08/2016 04:35:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[sp_Servicedetails](@RequestId bigint)
as begin
select data2.ResponseId,data2.UpdatedDate,data2.Description,data1.Preferences,data1.Budget,data2.Amount from 
ServiceRequests data1 
join ServiceResponses data2 
on data1.RequestId=data2.RequestId
where data2.requestid=@RequestId
end