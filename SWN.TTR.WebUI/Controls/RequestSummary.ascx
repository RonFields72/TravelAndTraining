<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestSummary.ascx.cs" Inherits="SWN.TTR.WebUI.Controls.RequestSummary" EnableViewState="false" %>
<div style="color: Navy; padding: 2px 5px 0px 5px;">
    <asp:Repeater ID="repSummary" runat="server" OnItemDataBound="repSummary_ItemDataBound">
        <HeaderTemplate>
            <div style="font-size: 1.5em; font-weight: bold; margin-bottom: 0px;">
                Request Summary            
            </div>   
            <hr />            
        </HeaderTemplate>
        <ItemTemplate>
            <asp:Repeater ID="repApprovers" runat="server">
                <HeaderTemplate>  
                    <div style="width: 90%;">                 
                        <div style="font-weight: bold; font-size: 1.2em; background-color: #104EBB; color: White; padding: 3px 0px 3px 3px;">Approvals</div>
                        <table style="background-color: White; border: Ridge 2px White; width: 100%;">                            
                </HeaderTemplate>
                <ItemTemplate>
                            <tr>
                                <td style="width: 45%;"><%# ((System.Data.DataRowView)Container.DataItem)["Reviewer_Full_Name"] %> <b><%# ((System.Data.DataRowView)Container.DataItem)["Reviewal_Status_Description"] %></b> on <%# ((System.Data.DataRowView)Container.DataItem)["Reviewed_On"] %>.</td>                                
                                <td><%# ((System.Data.DataRowView)Container.DataItem)["Comments"] %></td>
                            </tr>
                </ItemTemplate>  
                <FooterTemplate>
                        </table> 
                    </div>                   
                </FooterTemplate>          
            </asp:Repeater> 
            <br />            
            <div style="background-color: White; padding: 2px 2px 2px 2px;">
                <asp:Panel ID="pnlEvent" runat="server" GroupingText="<b>Event Details</b>" EnableViewState="false">                
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th>Event Name:</th>
                            <td colspan="5"><%# ((System.Data.DataRowView)Container.DataItem)["Event_Name"] %></td>                        
                        </tr>
                        <tr>
                            <th>Event Start Date:</th>
                            <td><%# ((DateTime)((System.Data.DataRowView)Container.DataItem)["Event_Start_Date"]).ToShortDateString() %></td>
                            <th>Event End Date:</th>
                            <td><%# ((DateTime)((System.Data.DataRowView)Container.DataItem)["Event_End_Date"]).ToShortDateString() %></td>
                            <th>Event Location:</th>
                            <td><%# ((System.Data.DataRowView)Container.DataItem)["Event_Location_City"] %>, <%# ((System.Data.DataRowView)Container.DataItem)["Event_Location_State"] %></td>
                        </tr>                    
                        <tr>
                            <th colspan="2">Business Purpose of Event:</th>
                            <td colspan="4"><%# ((System.Data.DataRowView)Container.DataItem)["Event_Business_Purpose"] %></td>                                                   
                        </tr>
                        <tr>
                            <th colspan="2">Associated Project Name:</th>
                            <td colspan="4"><%# ((System.Data.DataRowView)Container.DataItem)["Event_Associated_Project"] %></td>   
                        </tr>                    
                    </table>    
                </asp:Panel>                
                <asp:Panel ID="pnlTraining" runat="server" GroupingText="<b>Training Details</b>" EnableViewState="false">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th colspan="2">Class/Seminar/Conference:</th>
                            <td colspan="4"><%# ((System.Data.DataRowView)Container.DataItem)["Training_Name"] %></td>
                        </tr>
                        <tr>
                            <th>Start Date:</th>
                            <td><%# ((DateTime)((System.Data.DataRowView)Container.DataItem)["Training_Start_Date"]).ToShortDateString() %></td>
                            <th>End Date:</th>
                            <td><%# ((DateTime)((System.Data.DataRowView)Container.DataItem)["Training_End_Date"]).ToShortDateString() %></td>
                            <th>Estimated Cost</th>
                            <td>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Training_Total_Cost"]).ToString("0") %>.00</td>
                        </tr>
                    </table>
                </asp:Panel>                
                <asp:Panel ID="pnlTravel" runat="server" GroupingText="<b>Travel Details</b>" EnableViewState="false">
                    <table cellpadding="3" cellspacing="3">
                        <tr>
                            <th>Destination:</th>
                            <td colspan="3"><%# ((System.Data.DataRowView)Container.DataItem)["Travel_Destination_City"] %>, <%# ((System.Data.DataRowView)Container.DataItem)["Travel_Destination_State"] %></td>
                        </tr>                        
                    </table>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlAir" runat="server" GroupingText="<b>Air</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>Destination Mode:</th>
                                    <td><%# ((System.Data.DataRowView)Container.DataItem)["Travel_Flight_Destination_Description"] %></td>
                                    <th>Return Mode:</th>
                                    <td><%# ((System.Data.DataRowView)Container.DataItem)["Travel_Flight_Return_Description"] %></td>
                                    <th>Estimated Cost:</th>
                                    <td>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Travel_Flight_Total_Cost"]).ToString("0") %>.00</td>
                                </tr>                           
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlLodging" runat="server" GroupingText="<b>Lodging</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th># Of Nights:</th>
                                    <td><%# ((System.Data.DataRowView)Container.DataItem)["Travel_Lodging_Number_Of_Nights"] %></td>
                                    <th>Estimated Cost:</th>
                                    <td>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Travel_Lodging_Total_Cost"]).ToString("0") %>.00</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                    <div style="margin: 8px 5px 0px 5px;">
                        <asp:Panel ID="pnlGround" runat="server" GroupingText="<b>Ground</b>" EnableViewState="false">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <th>Ground Mode(s):</th>
                                    <td>
                                        <asp:Repeater ID="repGround" runat="server">
                                            <ItemTemplate>
                                                <%# ((System.Data.DataRowView)Container.DataItem)["Ground_Transportation_Description"] %>
                                            </ItemTemplate>
                                            <SeparatorTemplate>
                                            ,
                                            </SeparatorTemplate>
                                        </asp:Repeater>
                                    </td>
                                    <th>Estimated Cost:</th>
                                    <td>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Travel_Ground_Total_Cost"]).ToString("0") %>.00</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </asp:Panel>                
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>General Comments:</th>
                        <td><%# ((System.Data.DataRowView)Container.DataItem)["Travel_Comments"] %></td>
                        <th>Estimated Cost (General Comments):</th>
                        <td>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Misc_Costs"]).ToString("0") %>.00</td>
                    </tr>                
                </table>
                <hr />
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <th>Total Estimated Cost of Request:</th>
                        <th>$<%# ((System.Decimal)((System.Data.DataRowView)Container.DataItem)["Total_Cost"]).ToString("0") %>.00</th>
                    </tr>                
                </table> 
            </div>   
        </ItemTemplate>
        <FooterTemplate>
           
        </FooterTemplate>
        <SeparatorTemplate>
            
        </SeparatorTemplate>
    </asp:Repeater>
</div>