<%@ Page Title="[TTR] Request Approval" Language="C#" MasterPageFile="~/SWNMaster.Master" AutoEventWireup="true" CodeBehind="RequestApproval.aspx.cs" Inherits="SWN.TTR.WebUI.Approvers.RequestApproval" %>
<%@ Register Src="~/Controls/RequestSummary.ascx" TagName="RequestSummary" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="server">
     <table class="menu">
        <tbody>
            <tr> 
                <td>
                    <asp:HyperLink ID="lnkbtnHome" runat="server" Text="Home"
                         CssClass="menu" onmouseover="this.className='menumouseover';" onmouseout="this.className='menu';"
                         NavigateUrl="~/Main.aspx" ToolTip="Home"></asp:HyperLink>
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnApprove" runat="server" Text="Approve"
                         CssClass="menu" onmouseover="this.className='menumouseover';" onmouseout="this.className='menu';"
                         OnClick="ApproveRequest"
                         ToolTip="Approve Request"></asp:LinkButton>                                
                </td>
                <td>
                    <asp:LinkButton ID="lnkbtnDeny" runat="server" Text="Deny"
                         CssClass="menu" onmouseover="this.className='menumouseover';" onmouseout="this.className='menu';"
                         OnClick="DenyRequest"
                         ToolTip="Deny Request"></asp:LinkButton>
                </td>                
                <td>
                    <asp:LinkButton ID="lnkbtnFinalApprove" runat="server" Text="Final Approve"
                         CssClass="menu" onmouseover="this.className='menumouseover';" onmouseout="this.className='menu';"
                         OnClick="FinalApproveRequest"
                         ToolTip="Final Approve Request"></asp:LinkButton>            
                </td>
                <td>
                    <asp:HyperLink ID="lnkEditRequest" runat="server" Text="Edit Request"
                         CssClass="menu" onmouseover="this.className='menumouseover';" 
                         onmouseout="this.className='menu';"
                         ToolTip="Edit Request"
                         OnInit="LoadEditRequestLink">
                    </asp:HyperLink>
                </td>
            </tr>
        </tbody>
    </table>  
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContents" runat="server">
    <p class="status">
        <asp:Literal ID="litStatus" runat="server" EnableViewState="false"></asp:Literal>        
        <span class="error"><asp:Literal ID="litError" runat="server" EnableViewState="false"></asp:Literal></span>
    </p>
    
    <table cellpadding="3" cellspacing="3">
        <tr>                  
            <th>Approval/Denial Comments</th>
            <td>
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine"
                    Rows="2" Columns="60"></asp:TextBox>
            </td>
            <td><b>Route To </b> 
                <asp:DropDownList ID="ddlApprovers" runat="server"
                    DataTextField="FullName"
                    DataValueField="EmailAddress"
                    OnInit="LoadApprovers">
                </asp:DropDownList>
            </td>            
            <td>
                <asp:Button ID="btnRoute" runat="server" Text="Route" SkinID="DefaultButton" OnClick="Route" />
            </td>  
        </tr>    
    </table>
    <uc1:RequestSummary ID="ucSummary" runat="server"></uc1:RequestSummary>
</asp:Content>
