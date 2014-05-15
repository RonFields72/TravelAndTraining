<%@ Page Title="" Language="C#" MasterPageFile="~/SWNMaster.Master" AutoEventWireup="true" CodeBehind="RequestConfirmation.aspx.cs" Inherits="SWN.TTR.WebUI.RequestConfirmation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMenu" runat="server">
    <asp:Menu ID="menuMain" runat="server" Orientation="Horizontal" SkinID="SWNMenu"> 
        <Items>
            <asp:MenuItem Text='<%$Resources: SiteMap, Home %>' NavigateUrl="~/Main.aspx" ToolTip='<%$Resources: SiteMap, Home %>'></asp:MenuItem>
        </Items>                                            
    </asp:Menu>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphContents" runat="server">
    <div class="center">
        <h2>Request Submitted</h2>               
        <p class="status">                    
            You have successfully submitted a travel and training request. The reviewer will be notified of your request.<br />Once it has been reviewed, you will receive an email confirmation. Thank you.
        </p>
    </div>
</asp:Content>
